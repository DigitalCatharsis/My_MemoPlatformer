using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{

    public class ObjPooling : SubComponent
    {
        public ObjPooling_Data objPooling_Data;

        private void Start()
        {
            objPooling_Data = new ObjPooling_Data
            {
                dataTypes = new List<GameObject>(),
                Vfxs = new List<GameObject>(),

                AddToList = AddToList,
                RemoveFromList = RemoveFromList,
            };

            subComponentProcessor.objPoolingData = objPooling_Data;
            subComponentProcessor.arrSubComponents[(int)SubComponentType.OBJ_POOLING_DATA] = this;
        }
        public override void OnFixedUpdate()
        {
        }

        public override void OnUpdate()
        {
        }

        private void AddToList<T>(GameObject spawnedObject, T poolObjectType)
        {
            switch (poolObjectType)
            {
                case
                    DataType:
                    if (!objPooling_Data.dataTypes.Contains(spawnedObject))
                    {
                        objPooling_Data.dataTypes.Add(spawnedObject);
                    }
                    break;
                case
                    VFXType:
                    if (!objPooling_Data.Vfxs.Contains(spawnedObject))
                    {
                        objPooling_Data.Vfxs.Add(spawnedObject);
                    }
                    break;
            }
        }

        private void RemoveFromList<T>(GameObject spawnedObject, T poolObjectType)
        {
            switch (poolObjectType)
            {
                case
                    DataType:
                    if (objPooling_Data.dataTypes.Contains(spawnedObject))
                    {
                        objPooling_Data.dataTypes.Remove(spawnedObject);
                    }
                    break;
                case
                    VFXType:
                    if (objPooling_Data.Vfxs.Contains(spawnedObject))
                    {
                        objPooling_Data.Vfxs.Remove(spawnedObject);
                    }
                    break;
            }
        }
    }
}