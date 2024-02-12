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
		private GameSceneType gameSceneType;
		#endregion

		private void Awake()
		{
			DontDestroyOnLoad(gameObject);
		}

		private void OnEnable()
		{
			GameStartAction += StartGame;
			LobbyPrepereAction += LobbyStart;
			LevelPrepereAction += LevelStart;
		}

		private void OnDisable()
		{
			GameStartAction -= StartGame;
			LobbyPrepereAction -= LobbyStart;
			LevelPrepereAction -= LevelStart;
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

		private void LobbyStart()
		{
			LoadScene(GameSceneType.Lobby);
		}

		private void LevelStart()
        {
			LoadScene(GameSceneType.Gameplay);
		}

		private async void LoadScene(GameSceneType _gameSceneType)
        {
			gameSceneType = _gameSceneType;
			var handle = Addressables.LoadSceneAsync(GetGameScene(_gameSceneType), LoadSceneMode.Single);
			handle.Completed += OnSceneLoaded;
			await handle.Task;
        }

		void OnSceneLoaded(AsyncOperationHandle<SceneInstance> _scene)
		{
			if (_scene.Status == AsyncOperationStatus.Succeeded)
			{
                switch (gameSceneType)
                {
                    case GameSceneType.Lobby:
						LobbyStartAction?.Invoke();
						break;
                    case GameSceneType.Gameplay:
						LevelStartAction?.Invoke();
						break;
                    default:
                        break;
                }
            }
			else
			{
				Debug.LogError("Failed to load scene at address: " + _scene.DebugName);
			}
		}

		private AssetReference GetGameScene(GameSceneType _gameSceneType)
        {
			return resourcesStorage.GameScenes.Find(s => s.GameSceneType == _gameSceneType).GameSceneAsset;
		}
	}
}
