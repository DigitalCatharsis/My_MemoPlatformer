using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace My_MemoPlatformer
{
    public class ManualInput : MonoBehaviour
    {
        private CharacterControl characterControl;

        public List<InputKeyType> _doubleTaps = new List<InputKeyType>();
        [SerializeField] private List<InputKeyType> _upKeys = new List<InputKeyType>();  //какие отпустил
        [SerializeField] private Dictionary<InputKeyType, float> _doubleTapTimings = new Dictionary<InputKeyType, float>();


        private void Awake()
        {
            characterControl = this.gameObject.GetComponent<CharacterControl>();
        }

        private void Update()
        {
            if (VirtualInputManager.Instance.turbo)
            {
                characterControl.turbo = true;
                ProcessDoubleTap(InputKeyType.KEY_TURBO);

            }
            else
            {
                characterControl.turbo = false;
                RemoveDoubleTap(InputKeyType.KEY_TURBO);
            }

            if (VirtualInputManager.Instance.moveUp)
            {
                characterControl.moveUp = true;
                ProcessDoubleTap(InputKeyType.KEY_MOVE_UP);
            }
            else
            {
                characterControl.moveUp = false;
                RemoveDoubleTap(InputKeyType.KEY_MOVE_UP);
            }

            if (VirtualInputManager.Instance.moveDown)
            {
                characterControl.moveDown = true;
                ProcessDoubleTap(InputKeyType.KEY_MOVE_DOWN);
            }
            else
            {
                characterControl.moveDown = false;
                RemoveDoubleTap(InputKeyType.KEY_MOVE_DOWN);
            }

            if (VirtualInputManager.Instance.moveRight)
            {
                characterControl.moveRight = true;
                ProcessDoubleTap(InputKeyType.KEY_MOVE_RIGHT);
            }
            else
            {
                characterControl.moveRight = false;
                RemoveDoubleTap(InputKeyType.KEY_MOVE_RIGHT);
            }

            if (VirtualInputManager.Instance.moveLeft)
            {
                characterControl.moveLeft = true;
                ProcessDoubleTap(InputKeyType.KEY_MOVE_LEFT);
            }
            else
            {
                characterControl.moveLeft = false;
                RemoveDoubleTap(InputKeyType.KEY_MOVE_LEFT);
            }

            if (VirtualInputManager.Instance.jump)
            {
                characterControl.jump = true;
                ProcessDoubleTap(InputKeyType.KEY_JUMP);
            }
            else
            {
                characterControl.jump = false;
                RemoveDoubleTap(InputKeyType.KEY_JUMP);
            }

            if (VirtualInputManager.Instance.attack)
            {
                characterControl.attack = true;
                ProcessDoubleTap(InputKeyType.KEY_ATTACK);
            }
            else
            {
                characterControl.attack = false;
                RemoveDoubleTap(InputKeyType.KEY_ATTACK);
            }

            //double tap running
            if (_doubleTaps.Contains(InputKeyType.KEY_MOVE_RIGHT) || _doubleTaps.Contains(InputKeyType.KEY_MOVE_LEFT))
            {
                characterControl.turbo = true;
            }

            //double tap running turn
            if (characterControl.moveRight && characterControl.moveLeft)
            {
                if (_doubleTaps.Contains(InputKeyType.KEY_MOVE_RIGHT) || _doubleTaps.Contains(InputKeyType.KEY_MOVE_LEFT))
                {
                    if (!_doubleTaps.Contains(InputKeyType.KEY_MOVE_RIGHT))
                    {
                        _doubleTaps.Add(InputKeyType.KEY_MOVE_RIGHT);
                    }
                    if (!_doubleTaps.Contains(InputKeyType.KEY_MOVE_LEFT))
                    {
                        _doubleTaps.Add(InputKeyType.KEY_MOVE_LEFT);
                    }
                }
            }
        }

        private void ProcessDoubleTap(InputKeyType keyType)
        {
            if (!_doubleTapTimings.ContainsKey(keyType))
            {
                _doubleTapTimings.Add(keyType, 0f);
            }

            if (_doubleTapTimings[keyType] == 0f || _upKeys.Contains(keyType)) // Setting double timing in the (first press) || (if let it go)
            {
                if (Time.time < _doubleTapTimings[keyType]) //we doing it in next frame
                {
                    if (!_doubleTaps.Contains(keyType))
                    {
                        _doubleTaps.Add(keyType);
                    }
                }
            }

            if (_upKeys.Contains(keyType))
            {
                _upKeys.Remove(keyType);
            }

            _doubleTapTimings[keyType] = Time.time + 0.15f;
        }

        private void RemoveDoubleTap(InputKeyType keyType)
        {
            if (_doubleTaps.Contains(keyType))
            {
                _doubleTaps.Remove(keyType);
            }

            if (!_upKeys.Contains(keyType))
            {
                _upKeys.Add(keyType);
            }
        }
    }
}