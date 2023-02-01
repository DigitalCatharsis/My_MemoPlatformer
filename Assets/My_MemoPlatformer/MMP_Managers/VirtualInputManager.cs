using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace My_MemoPlatformer
{
    public class VirtualInputManager : Singleton<VirtualInputManager>
    {
        public bool moveRight;
        public bool moveLeft;
        public bool moveUp;
        public bool moveDown;
        public bool jump;
        public bool attack;
    }

}