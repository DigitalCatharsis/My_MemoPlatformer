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
    [SerializeField] private bool _debug_WallOverlappingStatus;

    [Space(10)]
    [SerializeField] private bool _displaySpheresAndColliders;
    [SerializeField] private bool _displayLedgeCheckers;

    #region TODO
    /*
     * 1. Пофиксить AI 
     *      (Сбивается PathFind)
     *      (Застревает в Jump)
     * 2. WallSlide отвой стены иногда мгновенно переходит в Fall, забавно, что WallSlide.Update при этом не отрабатывается
     * 3. RunToStop -> Run в другую сторогу делает двойной рывок
     * 4. Метательное оружие цепляется под странным углом
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
        My_MemoPlatformer.DebugContainer_Data.Instance.debug_WallOverlappingStatus = _debug_WallOverlappingStatus;

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
