using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Thermify.Test
{
    [TestClass]
    public class ThermometerTest
    {
        private IThermometer _thermometer;

        [TestInitialize]
        public void TestInitialize()
        {
            _thermometer = new ThermometerAggregate(TemperatureScale.Celsius);
        }

        [DataTestMethod]
        [DataRow(100, 100, 0, TemperatureChangeType.Drop, default(ThresholdInfo), DisplayName = "ShouldNotNotify_WhenTemperatureReadingIsSameAndNotFirstTime")]
        [DataRow(95, 100, 10, TemperatureChangeType.Drop, default(ThresholdInfo), DisplayName = "ShouldNotNotify_WhenTemperatureDropsAndNotFirstTimeAndDoesNotExceedSensitivity")]
        [DataRow(95, 100, 10, TemperatureChangeType.Rise, default(ThresholdInfo), DisplayName = "ShouldNotNotify_WhenTemperatureRisesAndNotFirstTimeAndDoesNotExceedSensitivity")]
        [DataRow(101, 100, 0, TemperatureChangeType.Drop, default(ThresholdInfo), DisplayName = "ShouldNotNotify_WhenTemperatureDropsButDoesNotHitThreshold")]
        [DataRow(99, 100, 0, TemperatureChangeType.Rise, default(ThresholdInfo), DisplayName = "ShouldNotNotify_WhenTemperatureRisesButDoesNotHitThreshold")]
        public void NotifyOnTemperatureChange_ShouldNotNotify(double triggerReading, double threshold, double sensitivityVariance, TemperatureChangeType changeType, ThresholdInfo result)
        {
            // Arrange
            // Set the reading before we request a notfication so it's not the first time
            _thermometer.SetReading(triggerReading);

            // Act
            var actualReading = default(ThresholdInfo);
            _thermometer.NotifyOnTemperatureChange(changeType, "Boiling", threshold, sensitivityVariance, (t) => { actualReading = t; });
            _thermometer.SetReading(triggerReading);

            // Assert
            // Should be null and should not have been called
            Assert.AreEqual(result, actualReading);
        }

        [DataTestMethod]
        [DataRow(100, 100, 0, TemperatureChangeType.Drop, 100, DisplayName = "ShouldNotNotify_WhenTemperatureReadingIsSameAndNotFirstTime")]
        [DataRow(95, 100, 0, TemperatureChangeType.Drop, 95, DisplayName = "ShouldNotify_WhenTemperatureDropsWithZeroSensitivity")]
        [DataRow(105, 100, 0, TemperatureChangeType.Rise, 105, DisplayName = "ShouldNotify_WhenTemperatureRisesWithZeroSensitivity")]
        [DataRow(115, 100, 10, TemperatureChangeType.Rise, 115, DisplayName = "ShouldNotify_WhenTemperatureRisesAndExceedsSensitivity")]
        public void NotifyOnTemperatureChange_ShouldNotify(double triggerReading, double threshold, double sensitivityVariance, TemperatureChangeType changeType, double result)
        {
            // Act
            var actualReading = default(ThresholdInfo);
            _thermometer.NotifyOnTemperatureChange(changeType, "Boiling", threshold, sensitivityVariance, (t) => { actualReading = t; });
            _thermometer.SetReading(triggerReading);

            // Assert
            // Should be null and should not have been called
            Assert.AreEqual(triggerReading, actualReading.CurrentReading);
        }
    }
}

