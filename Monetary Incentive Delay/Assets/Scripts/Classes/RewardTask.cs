using Assets.Scripts.DataTypes;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Classes
{
	public class RewardTask : MonoBehaviour
	{
		private readonly CommonCore _core;

		public RewardTask()
		{
			_core = new CommonCore(TaskType.Reward);
		}

		[UsedImplicitly]
		private void Start()
		{
			_core.Start(gameObject, null, null);
		}

		[UsedImplicitly]
		private void Update()
		{
			_core.Update();
		}
	}
}