﻿using Mirror;
using UnityEngine;
using UnityEngine.AI;

namespace Worlds.Enemies
{
    [RequireComponent(typeof(NavMeshAgent))]
    /// <summary>
    /// This is an extremely basic character controller.
    /// It will simply move an enemy towards a player instance, then attack when within range.
    /// </summary>
    public class BasicEnemyBehaviour : NetworkBehaviour
    {
        public HealthManager healthManager;

        public Vector3 firePoint;

        public int damage = 10;

        public LayerMask attackableLayers;

        private Vector3 GetFirePoint()
        {
            return transform.position + transform.TransformVector(firePoint);
        }

        [Header("Animation")]
        public Animator anim;

        public string moveSpeed = "speed";
        public string attack = "attack1";
        //public float timeBeforeAttack = 0.5f;

        public string death = "die";
        public string hurt = "hit";
        public float timeBeforeCleanup = 1;

        [SyncVar]
        private bool isDead;

        [Header("Navigation")]
        public NavMeshAgent navAgent;

        public float attackRange = 1.5f;
        public float searchRange = 100;
        private bool withinAttackRange = false;

        public float movementSpeed = 3f;

        private float startSpeed;

        private Transform target;

        private float nextWander;

        private void OnValidate()
        {
            if (navAgent == null)
                navAgent = GetComponent<NavMeshAgent>();

            if (anim == null)
                anim = GetComponent<Animator>();
        }

        [ServerCallback]
        internal virtual void Initialize()
        {
            RpcSetSpeed(movementSpeed);

            healthManager.onDeathCallback += () =>
            {
                isDead = true;
                if (anim != null) RpcSetAnimBool(death, isDead);
                Invoke(nameof(RpcReturnObject), timeBeforeCleanup);
            };

            healthManager.OnHealthModifiedCallback += (float modAmount) =>
            {
                if (modAmount < 0) // hurt
                {
                    RpcSetAnimTrigger(hurt);
                }
            };

            healthManager.RpcReset();
        }

        [ServerCallback]
        internal virtual void OnServerUpdate()
        {
            // We should replace this with moving onto a nav mesh, or reseting our position.
            if (navAgent.isOnNavMesh == false) healthManager.RpcModifyHealth(-healthManager.GetCurrentHealth());

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
        private void RpcReturnObject() => ObjectManager.ReturnObject(gameObject);

        [ClientRpc]
        private void RpcSetAnimFloat(string var, float value) => anim.SetFloat(var, value);

        [ClientRpc]
        private void RpcSetAnimTrigger(string var) => anim.SetTrigger(var);

        [ClientRpc]
        private void RpcSetAnimBool(string var, bool value) => anim.SetBool(var, value);

        [ClientRpc]
        private void RpcSetDestination(Vector3 destination) => navAgent.SetDestination(destination);

        internal virtual void SearchForTargets()
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

        [ClientRpc]
        public void RpcSetSpeed(float speed)
        {
            if (speed == -1) speed = movementSpeed;

            navAgent.speed = speed;
            navAgent.angularSpeed = speed * 90;
        }

        //private bool attacking = false;

        internal virtual void TryAttackTarget()
        {
            if (Vector3.Distance(transform.position, target.position) <= attackRange)
            {
                // Attack
                withinAttackRange = true; // Let the animator know we are attacking

                // Moved to animator
                // if (!attacking)
                // {
                //     attacking = true;
                //     //Invoke(nameof(RpcFireRay), timeBeforeAttack);
                // }
            }
            else
            {
                if (Vector3.Distance(transform.position, navAgent.destination) <= attackRange * 3)
                {
                    if (target.position != navAgent.destination)
                    {
                        // Set the new position
                        RpcSetDestination(Clamp(transform.position, target.position, Mathf.Clamp(attackRange - 0.5f, 0, attackRange)));
                    }
                }
            }
        }

        [ClientRpc]
        public void RpcFireRay()
        {
            var capsuleCast = Physics.SphereCastAll(GetFirePoint(), 0.35f, transform.forward, attackRange, attackableLayers, QueryTriggerInteraction.UseGlobal);

            if (capsuleCast != null)
            {
                foreach (var hitInfo in capsuleCast)
                {
                    if (hitInfo.transform != null)
                    {
                        var collision = hitInfo.transform.gameObject;

                        // Check to see if we hit something damagable
                        var other = collision.GetComponent<HealthManager>();

                        if (other != null) other.RpcModifyHealth(-damage);

                        Debug.DrawRay(GetFirePoint(), transform.forward * hitInfo.distance, Color.red, 1);
                    }
                    else Debug.DrawRay(GetFirePoint(), transform.forward * attackRange, Color.black, 1);
                }
            }

            // attacking = false;
        }

        private Vector3 Clamp(Vector3 clamping, Vector3 clampingTo, float clampRange = 1)
        {
            var value = clamping;
            value.x = Mathf.Clamp(value.x, clampingTo.x - clampRange, clampingTo.x + clampRange);
            value.y = Mathf.Clamp(value.y, clampingTo.y - clampRange, clampingTo.y + clampRange);
            value.z = Mathf.Clamp(value.z, clampingTo.z - clampRange, clampingTo.z + clampRange);

            return value;
        }

        internal virtual void Wander()
        {
            if (Time.time < nextWander) return;// Dont wander if we don't need to yet

            // Create a target position near me to move towards
            var pos = Random.insideUnitSphere * searchRange;
            pos.y = transform.position.y; // Keep the y level with our character

            RpcSetDestination(pos);

            // Create a timestamp so that when we wander again, it's after we have already reached out destination.
            nextWander = Time.time + (Random.Range(1f, 10F)) + (Vector3.Distance(transform.position, pos));
        }

        // Draw a debug through gizmos
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(GetFirePoint(), .25f);
            Gizmos.DrawRay(GetFirePoint(), transform.forward);

            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(transform.position, searchRange);

            if (target == null) return;
            Gizmos.DrawSphere(target.position, .5f);
            Gizmos.DrawLine(transform.position, target.position);
        }
    }
}