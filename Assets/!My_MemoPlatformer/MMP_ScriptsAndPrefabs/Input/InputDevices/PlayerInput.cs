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
            VirtualInputManager.Instance.turbo = Input.GetKey(VirtualInputManager.Instance.dicKeys[InputKeyType.KEY_TURBO]);
            VirtualInputManager.Instance.moveUp = Input.GetKey(VirtualInputManager.Instance.dicKeys[InputKeyType.KEY_MOVE_UP]);
            VirtualInputManager.Instance.moveLeft = Input.GetKey(VirtualInputManager.Instance.dicKeys[InputKeyType.KEY_MOVE_LEFT]);
            VirtualInputManager.Instance.moveRight = Input.GetKey(VirtualInputManager.Instance.dicKeys[InputKeyType.KEY_MOVE_RIGHT]);
            VirtualInputManager.Instance.moveDown = Input.GetKey(VirtualInputManager.Instance.dicKeys[InputKeyType.KEY_MOVE_DOWN]);
            VirtualInputManager.Instance.jump = Input.GetKey(VirtualInputManager.Instance.dicKeys[InputKeyType.KEY_JUMP]);
            VirtualInputManager.Instance.block = Input.GetKey(VirtualInputManager.Instance.dicKeys[InputKeyType.KEY_BLOCK]);
            VirtualInputManager.Instance.attack = Input.GetKey(VirtualInputManager.Instance.dicKeys[InputKeyType.KEY_ATTACK]);     
            
            //Temp Restart
            if (Input.GetKey(VirtualInputManager.Instance.dicKeys[InputKeyType.KEY_RESTART]))
            {
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            }
        }

    }
}
