using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Scripts.CommonCode;
using Scripts.PlayerCode;

namespace Scripts.UserInterface
{
    public class InventoryPanel : MonoBehaviour
    {
        [Header("Storages")]
        [SerializeField] private GameStorage gameStorage;

        [Header("Components")]
        [SerializeField] private Button lobbysButton;
        [SerializeField] private TMP_Text healthText;
        [SerializeField] private TMP_Text damageText;
        [SerializeField] private List<InventorySlot> inventorySlots = new List<InventorySlot>();

        #region Actions
        public static Action<HeroWeapon, WeaponSettings> ShowManageEquipmentDialogAction;
        public static Action HideManageEquipmentDialogAction;
        #endregion

        private void Awake()
        {
            PrepereButtons();

            GameManager.LobbyStartAction += LevelStartReation;
            DependencyStorage.PlayerStorage.ConcretePlayer.UpdateStats += SetParameters;
        }

        private void OnDestroy()
        {
            GameManager.LobbyStartAction -= LevelStartReation;
            DependencyStorage.PlayerStorage.ConcretePlayer.UpdateStats -= SetParameters;
        }

        private void LevelStartReation()
        {
            PrepereSlots();
            SetParameters();
        }

        private void SetParameters()
        {
            healthText.text = $"{DependencyStorage.PlayerStorage.ConcretePlayer.HeroStats.Health}";
            damageText.text = $"{DependencyStorage.PlayerStorage.ConcretePlayer.HeroStats.Damage}";
        }

        private void PrepereSlots()
        {
            var weaponsTmp = DependencyStorage.PlayerStorage.ConcretePlayer.GameWeapons;

            for (int i = 0; i < inventorySlots.Count; i++)
            {
                var weaponSettings = gameStorage.GameBaseParameters.WeaponSettings.Find(w => w.HeroWeaponType == weaponsTmp[i].HeroWeaponType);
                inventorySlots[i].SetSlot(weaponsTmp[i], weaponSettings);

            }
        }

        private void PrepereButtons()
        {


            lobbysButton.onClick.RemoveAllListeners();
            lobbysButton.onClick.AddListener(() =>
            {
                LobbyPanelController.ShowLobbyPanelAction?.Invoke(LobbyPanelType.Lobby);
            });
        }
    }
}
