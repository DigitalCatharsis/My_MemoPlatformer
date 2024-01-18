using System;
using System.Collections.Generic;

namespace My_MemoPlatformer
{
    [Serializable]
    public class SerializableList<T> : List<T>
    {
        public List<SubList<T>> sampleList;
    }

    [Serializable]
    public class SubList<T> : List<T>
    {
        public List<T> sampleList;
    }
}

