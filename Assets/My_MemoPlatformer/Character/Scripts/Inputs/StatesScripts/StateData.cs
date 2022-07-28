using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{

    public abstract class StateData : ScriptableObject
    {
        public float Duration;

        public abstract void UpdateAbility(CharacterState characterStateBase, Animator animator);
    }
}