using UnityEngine;
using UnityEditor;

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
                pathFindingAgent.ReinitAgent_And_CheckDestination();
            }
        }

    }
}