using UnityEngine;

namespace My_MemoPlatformer
{
    public class ChangeScene : MonoBehaviour
    {
        [SerializeField] private string _nextScene;

        public void ChangeSceneTo()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(_nextScene);
        }
    }
}

