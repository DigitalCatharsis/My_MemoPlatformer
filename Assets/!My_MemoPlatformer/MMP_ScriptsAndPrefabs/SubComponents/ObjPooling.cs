using System.Collections.Generic;
using My_MemoPlatformer;
using UnityEngine;

namespace My_MemoPlatformer
{

    public class ObjPooling : SubComponent
    {
        public ObjPooling_Data objPoolingData;

        private void Start()
        {
            objPoolingData = new ObjPooling_Data
            {
                dataTypes = new List<DataType>(),
                Vfxs = new List<VFXType>(),
            };

            subComponentProcessor.objPoolingData = objPoolingData;
            subComponentProcessor.arrSubComponents[(int)SubComponentType.OBJ_POOLING_DATA] = this;
        }
        public override void OnFixedUpdate()
        {
            throw new System.NotImplementedException();
        }

        public override void OnUpdate()
        {
            throw new System.NotImplementedException();
        }
    }

}