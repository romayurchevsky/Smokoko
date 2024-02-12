using UnityEngine;

namespace Scripts.CommonCode
{
    public class AppLoader : MonoBehaviour
    {
        private void Start()
        {
            DependencyStorage.AssetLoader.LoadAssets();
            GameManager.GameStartAction?.Invoke();
        }
    }
}
