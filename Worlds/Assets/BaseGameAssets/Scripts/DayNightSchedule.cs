using UnityEngine;

public class DayNightSchedule : MonoBehaviour
{
    public Light directionalLight;
    public LightingConditionPreset lightingPreset;

    public float timeOfDay;

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
        float realTimePercent = (float)(System.DateTime.Now.TimeOfDay.TotalMinutes / 1440f);

        timeOfDay = (realTimePercent);

        // Time == TimeOfDay * 24

        UpdateLighting(timeOfDay);
    }
}