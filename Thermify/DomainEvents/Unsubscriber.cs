using System;
using System.Collections.Generic;
using System.Text;

namespace Thermify.DomainEvents
{
    /// <summary>
    /// Unsubscribes the observer client when the Dispose is called so notifications stop to that client
    /// </summary>
    /// <typeparam name="Threshold">Threshold info</typeparam>
    internal class Unsubscriber<Threshold> : IDisposable
    {
        private List<ThresholdObservation> _observations;
        private ThresholdObservation _observation;

        internal Unsubscriber(List<ThresholdObservation> observations,
            ThresholdObservation observation)
        {
            _observations = observations;
            _observation = observation;
        }

        /// <summary>
        /// Cancels notification of the observer client by the thermometer
        /// </summary>
        public void Dispose()
        {
            if (_observations.Contains(_observation))
                _observations.Remove(_observation);
        }
    }
}
