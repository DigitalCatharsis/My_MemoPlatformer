using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using My_MemoPlatformer;

namespace My_MemoPlatformer
{
    [CustomEditor(typeof(PathFindingAgent))]
    public class PathFindingAgentEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            PathFindingAgent pathFindingAgent= (PathFindingAgent)target;

            if (GUILayout.Button("Go to target"))
            {
                pathFindingAgent.GoToTarget();
            }
        }

    }
}