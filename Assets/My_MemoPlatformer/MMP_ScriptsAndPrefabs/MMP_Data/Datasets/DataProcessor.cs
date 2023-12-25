using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer.Datasets
{
    public class DataProcessor : MonoBehaviour
    {
        private Dictionary<System.Type, Dataset> _datasetsDictionary = new Dictionary<System.Type, Dataset>();

        public void InitializeSets(System.Type[] arr)
        {
            foreach (System.Type type in arr)
            {
                var newObj = new GameObject(); 
                newObj.transform.parent = this.transform;
                newObj.transform.localPosition = Vector3.zero;
                newObj.transform.localRotation = Quaternion.identity;
                newObj.name = type.Name;

                Dataset dataset = (Dataset)newObj.AddComponent(type);

                _datasetsDictionary.Add(type, dataset);
            }
        }

        public Dataset GetDataset(System.Type type)
        {
            return _datasetsDictionary[type];
        }
    }
}
