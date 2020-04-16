using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WorldsUI
{
    public class HealthBar : MonoBehaviour
    {
        public Slider healthBar;

        public World.HealthManager HealthManager;

        public Gradient healthGradient;

        private void Start()
        {
            HealthManager.OnHealthModifiedCallback += HealthModified;
            HealthManager.Reset();
        }

        private void HealthModified(float modAmount)
        {
            var val = HealthManager.GetHealthValue();

            healthBar.value = val;
            healthBar.fillRect.GetComponent<Image>().color = healthGradient.Evaluate(val);
        }
    }
}