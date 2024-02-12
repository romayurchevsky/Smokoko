using Scripts.CommonCode;
using Scripts.PlayerCode;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UserInterface
{
	public class PreloaderPanelController : MonoBehaviour
	{
		[Header("Parameters")]
		[SerializeField] private float loadingSpeed;

		[Header("Components")]
		[SerializeField] private Slider loadingSlider;

		private void OnEnable()
		{
			GameManager.GameStartAction += GameStartReaction;
		}

        private void OnDisable()
        {
			GameManager.GameStartAction -= GameStartReaction;
		}

		private void GameStartReaction()
        {
			StartCoroutine(LoadingProcess());
		}

		private IEnumerator LoadingProcess()
        {
			loadingSlider.value = 0f;
			while (loadingSlider.value < loadingSlider.maxValue)
            {
				loadingSlider.value += loadingSpeed * Time.deltaTime;
				yield return null;
			}

			GameManager.LobbyPrepereAction?.Invoke();
		}
	}
}
