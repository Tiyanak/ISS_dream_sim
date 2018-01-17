using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Handlers
{
    public class GuiHandler : MonoBehaviour
    {
	    private static int _currentScene;
	    private const int MainMenu = 0;
	    private const int Baseline = 1;
	    private const int Tests = 2;
	    public GameObject SettingsPanel;

		public static void NextScene()
	    {
		    SceneManager.LoadScene(++_currentScene, LoadSceneMode.Single);
	    }

	    public static void GoToMainMenu()
	    {
		    _currentScene = MainMenu;
		    SceneManager.LoadScene(_currentScene, LoadSceneMode.Single);
	    }

	    public static void GoToBaseline()
	    {
		    _currentScene = Baseline;
		    SceneManager.LoadScene(_currentScene, LoadSceneMode.Single);
	    }

	    public static void GoToTests()
	    {
		    _currentScene = Tests;
		    SceneManager.LoadScene(_currentScene, LoadSceneMode.Single);
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

	    public void OpenSettings()
	    {
		    SettingsPanel.SetActive(true);
	    }

	    public void CloseSettings()
	    {
		    SettingsPanel.SetActive(false);
	    }

	    public void SaveSettings()
	    {
			//TaskSettings[] ts = null;	//TODO change this
			//GlobalSettings.Gs.SetSettings(ts);
		    SettingsPanel.SetActive(false);
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