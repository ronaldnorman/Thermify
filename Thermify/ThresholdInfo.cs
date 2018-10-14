namespace Thermify
{
    /// <summary>
    /// Holds information of the threshold to be shared with clients
    /// </summary>
    public class ThresholdInfo
    {
        internal ThresholdInfo(
            string name,
            double temperature,
            TemperatureScale scale,
            double sensitivity,
            double? lastNotificationTemperature
            )
        {
            Name = name;
            Temperature = temperature;
            Scale = scale;
            Sensitivity = sensitivity;
            LastNotificationTemperature = lastNotificationTemperature;
        }


        /// <summary>
        /// Name of the threshold such as 'boiling' or 'freezing'
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The temperature number of the threshold 
        /// </summary>
        public double Temperature { get; }

        // The temperature last notified to the observer client
        public double? LastNotificationTemperature { get; internal set; }

        /// <summary>
        /// The temperature scale of the threshold such as Celsuis
        /// </summary>
        public TemperatureScale Scale { get; }

        /// <summary>
        /// The sensitify of the unit delta of when the notification should be fired
        /// </summary>
        public double Sensitivity { get; }

        // The current reading that triggered the notification
        public double CurrentReading { get; internal set; }
    }
}
