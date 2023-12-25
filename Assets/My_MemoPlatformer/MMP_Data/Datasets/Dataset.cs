using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer.Datasets
{
    public class Dataset : MonoBehaviour
    {
        protected Dictionary<int, bool> boolDictionary = new Dictionary<int, bool>();
        protected Dictionary<int, float> floatDictionary = new Dictionary<int, float>();
        protected Dictionary<int, Vector3> vector3Dictionary = new Dictionary<int, Vector3>();

        public bool GetBool(int index)
        {
            if (boolDictionary.ContainsKey(index))
            {
                return boolDictionary[index];
            }
            else
            {
                return false;
            }
        }

        public void SetBool(int index, bool value)
        {
            boolDictionary[index] = value;
        }

        public void SetFloat(int index, float value)
        {
            floatDictionary[index] = value;
        }
        public float GetFloat(int index)
        {
            if (floatDictionary.ContainsKey(index))
            {
                return floatDictionary[index];
            }
            else
            {
                return 0f;
            }
        }
        public void SetVector3(int index, Vector3 value)
        {
            vector3Dictionary[index] = value;
        }
        public Vector3 GetVecto3(int index)
        {
            if (vector3Dictionary.ContainsKey(index))
            {
                return vector3Dictionary[index];
            }
            else
            {
                return Vector3.zero;
            }
        }
    }
}

