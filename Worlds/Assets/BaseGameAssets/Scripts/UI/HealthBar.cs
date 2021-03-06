﻿using UnityEngine;
using UnityEngine.UI;

namespace Worlds.UI
{
    public class HealthBar : MonoBehaviour
    {
        public Slider healthBar;

        public Worlds.HealthManager HealthManager;

        public Gradient healthGradient;

        private void Start()
        {
            HealthManager.OnHealthModifiedCallback += HealthModified;
            HealthModified(0);
        }

        private void HealthModified(float modAmount)
        {
            var val = HealthManager.GetHealthValue();

            healthBar.value = val;
            healthBar.fillRect.GetComponent<Image>().color = healthGradient.Evaluate(val);
        }
    }
}