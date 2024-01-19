using System;
using System.Collections.Generic;

namespace My_MemoPlatformer
{
    [Serializable]
    public class Serializable_SubList<T> 
    {
        public List<T> subList = new();
    }

    [Serializable]
    public class Serializable_List<T>
    {
        public List<Serializable_SubList<T>> instance = new();
    }
}

