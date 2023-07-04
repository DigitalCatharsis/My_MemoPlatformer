using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace My_MemoPlatformer
{
    public class PlayGame_Button : MonoBehaviour
    {
        public void OnClick_PlayGame()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("L_CharacterSelect");
        }
    }
}
