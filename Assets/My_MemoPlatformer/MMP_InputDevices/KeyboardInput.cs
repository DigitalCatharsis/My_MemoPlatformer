using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

namespace My_MemoPlatformer
{
    public class KeyboardInput : MonoBehaviour
    {
       void Update()
        {
            if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.LeftShift))
            {
                VirtualInputManager.Instance.turbo = true;
            }
            else
            {
                VirtualInputManager.Instance.turbo = false;
            }

            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                VirtualInputManager.Instance.moveUp = true;
            }
            else
            {
                VirtualInputManager.Instance.moveUp = false;
            }

            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                VirtualInputManager.Instance.moveDown = true;
            }
            else
            {
                VirtualInputManager.Instance.moveDown = false;
            }

            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                VirtualInputManager.Instance.moveRight = true;
            }
            else
            {
                VirtualInputManager.Instance.moveRight = false;
            }

            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                VirtualInputManager.Instance.moveLeft = true;
            }
            else
            {
                VirtualInputManager.Instance.moveLeft = false;
            }

            if (Input.GetKey(KeyCode.Space))
            {
                VirtualInputManager.Instance.jump = true;
            }
            else
            {
                VirtualInputManager.Instance.jump = false;
            }

            //    if (Input.GetKeyDown(KeyCode.C))
            if (Input.GetKey(KeyCode.C))
            {
                VirtualInputManager.Instance.attack = true;
            }
            else
            {
                VirtualInputManager.Instance.attack = false;
            }            
            
            //Temp Restart
            if (Input.GetKey(KeyCode.R))
            {
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            }
        }

    }
}

