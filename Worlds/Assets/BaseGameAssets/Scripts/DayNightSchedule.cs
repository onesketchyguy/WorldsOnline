using UnityEngine;

namespace Worlds.DayNight
{
    [ExecuteAlways]
    public class DayNightSchedule : MonoBehaviour
    {
        public Light directionalLight;
        public LightingConditionPreset lightingPreset;

        [SerializeField] private float timeOfDay = 12;

        [SerializeField] [Tooltip("Days per real life day")] private float dayDivider = 8;

        private void OnValidate()
        {
            if (timeOfDay > 24) timeOfDay = 0;
            if (timeOfDay < 0) timeOfDay = 24;
        }

        public bool debug = true;

        /// <summary>
        /// The current time of day percentage.
        /// </summary>
        public static float TOD;

        /// <summary>
        /// Sets the lighting for a specified time of day in percentage.
        /// </summary>
        /// <param name="tod">Time of day %</param>
        private void UpdateLighting(float tod)
        {
            RenderSettings.ambientLight = lightingPreset.ambientColor.Evaluate(tod);
            //RenderSettings.fogColor = lightingPreset.fogColor.Evaluate(tod);
            //RenderSettings.fogDensity = lightingPreset.fogIntensity.Evaluate(tod);

            directionalLight.color = lightingPreset.directionalColor.Evaluate(tod);
            directionalLight.transform.localRotation = Quaternion.Euler((tod * 360f) - 90, 170, 0);
        }

        private void Update()
        {
#if UNITY_EDITOR
            TOD = timeOfDay / 24;

            UpdateLighting(TOD);
            if (debug) return;
#endif

            var divider = (1440 / dayDivider);
            var minute = System.DateTime.Now.TimeOfDay.TotalMinutes % divider;

            float realTimePercent = (float)(minute / divider);

            TOD = (realTimePercent);
            timeOfDay = TOD * 24;

            UpdateLighting(TOD);
        }
    }
}