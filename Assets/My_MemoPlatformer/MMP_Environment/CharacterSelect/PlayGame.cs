using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{

    public class PlayGame : MonoBehaviour
    {
        public CharacterSelect characterSelect;
        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return)) 
            {
                if (characterSelect.selectedCharacterType != PlayableCharacterType.NONE)
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(MMP_Scenes.L_LevelStart.ToString());
                }
                else
                {
                    Debug.Log("Must select character first");
                }
            }
        }
    }
}