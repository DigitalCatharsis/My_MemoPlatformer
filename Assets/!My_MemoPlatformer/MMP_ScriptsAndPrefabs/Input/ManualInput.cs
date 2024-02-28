using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace My_MemoPlatformer
{
    //1. Looking for key pressing in PlayerInput
    //2. The pressed keys saved up to the VirtualInputManager, and bind up
    //3. ManualInput Relay that keys to the CharacterControl
    //4. CharacterControl contains actual fields about character moving etc.

    public class ManualInput : SubComponent
    {
        public ManualInput_Data manualInputData;

        [SerializeField] private List<InputKeyType> _doubleTaps = new List<InputKeyType>();
        [SerializeField] private List<InputKeyType> _upKeys = new List<InputKeyType>();  //какие отпустил
        [SerializeField] private Dictionary<InputKeyType, float> _doubleTapTimings = new Dictionary<InputKeyType, float>();

        [SerializeField] private float _doubleTapTime = 0.18f;
        [SerializeField] private bool useDoubleTapsForMoving;

        public override void OnComponentEnabled()
        {
            manualInputData = new ManualInput_Data
            {
                DoubleTapDown = IsDoubleTap_Down,
                DoubleTapUp = IsDoubleTap_Up,
            };

            subComponentProcessor.manualInput_Data = manualInputData;
        }

        public override void OnFixedUpdate()
        {
        }

        public override void OnUpdate()
        {
            if (control.AICONTROLLER_DATA.aiType != AI_Type.Player)
            {
                return;
            }

            UpdateDoubleTapsInDictionary();
            DoubleTapsMovement();
        }

        private void UpdateDoubleTapsInDictionary()
        {
            if (VirtualInputManager.Instance.turbo)
            {
                control.turbo = true;
                ProcessDoubleTap(InputKeyType.KEY_TURBO);
            }
            else
            {
                control.turbo = false;
                RemoveDoubleTap(InputKeyType.KEY_TURBO);
            }

            if (VirtualInputManager.Instance.moveUp)
            {
                control.moveUp = true;
                ProcessDoubleTap(InputKeyType.KEY_MOVE_UP);
            }
            else
            {
                control.moveUp = false;
                RemoveDoubleTap(InputKeyType.KEY_MOVE_UP);
            }

            if (VirtualInputManager.Instance.moveDown)
            {
                control.moveDown = true;
                ProcessDoubleTap(InputKeyType.KEY_MOVE_DOWN);
            }
            else
            {
                control.moveDown = false;
                RemoveDoubleTap(InputKeyType.KEY_MOVE_DOWN);
            }

            if (VirtualInputManager.Instance.moveRight)
            {
                control.moveRight = true;
                ProcessDoubleTap(InputKeyType.KEY_MOVE_RIGHT);
            }
            else
            {
                control.moveRight = false;
                RemoveDoubleTap(InputKeyType.KEY_MOVE_RIGHT);
            }

            if (VirtualInputManager.Instance.moveLeft)
            {
                control.moveLeft = true;
                ProcessDoubleTap(InputKeyType.KEY_MOVE_LEFT);
            }
            else
            {
                control.moveLeft = false;
                RemoveDoubleTap(InputKeyType.KEY_MOVE_LEFT);
            }

            if (VirtualInputManager.Instance.jump)
            {
                control.jump = true;
                ProcessDoubleTap(InputKeyType.KEY_JUMP);
            }
            else
            {
                control.jump = false;
                RemoveDoubleTap(InputKeyType.KEY_JUMP);
            }

            if (VirtualInputManager.Instance.block)
            {
                control.block = true;
                ProcessDoubleTap(InputKeyType.KEY_BLOCK);
            }
            else
            {
                control.block = false;
                RemoveDoubleTap(InputKeyType.KEY_BLOCK);
            }

            if (VirtualInputManager.Instance.attack)
            {
                control.attack = true;
                ProcessDoubleTap(InputKeyType.KEY_ATTACK);
            }
            else
            {
                control.attack = false;
                RemoveDoubleTap(InputKeyType.KEY_ATTACK);
            }
        }

        private void DoubleTapsMovement()
        {
            if (!useDoubleTapsForMoving)
            {
                return;
            }

            //double tap running
            if (_doubleTaps.Contains(InputKeyType.KEY_MOVE_RIGHT) ||
                _doubleTaps.Contains(InputKeyType.KEY_MOVE_LEFT))
            {
                control.turbo = true;
            }

            //double tap running turn

            if (control.moveRight && control.moveLeft)
            {
                if (_doubleTaps.Contains(InputKeyType.KEY_MOVE_RIGHT) ||
                    _doubleTaps.Contains(InputKeyType.KEY_MOVE_LEFT))
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

        private bool IsDoubleTap_Up()
        {
            if (_doubleTaps.Contains(InputKeyType.KEY_MOVE_UP))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsDoubleTap_Down()
        {
            if (_doubleTaps.Contains(InputKeyType.KEY_MOVE_DOWN))
            {
                return true;
            }
            else
            {
                return false;
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

            _doubleTapTimings[keyType] = Time.time + _doubleTapTime;
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