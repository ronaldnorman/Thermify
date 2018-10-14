using System;
using System.Collections.Generic;
using System.Text;

namespace Thermify.DomainEvents
{
    /// <summary>
    /// A domain event base class that fires and notifies clients when the temperature drops. 
    /// </summary>
    /// <remarks>This class controls and centralizes the logic flow for all domain events. Decision of whether to fire or not is delegated to the subclasses, the specific event types.</remarks>
    internal abstract class TemperatureEventBase
    {
        protected readonly List<ThresholdObservation> _observations = new List<ThresholdObservation>();
        protected IThermometer _thermometer;

        public TemperatureEventBase(IThermometer thermometer)
        {
            _thermometer = thermometer;
            _thermometer.OnNewReadingReceived += _thermometer_OnNewReadingReceived;
        }

        /// <summary>
        /// Template method to ask the subclass if the threshold was reached
        /// </summary>
        /// <param name="observation">The observation with all the necessary info</param>
        /// <param name="e">The current reading received</param>
        /// <returns></returns>
        protected abstract bool IsThresholdReached(ThresholdObservation observation, NewReadingReceivedEventArgs e);

        /// <summary>
        /// When we receive a new reading, check all the observer clients to see if we need to send any threshold notifications
        /// </summary>
        /// <param name="sender">The sender themometer</param>
        /// <param name="e">The <see cref="NewReadingReceivedEventArgs"/> reading</param>
        private void _thermometer_OnNewReadingReceived(object sender, NewReadingReceivedEventArgs e)
        {
            foreach (var observation in _observations)
            {
                var t = IsThresholdReached(observation, e);
                var f = IsFirstReading(observation, e);
                var d = IsReadingDifferentThanLastOne(observation, e);

                // Ask subclass if we should send the notification, and keep the flow steps here (Template Method).
                if (IsThresholdReached(observation, e) &&
                (IsFirstReading(observation, e) ||
                (IsReadingDifferentThanLastOne(observation, e) &&
                IsSensitivityVarianceMet(observation, e))))
                {
                    // Set the current temperature
                    observation.Threshold.CurrentReading = e.Reading;
                    
                    // Notify
                    observation.CallBack(observation.Threshold);

                    // Update last notification temperature
                    observation.Threshold.LastNotificationTemperature = e.Reading;
                }
            }
        }

        /// <summary>
        /// Adds a new observer to this event
        /// </summary>
        /// <param name="observation">Contains all the information needed</param>
        /// <returns></returns>
        internal IDisposable Add(ThresholdObservation observation)
        {
            // Only add a new observation if not already exists
            if (!_observations.Contains(observation))
            {
                _observations.Add(observation);
            }

            return new Unsubscriber<ThresholdInfo>(_observations, observation);
        }

        // Common business logic snippets to all
        protected bool IsFirstReading(ThresholdObservation observation, NewReadingReceivedEventArgs e) => !observation.Threshold.LastNotificationTemperature.HasValue;
        protected bool IsReadingDifferentThanLastOne(ThresholdObservation observation, NewReadingReceivedEventArgs e) => e.Reading != observation.Threshold.LastNotificationTemperature;
        protected bool IsSensitivityVarianceMet(ThresholdObservation observation, NewReadingReceivedEventArgs e) => Math.Abs(e.Reading - observation.Threshold.LastNotificationTemperature.Value) >= observation.Threshold.Sensitivity;
    }
}
