using UnityEngine;

namespace Scripts.CommonCode
{
    public class LobbyController : MonoBehaviour
    {
        [Header("Storages")]
        [SerializeField] private ResourcesStorage resourcesStorage;

        [Header("Components")]
        [SerializeField] private Transform heroContainer;

        private void Awake()
        {
            LoadLobby();
        }

        private void LoadLobby()
        {
            Instantiate(DependencyStorage.AssetLoader.GetAssetHandle(AssetType.LobbyHeroAsset).Result, heroContainer);
        }
    }
}
