using Mirror;

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