using Mirror;

namespace Worlds
{
    public class HealthManager : NetworkBehaviour
    {
        public delegate void OnDeath();

        public delegate void OnHealthModified(float modAmount);

        public OnDeath onDeathCallback;
        public OnHealthModified OnHealthModifiedCallback;

        [SyncVar] public int maxHealth = 100;
        [SyncVar] private float currentHealth = 100;

        public float GetCurrentHealth()
        {
            return currentHealth;
        }

        [ClientRpc]
        public void RpcReset()
        {
            RpcModifyHealth(maxHealth - currentHealth);
        }

        public float GetHealthValue()
        {
            return currentHealth / maxHealth;
        }

        [ClientRpc]
        public void RpcModifyHealth(float modAmount)
        {
            if (currentHealth > maxHealth) currentHealth = maxHealth;

            if (modAmount < 0 && currentHealth <= 0) return; // Beating a dead horse

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