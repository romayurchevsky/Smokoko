using System;
using Scripts.PlayerCode;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Scripts.CommonCode
{
	public class GameManager : MonoBehaviour
	{
		[Header("Storages")]
		[SerializeField] private GameStorage gameStorage;
		[SerializeField] private ResourcesStorage resourcesStorage;

		#region Actions
		public static Action GameStartAction;
		public static Action PlayerLoadedAction;
		public static Action LobbyPrepereAction;
		public static Action LobbyStartAction;
		public static Action LevelPrepereAction;
		public static Action LevelStartAction;
		public static Action LevelLoadedAction;
		public static Action EquipWeaponAction;
		#endregion

		#region Variables
		#endregion

		private void Awake()
		{
			DontDestroyOnLoad(gameObject);
		}

		private void OnEnable()
		{
			GameStartAction += StartGame;
		}

		private void OnDisable()
		{
			GameStartAction -= StartGame;
		}

		private void OnApplicationQuit()
		{
			SavePlayer();
		}

		private void OnApplicationPause(bool _pause)
		{
			if (_pause)
			{
				DependencyStorage.PlayerStorage.SavePlayer();
			}
		}

		private void SavePlayer()
		{
			DependencyStorage.PlayerStorage.SavePlayer();
		}

		private void StartGame()
		{
			DependencyStorage.PlayerStorage.LoadPlayer();
		}
	}
}
