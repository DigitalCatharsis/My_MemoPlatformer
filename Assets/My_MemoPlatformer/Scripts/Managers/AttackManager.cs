using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{

    public class AttackManager : Singleton<AttackManager>    //Whole list if current attacks
    {
        public List<AttackInfo> currentAttacks = new List<AttackInfo>();
    }
}