using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace WorldsUI
{
    public class ModifyWindowManager : MonoBehaviour
    {
        // GOAL: Create a manager that can resize windows at the users discression
        // HOW: Ray cast the UI to try to find the mouse position, then try to get the closest corner to the mouse of the object the mouse is over
        // Then proceed to hold onto that corner, and rescale the canvas element accordingly

        private GraphicRaycaster m_Raycaster;
        private PointerEventData m_PointerEventData;
        private EventSystem m_EventSystem;

        private GameObject holding;

        public string editableTag = "Editable";
        public float clickThreshold = 0.025f;

        private void Start()
        {
            //Fetch the Raycaster from the GameObject (the Canvas)
            m_Raycaster = GetComponent<GraphicRaycaster>();
            //Fetch the Event System from the Scene
            m_EventSystem = GetComponent<EventSystem>();
        }

        private GameObject ProcessClickEvents()
        {
            //Set up the new Pointer Event
            m_PointerEventData = new PointerEventData(m_EventSystem);
            //Set the Pointer Event Position to that of the mouse position
            m_PointerEventData.position = Input.mousePosition;

            //Create a list of Raycast Results
            var results = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and mouse click position
            m_Raycaster.Raycast(m_PointerEventData, results);

            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in results)
            {
                Debug.Log("Hit " + result.gameObject.name);

                var go = result.gameObject;

                while (go.transform.parent != null)
                {
                    go = go.transform.parent.gameObject;
                    if (go.gameObject.CompareTag(editableTag))
                        break;
                }

                return go;
            }

            return null;
        }

        private void Update()
        {
            //Check if the left Mouse button is clicked
            if (Input.GetButtonDown("Fire1"))
            {
                holding = ProcessClickEvents();
            }
            else
            if (Input.GetButtonUp("Fire1"))
            {
                holding = null;
            }

            if (holding != null)
            {
                var rectObject = holding.GetComponent<RectTransform>();
                var pos = Camera.main.ScreenToViewportPoint(Input.mousePosition);

                // Get the closest corner to us
                bool movingMin = true;
                bool usingX = true;
                bool usingY = true;

                if (pos.x > rectObject.anchorMin.x + clickThreshold)
                {
                    // Use max
                    movingMin = false;

                    if (pos.x < rectObject.anchorMax.x - clickThreshold)
                    {
                        // Use nothing in this case we are in the center of the object
                        usingX = false;
                    }
                }

                if (pos.y > rectObject.anchorMin.y + clickThreshold)
                {
                    // Use max
                    movingMin = false;

                    if (pos.y < rectObject.anchorMax.y - clickThreshold)
                    {
                        // Use nothing in this case we are in the center of the object
                        usingY = false;
                    }
                }

                pos.x = usingX ? pos.x : (movingMin ? rectObject.anchorMin.x : rectObject.anchorMax.x);
                pos.y = usingY ? pos.y : (movingMin ? rectObject.anchorMin.y : rectObject.anchorMax.y);

                // Rescale the object
                if (movingMin) rectObject.anchorMin = pos;
                else rectObject.anchorMax = pos;
            }
        }
    }
}