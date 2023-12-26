using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class AttackManager : Singleton<AttackManager>    //Whole list if current attacks
    {
        public List<AttackCondition> currentAttacks = new List<AttackCondition>();

        public void ForceDeregester(CharacterControl control)
        {
            foreach (var info in currentAttacks)
            {
                if (info.attacker == control)
                {
                    info.isFinished = true;
                    info.GetComponent<PoolObject>().TurnOff();
                }
            }
        }
    }
}