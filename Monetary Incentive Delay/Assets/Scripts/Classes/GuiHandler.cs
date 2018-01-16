using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Classes
{
    public class GuiHandler : MonoBehaviour
    {
	    private static int _currentScene;

	    public static void NextScene()
	    {
		    SceneManager.LoadScene(_currentScene + 1, LoadSceneMode.Single);
	    }

		public static void StaticLoadScene(int sceneIndex)
        {
	        _currentScene = sceneIndex;
            SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
        }

        public void LoadScene(int sceneIndex)
        {
	        _currentScene = sceneIndex;
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