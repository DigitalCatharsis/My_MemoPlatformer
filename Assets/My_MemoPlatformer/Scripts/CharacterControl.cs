using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TransitionParameter
{
    Move,
}


public class CharacterControl : MonoBehaviour
{
    private Vector3 _cameraOffset = new Vector3(0, 2.0f, -12.0f);

    // [SerializeField] private Camera _Playercamera;
    [SerializeField] public float _speed = 10;
    [SerializeField] Animator _animator;


    private NIS_PlayerControls _controls;

    public float _direction;
    public float Direction => _direction;
    
    private void Awake()
    {
        _controls = new NIS_PlayerControls();
        _controls.Enable();

        _controls.Land.Move.performed += ctx =>
        {
            _direction = ctx.ReadValue<float>();
        };

        _controls.Land.Jump.performed += ctx => Jump();
    }


    private void Jump()
    {

    }

    private void SetCamera()
    {
        //  _Playercamera.transform.position = transform.position + _cameraOffset;
    }


    private void LateUpdate()
    {
        SetCamera();
    }

}
