using System;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
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
        [SerializeField] private bool _debug_WallOverlappingStatus;

        [Space(10)]
        [SerializeField] private bool _displaySpheresAndColliders;
        [SerializeField] private bool _displayLedgeCheckers;

        [Space(10)]
        public SerializableList<HashData> test2 = new SerializableList<HashData> { };

        #region TODO
        /*
         * 1. ��������� AI 
         *      (��������� PathFind)
         *      (���������� � Jump)
         * 2. WallSlide ����� ����� ������ ��������� ��������� � Fall, �������, ��� WallSlide.Update ��� ���� �� ��������������
         * 3. RunToStop -> Run � ������ ������� ������ ������� �����
         * 4. ����������� ������ ��������� ��� �������� �����
         * 5. �������������� ����������, ������ �������� Pull(Momentum)
         * 6. ���������� ������� ������� (���������� �� Update � ������� Editor �����)
         * 7. ������� �������� ������, �������, ����
         * 8. �������� �������� �����
         * 9. ��������� ����� �� ����� �� �����. ������� �������.
         * 10. ������� ������� ��� ������ � ��� ����� ����������� ����� � �������
         * 11. ������ �� �������������
         * 12. Left Right Up Down � ����� �������?
         * 13. ��������� ����������� ������
         */
        #endregion

        private void Start()
        {
            for (var i = 0; i < 10; i++)
            {
                test2.Add(
                        new HashData
                        {
                            stateName = "State_" + i,
                            hashedNameValue = i,
                        }
                        );
            }

            var list = new List<HashData>();
        }

        public void UpdateDebugHelpersDict()
        {

        }

        private void Update()
        {
            DebugContainer_Data.Instance.debug_CameraState = _debug_CameraState;

            DebugContainer_Data.Instance.debug_Attack = _debug_Attack;

            DebugContainer_Data.Instance.debug_Instakill = _debug_Instakill;

            DebugContainer_Data.Instance.debug_Ragdoll = _debug_Ragdoll;

            DebugContainer_Data.Instance.debug_MoveForward = _debug_MoveForward;

            DebugContainer_Data.Instance.debug_Jump = _debug_Jump;

            DebugContainer_Data.Instance.debug_SpawnObjects = _debug_SpawnObjects;

            DebugContainer_Data.Instance.debug_InputManager = _debug_InputManager;

            DebugContainer_Data.Instance.debug_TransitionTiming = _debug_TransitionTiming;

            DebugContainer_Data.Instance.debug_TriggerDetector = _debug_TriggerDetector;

            DebugContainer_Data.Instance.debug_WallOverlappingStatus = _debug_WallOverlappingStatus;

            DebugContainer_Data.Instance.displaySpheresAndColliders = _displaySpheresAndColliders;

            DebugContainer_Data.Instance.displayLedgeCheckers = _displayLedgeCheckers;

            ChangeSpheresRendererStatus();
            ChangeLedgeCheckerStatus();
        }

        public List<HashData> FillHashDataList<T>(Type statesType_Collection, T hashValues_Collection) where T : IList<int>
        {
            var resultSubList = new List<HashData>();

            for (var i = 0; i < hashValues_Collection.Count; i++)
            {
                resultSubList.Add(
                    new HashData
                    {
                        hashedNameValue = hashValues_Collection[i],
                        stateName = Enum.GetName(statesType_Collection, i),
                    });
            }

            return resultSubList;
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
            if (_displayLedgeCheckers)
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
    [Serializable]
    public class HashData
    {
        [SerializeField]
        public string stateName;
        [SerializeField]
        public int hashedNameValue;
    }
}