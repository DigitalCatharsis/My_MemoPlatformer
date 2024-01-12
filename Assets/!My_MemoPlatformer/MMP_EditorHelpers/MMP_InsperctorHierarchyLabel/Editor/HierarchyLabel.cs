using UnityEngine;
using UnityEditor;

/// <summary>
/// Check out http://diegogiacomelli.com.br/unitytips-hierarchy-window-group-header/
/// </summary>
namespace My_MemoPlatformer
{
    [InitializeOnLoad]
    public class HierarchyLabel : MonoBehaviour
    {
        static HierarchyLabel()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
        }

        static void HierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
        {
            GameObject obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

            if (obj != null && obj.name.StartsWith("---", System.StringComparison.Ordinal))    //Every gameobject which starts with --- gonna be painted and have a shadow label
            {
                EditorGUI.DrawRect(selectionRect, Color.grey);
                EditorGUI.DropShadowLabel(selectionRect, obj.name.Replace("-", "").ToString());
            }

            HighLightObj(obj, selectionRect);
        }

        static void HighLightObj(GameObject obj, Rect selectionRect)
        {
            if (obj != null && obj.name.StartsWith("--R", System.StringComparison.Ordinal))    //Every gameobject which starts with --- gonna be painted and have a shadow label
            {
                EditorGUI.DrawRect(selectionRect, Color.red);
                EditorGUI.DropShadowLabel(selectionRect, obj.name.Replace("--R", "").ToString());
            }
            else if (obj != null && obj.name.StartsWith("--G", System.StringComparison.Ordinal))    //Every gameobject which starts with --- gonna be painted and have a shadow label
            {
                EditorGUI.DrawRect(selectionRect, Color.green);
                EditorGUI.DropShadowLabel(selectionRect, obj.name.Replace("--G", "").ToString());
            }
            else if (obj != null && obj.name.StartsWith("--B", System.StringComparison.Ordinal))    //Every gameobject which starts with --- gonna be painted and have a shadow label
            {
                EditorGUI.DrawRect(selectionRect, Color.blue);
                EditorGUI.DropShadowLabel(selectionRect, obj.name.Replace("--B", "").ToString());
            }
            else if (obj != null && obj.name.StartsWith("--Y", System.StringComparison.Ordinal))    //Every gameobject which starts with --- gonna be painted and have a shadow label
            {
                EditorGUI.DrawRect(selectionRect, Color.yellow);
                EditorGUI.DropShadowLabel(selectionRect, obj.name.Replace("--Y", "").ToString());
            }
            else if (obj != null && obj.name.StartsWith("--M", System.StringComparison.Ordinal))    //Every gameobject which starts with --- gonna be painted and have a shadow label
            {
                EditorGUI.DrawRect(selectionRect, Color.magenta);
                EditorGUI.DropShadowLabel(selectionRect, obj.name.Replace("--M", "").ToString());
            }
            else if (obj != null && obj.name.StartsWith("--C", System.StringComparison.Ordinal))    //Every gameobject which starts with --- gonna be painted and have a shadow label
            {
                EditorGUI.DrawRect(selectionRect, Color.cyan);
                EditorGUI.DropShadowLabel(selectionRect, obj.name.Replace("--Y", "").ToString());
            }
            else if (obj != null && obj.name.StartsWith($"{typeof(HierarchyLabel).Namespace}.", System.StringComparison.Ordinal))    //Every gameobject which starts with --- gonna be painted and have a shadow label
            {
                EditorGUI.DrawRect(selectionRect, Color.black);
                EditorGUI.DropShadowLabel(selectionRect, obj.name.ToString());
            }          
        }
    }
}