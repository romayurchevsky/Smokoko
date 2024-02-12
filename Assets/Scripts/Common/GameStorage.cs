using UnityEngine;

namespace Scripts.CommonCode
{
	[CreateAssetMenu(menuName = "ScriptableObjects/GameStorage", fileName = "GameStorage")]
	public class GameStorage : ScriptableObject
	{
		[Header("GameBaseParameters")]
		[SerializeField] private GameBaseParameters gameBaseParameters;

		#region Geters/Seters
		public GameBaseParameters GameBaseParameters { get => gameBaseParameters; }
		#endregion

	}
}
