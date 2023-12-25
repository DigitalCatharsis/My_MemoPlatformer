using UnityEngine;

namespace My_MemoPlatformer.Datasets
{
    public enum AirControlBool
    {
        NONE,
        JUMPED,
        CAN_WALL_JUMP,
        CHECK_WALL_BLOCK,
    }
    public enum AirControlFloat
    {
        NONE,
        AIR_MOMENTUM,
    }

    public class AirControl_Dataset : Dataset
    {
        private void Awake()
        {
            boolDictionary.Add((int)AirControlBool.JUMPED, false);
            boolDictionary.Add((int)AirControlBool.CAN_WALL_JUMP, false);
            boolDictionary.Add((int)AirControlBool.CHECK_WALL_BLOCK, false);
            floatDictionary.Add((int)AirControlFloat.AIR_MOMENTUM, 0f);
        }
    }
}

