using System;
using System.Collections.Generic;

namespace My_MemoPlatformer
{
    [Serializable]
    public class Serializable_SubList<T> 
    {
        public List<T> subList;
        public Serializable_SubList()
        {
            subList = new List<T>();
        }
    }

    [Serializable]
    public class Serializable_List<T>
    {
        public List<Serializable_SubList<T>> instance;
    }
}

