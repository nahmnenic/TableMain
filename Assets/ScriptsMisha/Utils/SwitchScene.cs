using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScriptsMisha.Utils
{
    public class SwitchScene : MonoBehaviour
    {
        [SerializeField] private string _sceneName;

        public void OnSwitch()
        {
            SceneManager.LoadScene(_sceneName);
        }
    }
}
