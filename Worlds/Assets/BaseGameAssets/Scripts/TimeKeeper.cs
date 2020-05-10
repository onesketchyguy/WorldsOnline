using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeKeeper : MonoBehaviour
{
    public Image clockFillImage;
    public Text clockText;

    private DayNightSchedule schedule;

    private void Start()
    {
        schedule = FindObjectOfType<DayNightSchedule>();
    }

    private void Update()
    {
        clockFillImage.fillAmount = schedule.timeOfDay;

        var timeOfDay = System.Math.Round(schedule.timeOfDay * 24, 2);

        clockText.text = $"{timeOfDay}";
    }
}