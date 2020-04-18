using Mirror;
using UnityEngine;

namespace World.Enemies
{
    public class EnemySpawner : NetworkBehaviour
    {
        public NetworkIdentity enemyPrefab;

        public override void OnStartServer()
        {
            for (int i = 0; i < 10; i++)
                SpawnPrize();
        }

        public void SpawnPrize()
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-19, 20), 1, Random.Range(-19, 20));

            var newPrize = ObjectPool.localInstance.GetObject(enemyPrefab.gameObject, spawnPosition);
            newPrize.name = enemyPrefab.name;
        }
    }
}