using UnityEditor;
using UnityEngine;

namespace My_MemoPlatformer
{
    [CustomPropertyDrawer(typeof(ColorPoint_Data))]
    public class ColorPointDrawer : PropertyDrawer
    {
        //https://catlikecoding.com/unity/tutorials/editor/custom-data/#a-typeof
        public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
        {
            /*While our prefix label turns bold to signal that it is a modified prefab value, 
             * it doesn't allow any actions. So we cannot revert our entire color point 
             * at once and neither can we easily delete or duplicate array elements of it.
             *We need to tell the editor where our property starts and where it ends, 
             *because right now we are only showing part of its contents. 
             *We can use the EditorGUI.BeginProperty method to construct a new label and signal the start of a property, 
             *and use the EditorGUI.EndProperty method to signal when we are done. 
             */
            label = EditorGUI.BeginProperty(position, label, property);

            Rect contentPosition = EditorGUI.PrefixLabel(position, label);

            if (position.height > 16f)
            {
                position.height = 16f;
                EditorGUI.indentLevel += 1;
                contentPosition = EditorGUI.IndentedRect(position);
                contentPosition.y += 18f;
            }

            contentPosition.width *= 0.75f;

            //prevent elements to be placed too far to the right.
            EditorGUI.indentLevel = 0;
            EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("position"), GUIContent.none);
            contentPosition.x += contentPosition.width;
            contentPosition.width = contentPosition.width / 3f;
            EditorGUIUtility.labelWidth = 14f;
            EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("color"), new GUIContent("Color"));


            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return Screen.width < 333 ? (16f + 18f) : 16f;
        }
    }
}