using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;  //��� ����� ����

namespace My_MemoPlatformer
{
    [CustomEditor(typeof(LookAtPoint))] //The CustomEditor attribute informs Unity which component it should act as an editor for.
    [CanEditMultipleObjects]  //The CanEditMultipleObjects attribute tells Unity that you can select multiple objects with this editor and change them all at the same time.

    public class LookAtPointEditor : Editor  //����������� �� Editor
    {
        SerializedProperty lookAtPoint; //�������� �������� ��� ����� �����
        SerializedProperty objectToLook; //�������� �������� ��� ����� �����
        SerializedProperty rotateToPoint; //�������� �������� ��� ����� �����
        SerializedProperty traceColor; //�������� �������� ��� ����� �����
        SerializedProperty traceExistDuration; //�������� �������� ��� ����� �����

        void OnEnable()
        {
            
            lookAtPoint = serializedObject.FindProperty("lookAtPoint"); //����� ���� �������
            objectToLook = serializedObject.FindProperty("objectToLook"); //����� ���� �������
            rotateToPoint = serializedObject.FindProperty("rotateToPoint"); //����� ���� �������
            traceColor = serializedObject.FindProperty("traceColor"); //����� ���� �������
            traceExistDuration = serializedObject.FindProperty("traceExistDuration"); //����� ���� �������
        }

        public override void OnInspectorGUI()  //GUI ����������
        {
            var tar = (target as LookAtPoint);
            tar.Update(); //���������� Update, ��������� � ������ tar

            serializedObject.Update(); //������� ����, ����� ��� �������
            EditorGUILayout.PropertyField(lookAtPoint); //���������� �������������� ����
            EditorGUILayout.PropertyField(objectToLook); //���������� �������������� ����
            EditorGUILayout.PropertyField(rotateToPoint); //���������� �������������� ����
            EditorGUILayout.PropertyField(traceColor); //���������� �������������� ����
            EditorGUILayout.PropertyField(traceExistDuration); //���������� �������������� ����

            if (lookAtPoint.vector3Value.y > (target as LookAtPoint).transform.position.y)
            {
                EditorGUILayout.LabelField("(Selected object is below lookAtPoint)"); //������ TIP
            }

            if (lookAtPoint.vector3Value.y < (target as LookAtPoint).transform.position.y)
            {
                EditorGUILayout.LabelField("(Selected object is above lookAtPoint)");
            }

            if (lookAtPoint.vector3Value.y == (target as LookAtPoint).transform.position.y)
            {
                EditorGUILayout.LabelField("Selected object at the same \"y\" level as lookAtPoint");
            }

            EditorGUILayout.LabelField($"Vector range to point: {(lookAtPoint.vector3Value - (target as LookAtPoint).transform.position)}");
            EditorGUILayout.LabelField("This is working even if script is disabled!"); //������ TIP
            serializedObject.ApplyModifiedProperties(); //��� ���� ������ �� ������ �������� ��������
        }

        //public void OnSceneGUI()
        //{
        //    var tar = (target as LookAtPoint);

        //    EditorGUI.BeginChangeCheck();
        //    Vector3 pos = Handles.PositionHandle(tar.lookAtPoint, Quaternion.identity);
        //    if (EditorGUI.EndChangeCheck())
        //    {
        //        Undo.RecordObject(target, "Move point");
        //        tar.lookAtPoint = pos;
        //        tar.Update(); //���������� Update, ��������� � ������ tar
        //    }
        //}
    }
}
