using Mirror;
using UnityEngine;
using UnityEngine.AI;

namespace World.Enemies
{
    [RequireComponent(typeof(NavMeshAgent))]
    /// <summary>
    /// This is an extremely basic character controller.
    /// It will simply move an enemy towards a player instance, then attack when within range.
    /// </summary>
    public class Zombie : NetworkBehaviour
    {
        public HealthManager healthManager;

        [Header("Animation")]
        public Animator anim;

        public string moveSpeed = "speed";
        public string attack = "attack1";
        public string death = "die";
        public float timeBeforeCleanup = 1;

        [SyncVar]
        private bool isDead;

        [Header("Navigation")]
        public NavMeshAgent navAgent;

        public float attackRange = 1.5f;
        public float searchRange = 100;
        private bool withinAttackRange = false;

        private Transform target;

        private float nextWander;

        private void OnValidate()
        {
            if (navAgent == null)
                navAgent = GetComponent<NavMeshAgent>();

            if (anim == null)
                anim = GetComponent<Animator>();
        }

        private void Start()
        {
            healthManager.onDeathCallback += () =>
            {
                isDead = true;
                if (anim != null) RpcSetAnimBool(death, isDead);
                Invoke(nameof(RpcReturnObject), timeBeforeCleanup);
            };
        }

        private void Update()
        {
            if (isDead) return;

            withinAttackRange = false;
            SearchForTargets();

            if (target == null)
            {
                // Search for targets/wander
                Wander();
            }
            else
            {
                // Move towards the target, then attack when in range
                TryAttackTarget();
            }

            if (anim != null)
            {
                RpcSetAnimFloat(moveSpeed, navAgent.velocity.magnitude);
                RpcSetAnimBool(attack, withinAttackRange);
            }
        }

        [ClientRpc]
        private void RpcReturnObject() => ObjectPool.localInstance.ReturnObject(gameObject);

        [ClientRpc]
        private void RpcSetAnimFloat(string var, float value) => anim.SetFloat(var, value);

        [ClientRpc]
        private void RpcSetAnimBool(string var, bool value) => anim.SetBool(var, value);

        private void SearchForTargets()
        {
            if (target != null && Vector3.Distance(transform.position, target.position) >= searchRange)
            {
                target = null;
                nextWander = Time.time + searchRange;
            }
            else
            {
                // Search for targets
                var targets = EnemyBlackBoard.GetPlayers();

                if (targets != null)
                {
                    var closest = searchRange;

                    foreach (var player in targets)
                    {
                        var dist = Vector3.Distance(transform.position, player.position);

                        if (dist < closest)
                        {
                            target = player;
                            closest = dist;
                        }
                    }
                }
            }
        }

        private void TryAttackTarget()
        {
            if (Vector3.Distance(transform.position, target.position) <= attackRange)
            {
                // Attack
                withinAttackRange = true;
            }
            else
            {
                if (Vector3.Distance(transform.position, navAgent.destination) <= attackRange * 3)
                {
                    if (target.position != navAgent.destination)
                    {
                        // Set the new position
                        navAgent.SetDestination(target.position);
                    }
                }
            }
        }

        private void Wander()
        {
            if (Time.time < nextWander) return;// Dont wander if we don't need to yet

            // Create a target position near me to move towards
            var pos = Random.insideUnitSphere * searchRange;
            pos.y = transform.position.y; // Keep the y level with our character

            navAgent.SetDestination(pos);

            // Create a timestamp so that when we wander again, it's after we have already reached out destination.
            nextWander = Time.time + (Random.Range(1f, 10F)) + (Vector3.Distance(transform.position, pos));
        }

        // Draw a debug through gizmos
        private void OnDrawGizmos()
        {
            // pink
            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(transform.position, searchRange);

            if (target == null) return;
            Gizmos.DrawSphere(target.position, .5f);
            Gizmos.DrawLine(transform.position, target.position);
        }
    }
}