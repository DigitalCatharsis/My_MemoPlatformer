using System;
using UnityEngine;

namespace My_MemoPlatformer
{
    [Serializable]
    public class CharacterMovement_Data
    {
        [Header("Latest Scripts")]
        public MoveForward latestMoveForwardScript;  //latest moveForward script
        public MoveUp latestMoveUpScript;  //latest moveUp script

        [Header("GroundMovement")]
        public bool isIgnoreCharacterTime; //slide beyond character (start ignoring character collider)  

        public Action<float, float> MoveCharacterForward;
        public Action NullifyUpVelocity;
        public Func<bool> IsFacingAtacker;
        public Func<bool> IsForwardReversed;
    }
}