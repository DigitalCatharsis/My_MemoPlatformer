using UnityEngine;
using UnityEngine.SceneManagement;

namespace My_MemoPlatformer
{
    public class PlayerInput : MonoBehaviour
    {
        //1. Looking for key pressing in PlayerInput
        //2. The pressed keys saved up to the VirtualInputManager, and bind up
        //3. ManualInput Relay that keys to the CharacterControl
        //4. CharacterControl contains actual fields about character moving etc.

        public SavedKeys SavedKeys;

       void Update()
        {
            if (Input.GetKey(VirtualInputManager.Instance.DicKeys[InputKeyType.KEY_TURBO]))
            {
                VirtualInputManager.Instance.turbo = true;
            }
            else
            {
                VirtualInputManager.Instance.turbo = false;
            }

            if (Input.GetKey(VirtualInputManager.Instance.DicKeys[InputKeyType.KEY_MOVE_UP]))
            {
                VirtualInputManager.Instance.moveUp = true;
            }
            else
            {
                VirtualInputManager.Instance.moveUp = false;
            }

            if (Input.GetKey(VirtualInputManager.Instance.DicKeys[InputKeyType.KEY_MOVE_DOWN]))
            {
                VirtualInputManager.Instance.moveDown = true;
            }
            else
            {
                VirtualInputManager.Instance.moveDown = false;
            }

            if (Input.GetKey(VirtualInputManager.Instance.DicKeys[InputKeyType.KEY_MOVE_RIGHT]))
            {
                VirtualInputManager.Instance.moveRight = true;
            }
            else
            {
                VirtualInputManager.Instance.moveRight = false;
            }

            if (Input.GetKey(VirtualInputManager.Instance.DicKeys[InputKeyType.KEY_MOVE_LEFT]))
            {
                VirtualInputManager.Instance.moveLeft = true;
            }
            else
            {
                VirtualInputManager.Instance.moveLeft = false;
            }

            if (Input.GetKey(VirtualInputManager.Instance.DicKeys[InputKeyType.KEY_JUMP]))
            {
                VirtualInputManager.Instance.jump = true;
            }
            else
            {
                VirtualInputManager.Instance.jump = false;
            }

            if (Input.GetKey(VirtualInputManager.Instance.DicKeys[InputKeyType.KEY_BLOCK]))
            {
                VirtualInputManager.Instance.block = true;
            }
            else
            {
                VirtualInputManager.Instance.block = false;
            }

            //    if (Input.GetKeyDown(KeyCode.C))
            if (Input.GetKey(VirtualInputManager.Instance.DicKeys[InputKeyType.KEY_ATTACK]))
            {
                VirtualInputManager.Instance.attack = true;
            }
            else
            {
                VirtualInputManager.Instance.attack = false;
            }            
            
            //Temp Restart
            if (Input.GetKey(VirtualInputManager.Instance.DicKeys[InputKeyType.KEY_RESTART]))
            {
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            }
        }

    }
}

