using System;
using System.Collections.Generic;
using System.Text;

namespace Thermify.DomainEvents
{
    /// <summary>
    /// Encapsulates the temperature dropping business logic 
    /// </summary>
    internal class TemperatureDropped : TemperatureEventBase
    {
        public TemperatureDropped(IThermometer thermometer) 
            : base(thermometer)
        {
        }

        // Check if we've hit the threshold
        protected override bool IsThresholdReached (ThresholdObservation observation, NewReadingReceivedEventArgs e) => e.Reading <= observation.Threshold.Temperature;
    }
}
