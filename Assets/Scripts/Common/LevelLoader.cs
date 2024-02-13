using Scripts.CommonCode;
using Scripts.PlayerCode;
using UnityEngine;

namespace Scripts.LevelCode
{
    public class LevelLoader : MonoBehaviour
    {
        [Header("Storages")]
        [SerializeField] private ResourcesStorage resourcesStorage;

        [Header("Components")]
        [SerializeField] private Transform container;

        private void Awake()
        {
            //LoadLevel();

            GameManager.LevelStartAction += LoadLevel;
        }

        private void OnDestroy()
        {
            GameManager.LevelStartAction -= LoadLevel;
        }

        private void LoadLevel()
        {
            Instantiate(DependencyStorage.AssetLoader.GetAssetHandle(AssetType.LevelAsset).Result, container);
        }
    }
}
