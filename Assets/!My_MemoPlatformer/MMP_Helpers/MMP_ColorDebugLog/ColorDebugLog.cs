using System.Drawing;
using UnityEngine;

namespace My_MemoPlatformer
{
    public static class ColorDebugLog
    {
        public static void Log(string content, KnownColor color)
        {
            Debug.Log($"<color={color}>{content}</color>");
        }
    }
}