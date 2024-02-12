using Scripts.CommonCode;
using Scripts.PlayerCode;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UserInterface
{
	public class GamePanelController : MonoBehaviour
	{
        [Header("Storages")]
        [SerializeField] private GameStorage gameStorage;

		[Header("Components")]
		[SerializeField] private Button lobbyButton;
		[SerializeField] private TMP_Text healthText;
		[SerializeField] private TMP_Text damageText;
		[SerializeField] private TMP_Text dapsText;

        private void Awake()
        {
            PrepereButtons();
            GameManager.LevelStartAction += SetParameters;
        }

        private void OnDestroy()
        {
            GameManager.LevelStartAction -= SetParameters;
        }

        private void SetParameters()
        {
            healthText.text = $"{DependencyStorage.PlayerStorage.ConcretePlayer.HeroStats.Health}";
            damageText.text = $"{DependencyStorage.PlayerStorage.ConcretePlayer.HeroStats.Damage}";
            dapsText.text = $"{DependencyStorage.PlayerStorage.ConcretePlayer.HeroStats.DPS}";
        }

        private void PrepereButtons()
        {
            lobbyButton.onClick.RemoveAllListeners();
            lobbyButton.onClick.AddListener(() =>
            {
                GameManager.LobbyPrepereAction?.Invoke();
            });
        }
    }
}
