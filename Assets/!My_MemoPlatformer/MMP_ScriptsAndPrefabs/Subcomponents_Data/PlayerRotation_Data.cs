using System;
using UnityEngine;

namespace My_MemoPlatformer
{
    [Serializable]

    public class PlayerRotation_Data
    {
        public bool lockEarlyTurn;
        public bool lockDirectionNextState;

        public delegate bool ReturnBool();
        public delegate void DoSomething(bool faceforward);

        public ReturnBool EarlyTurnIsLocked;
        public ReturnBool IsFacingForward;
        public DoSomething FaceForward;
    }

}
