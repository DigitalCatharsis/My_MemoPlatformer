using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class Settings : MonoBehaviour
    {
        public FrameSettings frameSettings;
        [SerializeField] private bool debug;

        private void Awake()
        {
            if (debug)
            {
                Debug.Log("timeScale: " + frameSettings.TimeScale);
            }
            Time.timeScale = frameSettings.TimeScale;

            if (debug)
            {
                Debug.Log("TargetFrameRate" + frameSettings.TargetFPS);
            }
            Application.targetFrameRate = frameSettings.TargetFPS;
        }
    }
}