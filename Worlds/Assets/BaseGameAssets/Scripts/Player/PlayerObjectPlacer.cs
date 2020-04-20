using Mirror;
using UnityEngine;

namespace Worlds.Player
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

        private Button buttonDown;

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
            /*
                        if (buttonDown != controller.latestButtons && buttonDown == Button.fire2) // Right click
                        {
                            // Debug, will move to having an object manager

                            var col = Physics.OverlapBox(objectPlacerGhost.transform.position, Vector3.one * 0.25f);

                            if (col == null || col.Length <= 0)
                                CmdPlaceObject(objectPlacerGhost.position);
                        }

                        if (buttonDown != controller.latestButtons && buttonDown == Button.fire1) // left click
                        {
                            // Debug, will move to having an object manager

                            var col = Physics.OverlapBox(objectPlacerGhost.transform.position, Vector3.one * 0.25f);

                            if (col != null && col.Length > 0 && col[0].tag == "Editable")
                            {
                                CmdRemoveObject(col[0].gameObject);
                            }
                        }

                        buttonDown = controller.latestButtons;*/
        }

        [Command]
        private void CmdPlaceObject(Vector3 pos)
        {
            CmdSnapToGrid(ObjectManager.localInstance.GetObject(prefabToSpawnOnClick, pos));
        }

        [Command]
        private void CmdRemoveObject(GameObject objectToReturn)
        {
            objectToReturn.SetActive(false);
            objectToReturn.SetActive(false);
            ObjectManager.localInstance.ReturnObject(objectToReturn);
        }

        [Command]
        private void CmdSnapToGrid(GameObject objectToSnap)
        {
            var x = (int)objectToSnap.transform.position.x;
            var y = (int)objectToSnap.transform.position.y;
            var z = (int)objectToSnap.transform.position.z;

            objectToSnap.transform.position = new Vector3(x, y, z);
        }
    }
}