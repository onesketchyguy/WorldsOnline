using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Worlds.Enemies
{
    public class EnemyManager : NetworkBehaviour
    {
        public NetworkIdentity[] enemyPrefabs;

        [Range(0, 100)]
        public int numberToSpawn = 10;

        [Range(1, 100)]
        public float spawnRadius = 20;

        [Range(0, 100)]
        public float antiSpawnRadius = 3;

        private List<System.Action> UpdateActions = new List<System.Action>();

        private List<BasicEnemyBehaviour> enemies = new List<BasicEnemyBehaviour>();

        public override void OnStartServer()
        {
            for (int i = 0; i < numberToSpawn; i++)
                SpawnEnemy(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)].gameObject);

            InvokeRepeating(nameof(UpdateServer), 0, Time.deltaTime);
        }

        public void SpawnEnemy(GameObject enemyToSpawn)
        {
            Vector3 spawnPosition = transform.position;
            while (Vector3.Distance(transform.position, spawnPosition) <= antiSpawnRadius)
                spawnPosition = transform.position + new Vector3(Random.Range(-spawnRadius, spawnRadius), 1, Random.Range(-spawnRadius, spawnRadius));

            var go = ObjectManager.GetObject(enemyToSpawn.gameObject, spawnPosition);
            go.name = enemyToSpawn.name;

            var enemy = go.GetComponent<BasicEnemyBehaviour>();
            enemy.Initialize();

            // Ensure this enemy is recieving updates
            System.Action serverUpdate = () =>
            {
                if (enemy != null)
                    enemy.OnServerUpdate();
            };

            // Remove this units update event on death
            enemy.healthManager.onDeathCallback += () =>
            {
                UpdateActions.Remove(serverUpdate);

                // Repspawn a new random enemy
                StartCoroutine(RespawnEnemy(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)].gameObject));
            };

            UpdateActions.Add(serverUpdate);
            enemies.Add(enemy);
        }

        private void UpdateServer()
        {
            if (UpdateActions != null)
            {
                for (int i = UpdateActions.Count - 1; i > 0; i--)
                {
                    UpdateActions[i].Invoke();
                }
            }
        }

        private IEnumerator RespawnEnemy(GameObject enemyToRespawn)
        {
            yield return new WaitForSecondsRealtime(3);
            SpawnEnemy(enemyToRespawn);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, spawnRadius);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, antiSpawnRadius);
        }
    }
}