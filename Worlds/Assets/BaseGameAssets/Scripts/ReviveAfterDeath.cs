using Mirror;
using UnityEngine;

namespace Worlds
{
    public class ReviveAfterDeath : NetworkBehaviour
    {
        [Tooltip("The health to track.")]
        public HealthManager health;

        public float reviveTime = 1;
        public float randomRange = 0.35f;

        private void OnValidate()
        {
            if (reviveTime - randomRange < 0) randomRange = reviveTime;
            if (reviveTime + randomRange < 0) randomRange = -reviveTime;
        }

        private void Start()
        {
            health.onDeathCallback += () =>
            {
                // Revive after X seconds
                Invoke(nameof(Revive), Random.Range(reviveTime - randomRange,
                    reviveTime + randomRange));
            };
        }

        public void Revive()
        {
            health.RpcReset();
        }
    }
}