using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class PoolManager : Singleton<PoolManager>
    {
        public Dictionary<PoolObjectType, List<GameObject>> poolDictionary = new Dictionary<PoolObjectType, List<GameObject>>();

        public void SetUpDictionary()   
        {
            //get every enum in PoolObjedtType
            PoolObjectType[] arr = System.Enum.GetValues(typeof(PoolObjectType)) as PoolObjectType[];
            
            foreach (PoolObjectType p in arr)
            {
                if (!poolDictionary.ContainsKey(p))
                {
                    poolDictionary.Add(p, new List<GameObject>());
                }
            }


        }

        public GameObject GetObject (PoolObjectType objType) // Когда мы активируем объект, он выходит из списка. Когда деактивируем, добавляем в список.
        {
            if (poolDictionary.Count == 0)
            {
                SetUpDictionary();
            }

            List<GameObject> list = poolDictionary[objType];
            GameObject obj = null;

            if (list.Count > 0)    // Опять забыл, не забывай, что это не совсем сам объект, а pull object, шаблончик под объект. мы смотрим, что есть свободные шаблончики. Например пуль 30, а шаблончиков 29, то далее мы выделяем 30-ый шаблончек...вроде так. 
            {
                obj = list[0];
                list.RemoveAt(0);
            }

            else
            {
                obj = PoolObjectLoader.InstantiatePrefab(objType).gameObject;
            }

            return obj;
        }

        public void AddObject (PoolObject obj)  //Добавляем, когда poolObject выключаетя
        {
            List<GameObject> list = poolDictionary[obj.poolObjectType];
            list.Add(obj.gameObject);
            obj.gameObject.SetActive(false);
        }

    } 
}