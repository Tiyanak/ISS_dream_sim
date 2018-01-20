using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Handlers.GUI
{
	public class MenuHandler : MonoBehaviour
	{
		public Canvas Canvas;
		public Button ResumeButton;
		public Button MainMenuButton;
		public static MenuHandler Menu;
		private static bool _escPressed;

		[UsedImplicitly]
		private void Start()
		{
			Button btn = ResumeButton.GetComponent<Button>();
			btn.onClick.AddListener(Resume);
			Button btn2 = MainMenuButton.GetComponent<Button>();
			btn2.onClick.AddListener(GoToMainMenu);
		}

		[UsedImplicitly]
		private void Awake()
		{
			if (Menu == null)
			{
				Menu = this;
				DontDestroyOnLoad(gameObject);
			}
			else
			{
				DestroyImmediate(gameObject);
			}
		}

		public void Update()
		{
			CheckIfEscPressed();
			if (_escPressed)
			{
				Time.timeScale = 0;
				Canvas.gameObject.SetActive(true);
			}
		}

		private void Resume()
		{
			Canvas.gameObject.SetActive(false);
			Time.timeScale = 1;
		}

		private void GoToMainMenu()
		{
			Canvas.gameObject.SetActive(false);
			Time.timeScale = 1;
			GuiHandler.GoToMainMenu();
		}

		private void CheckIfEscPressed()
		{
			_escPressed = Input.GetKeyDown("escape");
		}
	}
}