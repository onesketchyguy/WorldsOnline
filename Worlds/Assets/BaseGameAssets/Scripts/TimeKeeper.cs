using UnityEngine;
using UnityEngine.UI;

namespace Worlds.DayNight
{
    public class TimeKeeper : MonoBehaviour
    {
        public Image hourHand;
        public Text clockText;

        /// <summary>
        /// Wether or not to use a 24 or a AM clock.
        /// If true it will use an amPM clock.
        /// </summary>
        public static bool amPM = true;

        private void Update()
        {
            //hourHand.fillAmount = DayNightSchedule.TOD;
            hourHand.transform.rotation = Quaternion.Euler(0, 0, -(DayNightSchedule.TOD * 360));

            // By using all four decimcal places we are ensuring we are using an accurate timestamp
            var timeOfDay = (float)System.Math.Round(DayNightSchedule.TOD * 24, 4);

            // Convert the time into a readable template

            // Minutes
            int minute = 0;
            if (timeOfDay != System.Math.Floor(timeOfDay)) // Check to see if the tod has a decimal value (rarely will it not)
            {
                // Grab just the decimal value from the tod and convert it to minutes
                minute = Mathf.FloorToInt((timeOfDay - (float)System.Math.Truncate(timeOfDay)) * 60);
            }

            // Hours
            var hour = (amPM ? timeOfDay %= 12 : timeOfDay);
            if (timeOfDay < 1 && amPM)
                hour = 12;
            hour = Mathf.FloorToInt(hour);

            // Generate a string to plug into the clock text
            var TOD_STRING =
                $"{(hour.ToString().Length > 1 ? hour.ToString() : $"0{hour}")}:" +
                $"{(minute.ToString().Length > 1 ? minute.ToString() : $"0{minute}")}" +
                $"{(amPM ? (timeOfDay >= 12 ? "PM" : "AM") : "")}";

            clockText.text = TOD_STRING; // Apply our changes
        }
    }
}