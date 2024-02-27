using System.Collections.Generic;
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

        private void OnEnable()
        {
            manualInputData = new ManualInput_Data
            {
                DoubleTapDown = IsDoubleTap_Down,
                DoubleTapUp = IsDoubleTap_Up,
            };

            subComponentProcessor.manualInput_Data = manualInputData;
            subComponentProcessor.arrSubComponents[(int)SubComponentType.MANUAL_INPUT] = this;
        }

        //private void Start()
        //{
        //    manualInputData = new ManualInput_Data
        //    {
        //        DoubleTapDown = IsDoubleTap_Down,
        //        DoubleTapUp = IsDoubleTap_Up,
        //    };

        //    subComponentProcessor.manualInput_Data = manualInputData;
        //    subComponentProcessor.arrSubComponents[(int)SubComponentType.MANUAL_INPUT] = this;
        //}

        public override void OnFixedUpdate()
        {
            throw new System.NotImplementedException();
        }

        public override void OnUpdate()
        {
            UpdateDoubleTapsInDictionary();
            DoubleTapsMovement();
        }

        private void UpdateDoubleTapsInDictionary()
        {
            if (VirtualInputManager.Instance.turbo)
            {
                Control.turbo = true;
                ProcessDoubleTap(InputKeyType.KEY_TURBO);
            }
            else
            {
                Control.turbo = false;
                RemoveDoubleTap(InputKeyType.KEY_TURBO);
            }

            if (VirtualInputManager.Instance.moveUp)
            {
                Control.moveUp = true;
                ProcessDoubleTap(InputKeyType.KEY_MOVE_UP);
            }
            else
            {
                Control.moveUp = false;
                RemoveDoubleTap(InputKeyType.KEY_MOVE_UP);
            }

            if (VirtualInputManager.Instance.moveDown)
            {
                Control.moveDown = true;
                ProcessDoubleTap(InputKeyType.KEY_MOVE_DOWN);
            }
            else
            {
                Control.moveDown = false;
                RemoveDoubleTap(InputKeyType.KEY_MOVE_DOWN);
            }

            if (VirtualInputManager.Instance.moveRight)
            {
                Control.moveRight = true;
                ProcessDoubleTap(InputKeyType.KEY_MOVE_RIGHT);
            }
            else
            {
                Control.moveRight = false;
                RemoveDoubleTap(InputKeyType.KEY_MOVE_RIGHT);
            }

            if (VirtualInputManager.Instance.moveLeft)
            {
                Control.moveLeft = true;
                ProcessDoubleTap(InputKeyType.KEY_MOVE_LEFT);
            }
            else
            {
                Control.moveLeft = false;
                RemoveDoubleTap(InputKeyType.KEY_MOVE_LEFT);
            }

            if (VirtualInputManager.Instance.jump)
            {
                Control.jump = true;
                ProcessDoubleTap(InputKeyType.KEY_JUMP);
            }
            else
            {
                Control.jump = false;
                RemoveDoubleTap(InputKeyType.KEY_JUMP);
            }

            if (VirtualInputManager.Instance.block)
            {
                Control.block = true;
                ProcessDoubleTap(InputKeyType.KEY_BLOCK);
            }
            else
            {
                Control.block = false;
                RemoveDoubleTap(InputKeyType.KEY_BLOCK);
            }

            if (VirtualInputManager.Instance.attack)
            {
                Control.attack = true;
                ProcessDoubleTap(InputKeyType.KEY_ATTACK);
            }
            else
            {
                Control.attack = false;
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
                Control.turbo = true;
            }

            //double tap running turn

            if (Control.moveRight && Control.moveLeft)
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