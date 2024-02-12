using Scripts.CommonCode;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.LevelCode
{
    public class LevelComtroller : MonoBehaviour
    {
        [Header("Storages")]
        [SerializeField] private ResourcesStorage resourcesStorage;

        [Header("Components")]
        [SerializeField] private List<Transform> enemyContainers = new List<Transform>();
        [SerializeField] private Transform heroContainer;

        private void Awake()
        {
            LoadLevel();
        }

        private void LoadLevel()
        {
            LoadEnemy();
            LoadEHero();

            GameManager.LevelLoadedAction?.Invoke();
        }

        private void LoadEnemy()
        {
            for (int i = 0; i < enemyContainers.Count; i++)
            {
                Instantiate(DependencyStorage.AssetLoader.GetAssetHandle(AssetType.EnemyAsset).Result, enemyContainers[i]);
            }
        }


        private void LoadEHero()
        {
            Instantiate(DependencyStorage.AssetLoader.GetAssetHandle(AssetType.HeroAsset).Result, heroContainer);
        }
    }
}
