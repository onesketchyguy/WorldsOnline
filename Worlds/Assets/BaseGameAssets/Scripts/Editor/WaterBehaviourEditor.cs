using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Worlds.Effects
{
    [CustomEditor(typeof(WaterBehaviour))]
    public class WaterBehaviourEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var water = (WaterBehaviour)target;
            if (water.autoUpdate)
                water.CreateShape();
        }
    }
}