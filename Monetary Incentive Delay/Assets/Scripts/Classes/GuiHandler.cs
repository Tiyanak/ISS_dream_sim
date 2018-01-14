using UnityEngine;
using UnityEngine.SceneManagement;

namespace Classes
{
    public class GuiHandler : MonoBehaviour
    {
        protected static void StaticLoadScene(int sceneIndex)
        {
            SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
        }

        public void LoadScene(int sceneIndex)
        {
            StaticLoadScene(sceneIndex);
        }
        
        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
		#endif
        }
    }
}