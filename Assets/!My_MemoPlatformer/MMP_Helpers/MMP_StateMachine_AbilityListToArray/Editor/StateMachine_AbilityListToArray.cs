using UnityEngine;
using UnityEditor;

namespace My_MemoPlatformer
{
    [CustomEditor(typeof(AbilityListToArray))]
    public class AbilityListToArrayEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            AbilityListToArray converter = (AbilityListToArray)target;

            if (GUILayout.Button("Put Lists in Array"))
            {
                converter.Convert();
            }

            if (GUILayout.Button("Clear List"))
            {
                converter.ClearLists();
            }
        }
    }
}
