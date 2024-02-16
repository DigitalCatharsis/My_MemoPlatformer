using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class AttackManager : Singleton<AttackManager>    
    {
        public GameObject activeAttacks;
        public List<AttackCondition> currentAttacks = new List<AttackCondition>(); //Whole list of current attacks

        public void ForceDeregister(CharacterControl control)
        {
            foreach (var info in currentAttacks)
            {
                if (info.attacker == control)
                {
                    info.isFinished = true;
                    info.GetComponent<DataPoolObject>().TurnOff();
                }
            }
        }
    }
}