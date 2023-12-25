using UnityEngine;

namespace My_MemoPlatformer.Datasets
{
    public enum AirControlBool
    {
        NONE,
        CAN_WALL_JUMP,
        CHECK_WALL_BLOCK,
    }

    public class AirControl_Dataset : Dataset
    {
        private void Awake()
        {
            boolDictionary.Add((int)AirControlBool.CAN_WALL_JUMP, false);
            boolDictionary.Add((int)AirControlBool.CHECK_WALL_BLOCK, false);
        }
    }
}

