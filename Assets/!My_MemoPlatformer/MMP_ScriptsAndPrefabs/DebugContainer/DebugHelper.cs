using My_MemoPlatformer;
using System.Collections.Generic;
using UnityEngine;

public class DebugHelper : MonoBehaviour
{
    [Header("SO_Refs")]
    [SerializeField] private ScriptableObject _abilitySearcher;

    [Header("Debug Bools")]
    [SerializeField] private bool _debug_CameraState;
    [SerializeField] private bool _debug_Attack;
    [SerializeField] private bool _debug_Instakill;
    [SerializeField] private bool _debug_Ragdoll;
    [SerializeField] private bool _debug_MoveForward;
    [SerializeField] private bool _debug_Jump;
    [SerializeField] private bool _debug_SpawnObjects;
    [SerializeField] private bool _debug_InputManager;
    [SerializeField] private bool _debug_TransitionTiming;
    [SerializeField] private bool _debug_TriggerDetector;

    [Space(10)]
    [SerializeField] private bool _displaySpheresAndColliders;
    [SerializeField] private bool _displayLedgeCheckers;
    private void Update()
    {
        My_MemoPlatformer.DebugContainer_Data.Instance.debug_CameraState = _debug_CameraState;
        My_MemoPlatformer.DebugContainer_Data.Instance.debug_Attack = _debug_Attack;
        My_MemoPlatformer.DebugContainer_Data.Instance.debug_Instakill = _debug_Instakill;
        My_MemoPlatformer.DebugContainer_Data.Instance.debug_Ragdoll = _debug_Ragdoll;
        My_MemoPlatformer.DebugContainer_Data.Instance.debug_MoveForward = _debug_MoveForward;
        My_MemoPlatformer.DebugContainer_Data.Instance.debug_Jump = _debug_Jump;
        My_MemoPlatformer.DebugContainer_Data.Instance.debug_SpawnObjects = _debug_SpawnObjects;
        My_MemoPlatformer.DebugContainer_Data.Instance.debug_InputManager = _debug_InputManager;
        My_MemoPlatformer.DebugContainer_Data.Instance.debug_TransitionTiming = _debug_TransitionTiming;
        My_MemoPlatformer.DebugContainer_Data.Instance.debug_TriggerDetector = _debug_TriggerDetector;

        ChangeSpheresRendererStatus();
        ChangeLedgeCheckerStatus();
    }

    private void ChangeSpheresRendererStatus()
    {
        //Render all Spheres
        if (_displaySpheresAndColliders)
        {
            foreach (var control in CharacterManager.Instance.characters)
            {
                var listOfSpheres = new List<GameObject>();
                listOfSpheres.AddRange(control.COLLISION_SPHERE_DATA.frontSpheres);
                listOfSpheres.AddRange(control.COLLISION_SPHERE_DATA.backSpheres);
                listOfSpheres.AddRange(control.COLLISION_SPHERE_DATA.upSpheres);
                listOfSpheres.AddRange(control.COLLISION_SPHERE_DATA.bottomSpheres);


                foreach (var sphere in listOfSpheres)
                {
                    sphere.GetComponent<MeshRenderer>().enabled = true;
                }
            }
        }
        else
        {
            foreach (var control in CharacterManager.Instance.characters)
            {
                var listOfSpheres = new List<GameObject>();
                listOfSpheres.AddRange(control.COLLISION_SPHERE_DATA.frontSpheres);
                listOfSpheres.AddRange(control.COLLISION_SPHERE_DATA.backSpheres);
                listOfSpheres.AddRange(control.COLLISION_SPHERE_DATA.upSpheres);
                listOfSpheres.AddRange(control.COLLISION_SPHERE_DATA.bottomSpheres);

                //Render all Spheres
                foreach (var sphere in listOfSpheres)
                {
                    sphere.GetComponent<MeshRenderer>().enabled = false;
                }
            }
        }
    }

    private void ChangeLedgeCheckerStatus()
    {
        //Render all Spheres
        if (_displaySpheresAndColliders)
        {
            foreach (var control in CharacterManager.Instance.characters)
            {
                var listOfLedgeCheckers = new List<LedgeChecker>();
                listOfLedgeCheckers.AddRange(control.GetComponentsInChildren<LedgeChecker>());

                foreach (var ledgeChecker in listOfLedgeCheckers)
                {
                    ledgeChecker.collider1.gameObject.GetComponent<MeshRenderer>().enabled = true;
                    ledgeChecker.collider2.gameObject.GetComponent<MeshRenderer>().enabled = true;
                }
            }
        }
        else
        {
            foreach (var control in CharacterManager.Instance.characters)
            {
                var listOfLedgeCheckers = new List<LedgeChecker>();
                listOfLedgeCheckers.AddRange(control.GetComponentsInChildren<LedgeChecker>());

                foreach (var ledgeChecker in listOfLedgeCheckers)
                {
                    ledgeChecker.collider1.gameObject.GetComponent<MeshRenderer>().enabled = false;
                    ledgeChecker.collider2.gameObject.GetComponent<MeshRenderer>().enabled = false;
                }
            }
        }
    }
}
