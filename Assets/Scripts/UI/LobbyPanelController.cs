using Scripts.CommonCode;
using Scripts.PlayerCode;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UserInterface
{
    public class LobbyPanelController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Button playButton;
        [SerializeField] private Button inventoryButton;
        [SerializeField] private Button addCoinsButton;
        [SerializeField] private Button takeCoinsButton;        
        [SerializeField] private Button upgradeHealthButton;
        [SerializeField] private Button ugradeDamageButton;
        [SerializeField] private TMP_Text damageText;
        [SerializeField] private List<LobbyPanel> lobbyPanels = new List<LobbyPanel>();

        #region Actions
        public static Action<LobbyPanelType> ShowLobbyPanelAction;
        #endregion

        private void Awake()
        {
            PrepereButtons();
            ShowLobbyPanel(LobbyPanelType.Lobby);

            GameManager.LobbyStartAction += SetParameters;
            DependencyStorage.PlayerStorage.ConcretePlayer.UpdateStats += SetParameters;
            DependencyStorage.PlayerStorage.ConcretePlayer.ChangeCoinsCount += UpdateBuyButtons;
            ShowLobbyPanelAction += ShowLobbyPanel;
        }

        private void OnDestroy()
        {
            GameManager.LobbyStartAction -= SetParameters;
            DependencyStorage.PlayerStorage.ConcretePlayer.UpdateStats -= SetParameters;
            DependencyStorage.PlayerStorage.ConcretePlayer.ChangeCoinsCount -= UpdateBuyButtons;
            ShowLobbyPanelAction -= ShowLobbyPanel;
        }

        private void PrepereButtons()
        {
            playButton.onClick.RemoveAllListeners();
            playButton.onClick.AddListener(() =>
            {
                GameManager.LevelPrepereAction?.Invoke();
            });

            inventoryButton.onClick.RemoveAllListeners();
            inventoryButton.onClick.AddListener(() =>
            {
                ShowLobbyPanelAction?.Invoke(LobbyPanelType.Inventory);
            });

            addCoinsButton.onClick.RemoveAllListeners();
            addCoinsButton.onClick.AddListener(() =>
            {
                DependencyStorage.PlayerStorage.ConcretePlayer.AddCoins(100);
            });

            takeCoinsButton.onClick.RemoveAllListeners();
            takeCoinsButton.onClick.AddListener(() =>
            {
                DependencyStorage.PlayerStorage.ConcretePlayer.TakeCoins(100);
            });

            upgradeHealthButton.onClick.RemoveAllListeners();
            upgradeHealthButton.onClick.AddListener(() =>
            {
                DependencyStorage.PlayerStorage.ConcretePlayer.TakeCoins(100);
                DependencyStorage.PlayerStorage.BuyHealthStat(10);
            });

            ugradeDamageButton.onClick.RemoveAllListeners();
            ugradeDamageButton.onClick.AddListener(() =>
            {
                DependencyStorage.PlayerStorage.ConcretePlayer.TakeCoins(100);
                DependencyStorage.PlayerStorage.BuyDamageStat(1);
            });
        }

        private void SetParameters()
        {
            UpdateBuyButtons();
            damageText.text = $"{DependencyStorage.PlayerStorage.ConcretePlayer.HeroStats.DPS}";
        }

        private void ShowLobbyPanel(LobbyPanelType lobbyPanelType)
        {
            lobbyPanels.ForEach(p => p.Panel.gameObject.SetActive(false));
            lobbyPanels.Find(p => p.LobbyPanelType == lobbyPanelType).Panel.gameObject.SetActive(true);
        }

        private void UpdateBuyButtons()
        {
            upgradeHealthButton.interactable = DependencyStorage.PlayerStorage.ConcretePlayer.CurrentCoins >= 100;
            ugradeDamageButton.interactable = DependencyStorage.PlayerStorage.ConcretePlayer.CurrentCoins >= 100;
        }
    }

    [Serializable]
    public class LobbyPanel
    {
        [field: SerializeField] public LobbyPanelType LobbyPanelType { get; private set; }
        [field: SerializeField] public RectTransform Panel { get; private set; }
    }

    public enum LobbyPanelType
    {
        Lobby,
        Inventory
    }
}
