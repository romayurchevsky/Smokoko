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
            ShowLobbyPanelAction += ShowLobbyPanel;
        }

        private void OnDestroy()
        {
            GameManager.LobbyStartAction -= SetParameters;
            DependencyStorage.PlayerStorage.ConcretePlayer.UpdateStats -= SetParameters;
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
        }

        private void SetParameters()
        {
            damageText.text = $"{DependencyStorage.PlayerStorage.ConcretePlayer.HeroStats.DPS}";
        }

        private void ShowLobbyPanel(LobbyPanelType lobbyPanelType)
        {
            lobbyPanels.ForEach(p => p.Panel.gameObject.SetActive(false));
            lobbyPanels.Find(p => p.LobbyPanelType == lobbyPanelType).Panel.gameObject.SetActive(true);
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
