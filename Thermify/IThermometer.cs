using System;

namespace Thermify
{
    public interface IThermometer
    {
        event EventHandler<NewReadingReceivedEventArgs> OnNewReadingReceived;
        
        /// <summary>
        /// Temperature scale such as Fahrenheit
        /// </summary>
        TemperatureScale Scale { get; }

        /// <summary>
        /// Sets the reading.
        /// </summary>
        /// <param name="reading">The reading</param>
        /// <remarks>Must be in the same temperature scale as the one set when constructing <see cref="ThermometerAggregate"/></remarks>
        void SetReading(double reading);
        
        /// <summary>
        /// Gets the current temperature reading of the thermometer in the requeste scale
        /// </summary>
        double? GetReading(TemperatureScale scale);

        /// <summary>
        /// Adds a new observer to be notified when a specific temperature drop or rise threshold has been reached
        /// </summary>
        /// <param name="name">The name of this notification and threshold</param>
        /// <param name="thresholdTemperature">The temperature reading to be notified at</param>
        /// <param name="sensitivity">The sensitivity of when to notify</param>
        /// <param name="observer">The client to be notified that implements <see cref="IObserver{T}"/></param>
        /// <returns>An <see cref="IDisposable" />to enable cancellation of notificaiton for that observer</returns>
        IDisposable NotifyOnTemperatureChange(
            TemperatureChangeType changeType,
            string name,
            double thresholdTemperature,
            double sensitivity,
            Action<ThresholdInfo> callBack
            );
    }
}