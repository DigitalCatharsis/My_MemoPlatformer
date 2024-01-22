using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

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
        [SerializeField] private bool _debug_HashManager;
        [SerializeField] private bool _debug_AI;

        [Space(10)]
        [SerializeField] private bool _displaySpheresAndColliders;
        [SerializeField] private bool _displayLedgeCheckers;

        #region TODO
        /*
         * 1. Пофиксить AI 
         *      (Сбивается PathFind)
         *      (Застревает в Jump)
         * 2. WallSlide отвой стены иногда мгновенно переходит в Fall, забавно, что WallSlide.Update при этом не отрабатывается
         * 5. Оптимизировать управление, убрать ненужный Pull(Momentum)
         * 6. Переделать текущий дебагер (избавиться от Update и слелать Editor класс)
         * 7. Сменить анимации прыжка, падения, бега
         * 8. Уеличить скорость атаки
         * 9. Перенести сцену со стола на комод. Сделать маршрут.
         * 10. Сделать фабрику для спавна и сам спавн противников рядом с игроком
         * 11. Рыдать от безысходности
         * 12. Left Right Up Down в стейт машиине?
         * 13. Исправить приближение камеры
         */
        #endregion

        private void Start()
        {
            //for (var i = 0; i < 10; i++)
            //{
            //    testListOfLists.instance.Add(
            //        new Serializable_SubList<StatesHashData>
            //        {
            //            subList = new List<StatesHashData>()
            //            {
            //                new StatesHashData
            //                {
            //                    stateName = "SubList_Data_" + i,
            //                    hashedNameValue = i,

            //                },
            //            },
            //        }); ;
            //} 

            //var HN = new int[] { 0,1, 2 };
            //var SN = new string[] { "0","1","2"};

            //AddValuesToTestOfLists(HN, SN);

        }

        //public void AddValuesToTestOfLists(int[] hashesNames, string[] stateNamess)
        //{
        //    testListOfLists.instance.Add(new Serializable_SubList<StatesHashData>());

        //    for (int i = 0; i < hashesNames.Length; i++)
        //    {
        //        testListOfLists.instance.Last().subList.Add(new StatesHashData { hashedNameValue = hashesNames[i], stateName = stateNamess[i] });
        //    }
        //}

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
            DebugContainer_Data.Instance.debug_HashManager = _debug_HashManager;
            DebugContainer_Data.Instance.debug_AI = _debug_AI;

            DebugContainer_Data.Instance.displaySpheresAndColliders = _displaySpheresAndColliders;

            DebugContainer_Data.Instance.displayLedgeCheckers = _displayLedgeCheckers;

            ChangeSpheresRendererStatus(_displaySpheresAndColliders);
            ChangeLedgeCheckerStatus(_displayLedgeCheckers);
        }

        //public List<StatesHashData> FillHashDataList<T>(Type statesType_Collection, T hashValues_Collection) where T : IList<int>
        //{
        //    var resultSubList = new List<StatesHashData>();

        //    for (var i = 0; i < hashValues_Collection.Count; i++)
        //    {
        //        resultSubList.Add(
        //            new StatesHashData
        //            {
        //                hashedNameValue = hashValues_Collection[i],
        //                stateName = Enum.GetName(statesType_Collection, i),
        //            });
        //    }

        //    return resultSubList;
        //}

        private void ChangeSpheresRendererStatus(bool isEnabled)
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
                    sphere.GetComponent<MeshRenderer>().enabled = isEnabled;
                }
            }
        }

        private void ChangeLedgeCheckerStatus(bool enabled)
        {
            foreach (var control in CharacterManager.Instance.characters)
            {
                var listOfLedgeCheckers = new List<LedgeChecker>();
                listOfLedgeCheckers.AddRange(control.GetComponentsInChildren<LedgeChecker>());

                foreach (var ledgeChecker in listOfLedgeCheckers)
                {
                    ledgeChecker.collider1.gameObject.GetComponent<MeshRenderer>().enabled = enabled;
                    ledgeChecker.collider2.gameObject.GetComponent<MeshRenderer>().enabled = enabled;
                }
            }
        }
    }

}