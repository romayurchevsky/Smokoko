using Scripts.PlayerCode;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Scripts.CommonCode
{
    public class AssetLoader : MonoBehaviour
    {
        [Header("Storages")]
        [SerializeField] private ResourcesStorage resourcesStorage;

        #region Variables
        private GameSceneType gameSceneType;
        private List<LoadedAsset> loadedAssets = new List<LoadedAsset>();
        private List<LoadedWeaponAsset> loadedWeaponAssets = new List<LoadedWeaponAsset>();
        #endregion

        private void Awake()
        {
            DependencyStorage.AssetLoader = this;
            GameManager.LobbyPrepereAction += LoadLobbyAssets;
            GameManager.LevelPrepereAction += LoadGamePlayAssets;
        }

        private void OnDestroy()
        {
            GameManager.LobbyPrepereAction -= LoadLobbyAssets;
            GameManager.LevelPrepereAction -= LoadGamePlayAssets;
        }

        public void LoadLobbyAssets()
        {
            StartCoroutine(LoadLobbyAssetsProcesss());
        }

        private IEnumerator LoadLobbyAssetsProcesss()
        {
            LoadGameAsset(AssetType.LobbyHeroAsset);
            while (GetAssetHandle(AssetType.LobbyHeroAsset).Status != AsyncOperationStatus.Succeeded) yield return null;
            LoadGameAsset(AssetType.HeroBodyAsset);
            while (GetAssetHandle(AssetType.HeroBodyAsset).Status != AsyncOperationStatus.Succeeded) yield return null;

            var weapons = DependencyStorage.PlayerStorage.ConcretePlayer.GameWeapons;
            for (int i = 0; i < weapons.Count; i++)
            {
                LoadWeaponAsset(weapons[i].HeroWeaponType);
                while (GetWeaponAssetHandle(weapons[i].HeroWeaponType).Status != AsyncOperationStatus.Succeeded) yield return null;
            }

            LoadScene(GameSceneType.Lobby);
        }


        public void LoadGamePlayAssets()
        {
            StartCoroutine(LoadGamePlayAssetProcesss());
        }

        private IEnumerator LoadGamePlayAssetProcesss()
        {
            LoadGameAsset(AssetType.BulletAsset);
            while (GetAssetHandle(AssetType.BulletAsset).Status != AsyncOperationStatus.Succeeded) yield return null;
            LoadGameAsset(AssetType.LevelAsset);
            while (GetAssetHandle(AssetType.LevelAsset).Status != AsyncOperationStatus.Succeeded) yield return null;
            LoadGameAsset(AssetType.EnemyAsset);
            while (GetAssetHandle(AssetType.EnemyAsset).Status != AsyncOperationStatus.Succeeded) yield return null;
            LoadGameAsset(AssetType.HeroAsset);
            while (GetAssetHandle(AssetType.HeroAsset).Status != AsyncOperationStatus.Succeeded) yield return null;
            LoadScene(GameSceneType.Gameplay);
        }

        private void LoadGameAsset(AssetType _assetType)
        {
            var tmp = loadedAssets.Find(a => a.AssetType == _assetType);
            if (tmp != null) return;
            var asset = resourcesStorage.GameAssets.Find(a => a.AssetType == _assetType);
            var handle = asset.GameSceneAsset.LoadAssetAsync<GameObject>();
            loadedAssets.Add(new LoadedAsset(_assetType, handle));
        }

        private void LoadWeaponAsset(HeroWeaponType _weaponType)
        {
            var tmp = loadedWeaponAssets.Find(a => a.WeaponType == _weaponType);
            if (tmp != null) return;
            var asset = resourcesStorage.WeaponAssets.Find(a => a.WeaponType == _weaponType);
            var handle = asset.GameSceneAsset.LoadAssetAsync<GameObject>();
            loadedWeaponAssets.Add(new LoadedWeaponAsset(_weaponType, handle));
        }

        public AsyncOperationHandle<GameObject> GetAssetHandle(AssetType _assetType)
        {
            return loadedAssets.Find(a => a.AssetType == _assetType).Handle;
        }

        public AsyncOperationHandle<GameObject> GetWeaponAssetHandle(HeroWeaponType _weaponType)
        {
            return loadedWeaponAssets.Find(a => a.WeaponType == _weaponType).Handle;
        }

        private async void LoadScene(GameSceneType _gameSceneType)
        {
            gameSceneType = _gameSceneType;
            var handle = Addressables.LoadSceneAsync(GetGameScene(_gameSceneType), LoadSceneMode.Single);
            handle.Completed += OnSceneLoaded;
            await handle.Task;
        }

        private void OnSceneLoaded(AsyncOperationHandle<SceneInstance> _scene)
        {
            if (_scene.Status == AsyncOperationStatus.Succeeded)
            {
                switch (gameSceneType)
                {
                    case GameSceneType.Lobby:
                        GameManager.LobbyStartAction?.Invoke();
                        break;
                    case GameSceneType.Gameplay:
                        GameManager.LevelStartAction?.Invoke();
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

    [Serializable]
    public class SceneAssets
    {
        [field: SerializeField] public GameSceneType GameSceneType { get; private set; }
        [field: SerializeField] public List<AssetType> AssetTypes { get; private set; } = new List<AssetType>();
        [field: SerializeField] public List<HeroWeaponType> WeaponAssets { get; private set; } = new List<HeroWeaponType>();
    }

    [Serializable]
    public class LoadedAsset
    {
        [field: SerializeField] public AssetType AssetType { get; private set; }
        [field: SerializeField] public AsyncOperationHandle<GameObject> Handle { get; private set; }

        public LoadedAsset(AssetType _assetType, AsyncOperationHandle<GameObject> _handle)
        {
            AssetType = _assetType;
            Handle = _handle;
        }
    }

    [Serializable]
    public class LoadedWeaponAsset
    {
        [field: SerializeField] public HeroWeaponType WeaponType { get; private set; }
        [field: SerializeField] public AsyncOperationHandle<GameObject> Handle { get; private set; }

        public LoadedWeaponAsset(HeroWeaponType _weaponType, AsyncOperationHandle<GameObject> _handle)
        {
            WeaponType = _weaponType;
            Handle = _handle;
        }
    }
}
