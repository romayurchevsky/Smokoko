using UnityEngine;

namespace Scripts.CommonCode
{
    public class AppLoader : MonoBehaviour
    {
        private void Start()
        {
            GameManager.GameStartAction?.Invoke();
        }
    }
}
