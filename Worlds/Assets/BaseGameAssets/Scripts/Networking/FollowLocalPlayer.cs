using Mirror;
using UnityEngine;

namespace Worlds.Networking
{
    public class FollowLocalPlayer : NetworkBehaviour
    {
        private Transform following;

        public float followSpeed = 10;

        private void Start()
        {
            foreach (var item in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (item.GetComponent<NetworkBehaviour>().isLocalPlayer)
                {
                    following = item.transform;
                    break;
                }
            }
        }

        private void Update()
        {
            if (following == null)
            {
                Start();
                return;
            }
            transform.position = Vector3.Slerp(transform.position, following.position, (followSpeed + Vector3.Distance(transform.position, following.position)) * Time.deltaTime);
        }
    }
}