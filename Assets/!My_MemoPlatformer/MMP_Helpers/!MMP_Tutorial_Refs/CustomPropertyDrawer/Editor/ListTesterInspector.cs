using UnityEditor;
using UnityEngine;

namespace My_MemoPlatformer
{
    [CustomEditor(typeof(ListTester))]
    public class ListTesterInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorList.Show(serializedObject.FindProperty("integers"));
            EditorList.Show(serializedObject.FindProperty("vectors"));
            EditorList.Show(serializedObject.FindProperty("colorPoints"));
            EditorList.Show(serializedObject.FindProperty("objects"));
            serializedObject.ApplyModifiedProperties();
        }
    }
}