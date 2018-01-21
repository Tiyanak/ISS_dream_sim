using Assets.Scripts.DataTypes;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Classes
{
	public class PunishmentTask : MonoBehaviour
	{
		private readonly CommonCore _core;
		public GameObject SpammingText;

		public PunishmentTask()
		{
			_core = new CommonCore(TaskType.Punishment);
		}

		[UsedImplicitly]
		private void Start()
		{
			_core.Start(gameObject, null, null, SpammingText);
		}

		[UsedImplicitly]
		private void Update()
		{
			_core.Update();
		}
	}
}