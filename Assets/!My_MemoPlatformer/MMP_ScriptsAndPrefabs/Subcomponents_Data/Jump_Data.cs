using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    [System.Serializable]
    public class Jump_Data
    {
        public Dictionary<int, bool> dicJumped;
        public bool canWallJump;
        public bool checkWallBlock;
    }
}
