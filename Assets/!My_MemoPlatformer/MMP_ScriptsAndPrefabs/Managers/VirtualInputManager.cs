using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{

    public enum InputKeyType
    {
        KEY_MOVE_UP,
        KEY_MOVE_DOWN,
        KEY_MOVE_LEFT,
        KEY_MOVE_RIGHT,

        KEY_ATTACK,
        KEY_TURBO,
        KEY_JUMP,
        KEY_BLOCK,

        KEY_RESTART,
    }

    //1. Looking for key pressing in PlayerInput
    //2. The pressed keys saved up to the VirtualInputManager, and bind up
    //3. ManualInput Relay that keys to the CharacterControl
    //4. CharacterControl contains actual fields about character moving etc.

    public class VirtualInputManager : Singleton<VirtualInputManager>
    {
        public PlayerInput playerInput;

        public bool turbo;
        public bool moveRight;
        public bool moveLeft;
        public bool moveUp;
        public bool moveDown;
        public bool jump;
        public bool attack;
        public bool block;

        [Header("Custom Key Bindings")]
        public bool useCustomKeys;
        [Space(5)]
        public bool bind_MoveUp;
        public bool bind_MoveDown;
        public bool bind_MoveRight;
        public bool bind_MoveLeft;
        public bool bind_Attack;
        public bool bind_Jump;
        public bool bind_Turbo;
        public bool bind_Restart;
        public bool bind_Block;

        [Space(10)]
        public Dictionary<InputKeyType, KeyCode> dicKeys = new Dictionary<InputKeyType, KeyCode>();
        [SerializeField] private KeyCode[] _possibleKeys;


        private void Awake()
        {
            _possibleKeys = System.Enum.GetValues(typeof(KeyCode)) as KeyCode[];

            var piObj = Instantiate(Resources.Load("PlayerInput", typeof (GameObject))) as GameObject;
            playerInput = piObj.GetComponent<PlayerInput>();
        }

        public void LoadKeys()
        {
            if (playerInput.SavedKeys.keyCodesList.Count > 0)
            {
                foreach(KeyCode k in playerInput.SavedKeys.keyCodesList)
                {
                    if (k == KeyCode.None)
                    {
                        SetDefaultKeys();
                        break;
                    }
                }
            }
            else 
            {
                SetDefaultKeys(); 
            }

            for (int i = 0; i < playerInput.SavedKeys.keyCodesList.Count; i++)
            {
                dicKeys[(InputKeyType)i] = playerInput.SavedKeys.keyCodesList[i];
            }
        }

        public void SaveKeys()
        {
            playerInput.SavedKeys.keyCodesList.Clear();

            var count = System.Enum.GetValues(typeof(InputKeyType)).Length;

            for (int i = 0; i < count; ++i) 
            {
                playerInput.SavedKeys.keyCodesList.Add(dicKeys[(InputKeyType)i]);
            }
        }

        public void SetDefaultKeys()
        {
            dicKeys.Clear();

            dicKeys.Add(InputKeyType.KEY_MOVE_UP,        KeyCode.UpArrow);
            dicKeys.Add(InputKeyType.KEY_MOVE_DOWN,      KeyCode.DownArrow);
            dicKeys.Add(InputKeyType.KEY_MOVE_LEFT,      KeyCode.LeftArrow);
            dicKeys.Add(InputKeyType.KEY_MOVE_RIGHT,     KeyCode.RightArrow);

            dicKeys.Add(InputKeyType.KEY_ATTACK,         KeyCode.C);
            dicKeys.Add(InputKeyType.KEY_JUMP,           KeyCode.X);
            dicKeys.Add(InputKeyType.KEY_TURBO,          KeyCode.Z);

            dicKeys.Add(InputKeyType.KEY_BLOCK,          KeyCode.F);

            dicKeys.Add(InputKeyType.KEY_RESTART,        KeyCode.R);

            SaveKeys();
        }

        private void Update()
        {
            //if (!useCustomKeys)
            //{
            //    return;
            //}

            if (useCustomKeys)
            {
                if (bind_MoveUp)
                {
                    if (KeyIsChanged(InputKeyType.KEY_MOVE_UP))
                    {
                        bind_MoveUp = false;
                    }
                }
                if (bind_MoveDown)
                {
                    if (KeyIsChanged(InputKeyType.KEY_MOVE_DOWN))
                    {
                        bind_MoveDown = false;
                    }
                }
                if (bind_MoveRight)
                {
                    if (KeyIsChanged(InputKeyType.KEY_MOVE_RIGHT))
                    {
                        bind_MoveRight = false;
                    }
                }
                if (bind_MoveLeft)
                {
                    if (KeyIsChanged(InputKeyType.KEY_MOVE_LEFT))
                    {
                        bind_MoveLeft = false;
                    }
                }
                if (bind_Attack)
                {
                    if (KeyIsChanged(InputKeyType.KEY_ATTACK))
                    {
                        bind_Attack = false;
                    }
                }
                if (bind_Jump)
                {
                    if (KeyIsChanged(InputKeyType.KEY_JUMP))
                    {
                        bind_Jump = false;
                    }
                }
                if (bind_Turbo)
                {
                    if (KeyIsChanged(InputKeyType.KEY_TURBO))
                    {
                        bind_Turbo = false;
                    }
                }
                if (bind_Block)
                {
                    if (KeyIsChanged(InputKeyType.KEY_BLOCK))
                    {
                        bind_Block = false;
                    }
                }

                if (bind_Restart)
                {
                    if (KeyIsChanged(InputKeyType.KEY_RESTART))
                    {
                        bind_Restart = false;
                    }
                }
            }
        }

        private void SetCustomKey(InputKeyType inputKey, KeyCode key)
        {
            if (DebugContainer.Instance.debug_InputManager)
            {
                Debug.Log("Key changed: " + inputKey.ToString() + " -> " + key.ToString());
            }

            if (!dicKeys.ContainsKey(inputKey))
            {
                dicKeys.Add(inputKey, key);
            }
            else
            {
                dicKeys[inputKey] = key;
            }

            SaveKeys();
        }

        private bool KeyIsChanged(InputKeyType inputKey)
        {
            if (Input.anyKey)
            {
                foreach (KeyCode k in _possibleKeys)
                {
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        continue;
                    }

                    if (Input.GetKeyDown(k))
                    {
                        SetCustomKey(inputKey, k);
                        return true;
                    }
                }
            }

            return false;
        }
    }
}