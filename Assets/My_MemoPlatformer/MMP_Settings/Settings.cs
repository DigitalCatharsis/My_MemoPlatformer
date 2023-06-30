using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class Settings : MonoBehaviour
    {
        public FrameSettings frameSettings;
        public PhysicsSettings physicsSettings;
        [SerializeField] private bool debug;

        private void Awake()
        {
            //Frames
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

            //Physics
            if (debug)
            {
                Debug.Log("Default Solver Velocity Iterations" + physicsSettings.DefaultSolverVelocityIterations);
            }
            Physics.defaultSolverVelocityIterations = physicsSettings.DefaultSolverVelocityIterations;

            //Load Keys
            if (debug)
            {
                Debug.Log("Loading keys binding");
            }

            VirtualInputManager.Instance.LoadKeys();
        }
    }
}