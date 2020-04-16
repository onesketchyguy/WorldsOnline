using Mirror;
using UnityEngine;

namespace World
{
    public class HealthManager : NetworkBehaviour
    {
        public delegate void OnDeath();

        public delegate void OnHealthModified(float modAmount);

        public OnDeath onDeathCallback;
        public OnHealthModified OnHealthModifiedCallback;

        [SyncVar] public int maxHealth = 100;
        [SyncVar] private float currentHealth = 100;

        [ServerCallback]
        public void Reset()
        {
            ModifyHealth(maxHealth - currentHealth);
        }

        public float GetHealthValue()
        {
            return currentHealth / maxHealth;
        }

        [ServerCallback]
        public void ModifyHealth(float modAmount)
        {
            currentHealth += modAmount;

            if (currentHealth <= 0)
            {
                currentHealth = 0;

                if (onDeathCallback != null)
                    onDeathCallback.Invoke();
            }

            if (OnHealthModifiedCallback != null)
                OnHealthModifiedCallback.Invoke(modAmount);
        }
    }
}