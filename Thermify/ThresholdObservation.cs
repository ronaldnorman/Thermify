using System;

namespace Thermify
{
    internal class ThresholdObservation
    {
        /// <summary>
        /// Hold information about each obervation, i.e., the observer and the threshold info
        /// </summary>
        /// <param name="callBack"></param>
        /// <param name="threshold"></param>
        public ThresholdObservation(Action<ThresholdInfo> callBack,
            ThresholdInfo threshold)
        {
            CallBack = callBack;
            Threshold = threshold;
        }

        /// <summary>
        /// The observer to notify
        /// </summary>
        public Action<ThresholdInfo> CallBack { get; }

        /// <summary>
        /// Information about the specific threshold
        /// </summary>
        public ThresholdInfo Threshold { get; }
    }
}
