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
            
            foreach (var p in arr)
            {
                if (!poolDictionary.ContainsKey(p))
                {
                    poolDictionary.Add(p, new List<GameObject>());
                }
            }


        }

        public GameObject GetObject (PoolObjectType objType) // ����� �� ���������� ������, �� ������� �� ������. ����� ������������, ��������� � ������.
        {
            if (poolDictionary.Count == 0)
            {
                SetUpDictionary();
            }

            List<GameObject> list = poolDictionary[objType];
            GameObject obj = null;

            if (list.Count > 0)    // ����� �����, �� �������, ��� ��� �� ������ ��� ������, � pull object, ��������� ��� ������. �� �������, ��� ���� ��������� ����������. �������� ���� 30, � ����������� 29, �� ����� �� �������� 30-�� ���������...����� ���. 
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

        public void AddObject (PoolObject obj)  //���������, ����� poolObject ����������
        {
            List<GameObject> list = poolDictionary[obj.poolObjectType];
            list.Add(obj.gameObject);
            obj.gameObject.SetActive(false);
        }

    } 
}