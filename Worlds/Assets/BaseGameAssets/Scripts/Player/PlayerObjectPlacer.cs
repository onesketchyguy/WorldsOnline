using Mirror;
using UnityEngine;

namespace Player
{
    public class PlayerObjectPlacer : NetworkBehaviour
    {
        public PlayerController controller;
        public Transform objectPlacerGhost;

        public LayerMask excludeFromCheck;

        /// <summary>
        /// Will be replaced with a user fed in object at a later point.
        /// </summary>
        public GameObject prefabToSpawnOnClick;

        private void Start()
        {
            objectPlacerGhost.gameObject.SetActive(false);

            if (controller == null)
                controller = GetComponent<PlayerController>();
        }

        private void Update()
        {
            if (!isLocalPlayer) return;

            if (controller == null)
            {
                Debug.LogError($"No player controller assigned to: {gameObject.name}:{nameof(PlayerObjectPlacer)}");
                Destroy(this);
                return;
            }

            objectPlacerGhost.gameObject.SetActive(true);
            objectPlacerGhost.rotation = Quaternion.identity;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo)) // hit an object
            {
                var pos = new Vector3(Mathf.CeilToInt(hitInfo.point.x), Mathf.CeilToInt(hitInfo.point.y + 0.75f), Mathf.CeilToInt(hitInfo.point.z));

                objectPlacerGhost.position = pos;
            }

            if (controller.lastButton == LastButton.fire2) // Right click
            {
                // Debug, will move to having an object manager

                var col = Physics.OverlapBox(objectPlacerGhost.transform.position, Vector3.one * 0.25f);

                if (col == null || col.Length <= 0)
                    CmdPlaceObject(objectPlacerGhost.position);
            }
        }

        [Command]
        private void CmdPlaceObject(Vector3 pos)
        {
            ObjectPool.localInstance.GetObject(prefabToSpawnOnClick, pos);
        }
    }
}