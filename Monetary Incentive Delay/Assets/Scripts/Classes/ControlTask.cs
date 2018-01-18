using Assets.Scripts.DataTypes;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Classes
{
	public class ControlTask : MonoBehaviour
	{
		private readonly CommonCore _core;
		public GameObject RewardPanel;
		public GameObject PunishmentPanel;

		public ControlTask()
		{
			_core = new CommonCore(TaskType.Control);
		}

		[UsedImplicitly]
		private void Start() {
			_core.Start(gameObject, RewardPanel, PunishmentPanel);
		}
	
		[UsedImplicitly]
		private void Update()
		{
			_core.Update();
		}
		
	}
}