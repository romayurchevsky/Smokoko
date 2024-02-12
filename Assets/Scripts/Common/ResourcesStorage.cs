using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Scripts.CommonCode
{
    [CreateAssetMenu(menuName = "ScriptableObjects/ResourcesStorage", fileName = "ResourcesStorage")]
    public class ResourcesStorage : ScriptableObject
    {
        [field: SerializeField] public List<GameAsset> GameAssets { get; private set; } = new List<GameAsset>();
        [field: SerializeField] public List<GameScene> GameScenes { get; private set; } = new List<GameScene>();
    }

    [Serializable]
    public class GameAsset
    {
        [field: SerializeField] public AssetType AssetType { get; private set; }
        [field: SerializeField] public AssetReference GameSceneAsset { get; private set; }
    }

    [Serializable]
    public class GameScene
    {
        [field: SerializeField] public GameSceneType GameSceneType { get; private set; }
        [field: SerializeField] public AssetReference GameSceneAsset { get; private set; }
    }

    public enum GameSceneType
    {
        Preloader,
        Lobby,
        Gameplay
    }

    public enum AssetType
    {
        LevelAsset,
        HeroAsset,
        LobbyHeroAsset,
        HeroBodyAsset,
        EnemyAsset,
        BulletAsset,
        WeaponAsset
    }

}
