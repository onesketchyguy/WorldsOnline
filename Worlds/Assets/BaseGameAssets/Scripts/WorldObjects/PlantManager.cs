using UnityEngine;
using Mirror;

namespace Worlds.WorldObjects
{
    public class PlantManager : NetworkBehaviour
    {
        public GameObject toppledEffect;

        public SkinnedMeshRenderer dirt;

        public SkinnedMeshRenderer plant;

        public float growthRate = 1;

        [SyncVar(hook = nameof(SetSize))] private float plantGrowth;

        [SyncVar(hook = nameof(SetTilled))] private bool tilled;

        private void OnValidate()
        {
            if (growthRate < 0.01f) growthRate = 0.01f;
        }

        public override void OnStartServer()
        {
            base.OnStartServer();

            InvokeRepeating(nameof(UpdateSize), 0, growthRate);
        }

        private void UpdateSize()
        {
            if (tilled)
            {
                if (plantGrowth >= 100) plantGrowth = 100;
                else plantGrowth++;
            }
        }

        private void SetSize(float oldValue, float newValue)
        {
            plant.SetBlendShapeWeight(0, 100 - newValue);
        }

        private void SetTilled(bool oldValue, bool newValue)
        {
            if (newValue == false)
            {
                if (plantGrowth > 1)
                {
                    ObjectManager.GetObject(toppledEffect, transform.position);
                    plantGrowth = 0;
                }
            }

            var val = newValue == true ? 100 : 0;
            dirt.SetBlendShapeWeight(0, val);
        }

        private void OnTriggerEnter(Collider collision)
        {
            tilled = !tilled;
        }
    }
}