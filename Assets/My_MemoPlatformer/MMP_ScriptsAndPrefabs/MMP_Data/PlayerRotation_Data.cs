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

        public ReturnBool EarlyTurnIsLocked;
    }

}
