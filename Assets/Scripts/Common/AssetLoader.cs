using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Scripts.CommonCode
{
    public class AssetLoader : MonoBehaviour
    {
        [Header("Storages")]
        [SerializeField] private ResourcesStorage resourcesStorage;

        #region Variables
        private List<LoadedAsset> loadedAssets = new List<LoadedAsset>();
        #endregion

        private void Awake()
        {
            DependencyStorage.AssetLoader = this;
        }

        public void LoadAssets()
        {
            for (int i = 0; i < resourcesStorage.GameAssets.Count; i++)
            {
                var handle = resourcesStorage.GameAssets[i].GameSceneAsset.LoadAssetAsync<GameObject>();
                loadedAssets.Add(new LoadedAsset(resourcesStorage.GameAssets[i].AssetType, handle));
            }
        }

        public AsyncOperationHandle<GameObject> GetAssetHandle(AssetType _assetType)
        {
            return loadedAssets.Find(a => a.AssetType == _assetType).Handle;
        }
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
}
