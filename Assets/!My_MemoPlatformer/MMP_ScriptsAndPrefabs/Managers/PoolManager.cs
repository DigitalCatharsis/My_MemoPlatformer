using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class PoolManager : Singleton<PoolManager>
    {
        [SerializedDictionary("CharacterType", "Values")]
        public SerializedDictionary<CharacterType, List<GameObject>> characterPoolDictionary = new SerializedDictionary<CharacterType, List<GameObject>>();

        [SerializedDictionary("VFXType", "Values")]
        public SerializedDictionary<VFXType, List<GameObject>> vfxPoolDictionary = new SerializedDictionary<VFXType, List<GameObject>>();

        [SerializedDictionary("DataType", "Values")]
        public SerializedDictionary<DataType, List<GameObject>> dataPoolDictionary = new SerializedDictionary<DataType, List<GameObject>>();

        [SerializedDictionary("DataType", "Values")]
        public SerializedDictionary<DataType, SerializedDictionary<DataType, GameObject>> testList = new SerializedDictionary<DataType, SerializedDictionary<DataType, GameObject>>();

        #region SetupDictionary
        private void SetUpDictionary<T>(Dictionary<T,List<GameObject>> poolDictionary)
        {
            T[] arr = Enum.GetValues(typeof(T)) as T[];

            foreach (T p in arr)
            {
                if (!poolDictionary.ContainsKey(p))
                {
                    poolDictionary.Add(p, new List<GameObject>());
                }
            }
        }
        #endregion

        #region GetObjectFromPool
        public GameObject GetObject<T>(T objType, Dictionary<T, List<GameObject>> poolDictionary,  Vector3 position, Quaternion rotation)
        {
                return ObjectGetter(poolDictionary, objType, position, rotation);
        }

        private GameObject ObjectGetter<T>(Dictionary<T, List<GameObject>> poolDictionary, T objType, Vector3 position, Quaternion rotation)
        {
            if (poolDictionary.Count == 0)
            {
                SetUpDictionary(poolDictionary);
            }

             List<GameObject> list = poolDictionary[objType];

            if (list.Count > 0)
            {
                var obj = list[0];
                list.RemoveAt(0);
                return obj;
            }
            else
            {
                return PoolObjectLoader.Instance.InstantiatePrefab(objType, position, rotation);
            }
        }
        #endregion

        public void AddObject<T>(T objType, Dictionary<T, List<GameObject>> poolDictionary, GameObject poolGameObject)
        {
            var listOfGameObjects = poolDictionary[(T)objType];
            listOfGameObjects.Add(poolGameObject);
            poolGameObject.SetActive(false);
        }
    }
}
