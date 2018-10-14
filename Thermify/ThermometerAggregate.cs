using System;
using System.Collections.Generic;
using Thermify.DomainEvents;

namespace Thermify
{
    /// <summary>
    /// Main domain aggregate class that handles the thermometer functionality
    /// </summary>
    public class ThermometerAggregate : IThermometer
    {
        private readonly TemperatureDropped _temperatureDropped;
        private readonly TemperatureRose _temperatureRose;
        private double? _reading;
        internal event EventHandler<NewReadingReceivedEventArgs> OnNewReadingReceived;
        private object _objectLock = new Object();
        private readonly TemperatureScale _scale;

        /// <summary>
        /// Constructs a themometer 
        /// </summary>
        /// <param name="supportedScales">A <see cref="Dictionary{TKey, TValue}"/> object containing the scale names as keys with its equivalen in celcius as the base.</param>
        /// <remarks>For example, you can enter "Fahrenheit" as one entry in the dictionary with -17.2222 as its celius equivalent</remarks>
        public ThermometerAggregate(TemperatureScale scale)
        {
            _scale = scale;
            _temperatureDropped = new TemperatureDropped(this);
            _temperatureRose = new TemperatureRose(this);
        }

        /// <summary>
        /// Allows internal domain events to get notified when new readings are received.
        /// </summary>
        event EventHandler<NewReadingReceivedEventArgs> IThermometer.OnNewReadingReceived
        {
            add
            {
                lock (_objectLock)
                {
                    OnNewReadingReceived += value;
                }
            }

            remove
            {
                lock (_objectLock)
                {
                    OnNewReadingReceived -= value;
                }
            }
        }

        public TemperatureScale Scale => _scale;

        /// <summary>
        /// Sets or gets the current temperature reading of the thermometer
        /// </summary>
        public void SetReading(double reading)
        {
            _reading = reading;
            OnNewReadingReceived?.Invoke(this, new NewReadingReceivedEventArgs { Reading = reading });
        }

        public double? GetReading(TemperatureScale requestedScale)
        {
            // No reading set yet?
            if (_reading == null)
            {
                return null;
            }

            if (requestedScale == TemperatureScale.Fahrenheit)
            {
                return _scale == TemperatureScale.Fahrenheit ? _reading : (_reading * 9 / 5) + 32;
            }

            if (requestedScale == TemperatureScale.Celsius)
            {
                return _scale == TemperatureScale.Celsius ? _reading : (1 - 32) * 5 / 9;
            }

            throw new ArgumentOutOfRangeException("Only Celsius and Fahrenheit are supported temperature scales.");
        }

        /// <summary>
        /// Adds a new observer to be notified when a specific temperature drop or rise threshold has been reached
        /// </summary>
        /// <param name="name">The name of this notification and threshold</param>
        /// <param name="thresholdTemperature">The temperature reading to be notified at</param>
        /// <param name="sensitivity">The sensitivity of when to notify</param>
        /// <param name="observer">The client to be notified that implements <see cref="IObserver{T}"/></param>
        /// <returns>An <see cref="IDisposable" />to enable cancellation of notificaiton for that observer</returns>
        public IDisposable NotifyOnTemperatureChange(
            TemperatureChangeType changeType,
            string name,
            double thresholdTemperature,
            double sensitivity,
            Action<ThresholdInfo> callBack
            )
        {
            if (changeType == TemperatureChangeType.Drop)
            {
                return _temperatureDropped.Add(
                         new ThresholdObservation(
                             callBack,
                             new ThresholdInfo(name, thresholdTemperature, _scale, sensitivity, _reading)));
            }

            if (changeType == TemperatureChangeType.Rise)
            {
                return _temperatureRose.Add(
                         new ThresholdObservation(
                             callBack,
                             new ThresholdInfo(name, thresholdTemperature, _scale, sensitivity, _reading)));
            }

            throw new ArgumentOutOfRangeException("Temperature change type was not recognized.");
        }
    }
}
