using Scripts.CommonCode;
using Scripts.PlayerCode;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UserInterface
{
    public class ManageEquipmentDialog : MonoBehaviour
    {
        [Header("Storages")]
        [SerializeField] private GameStorage gameStorage;

        [Header("Components")]
        [SerializeField] private Transform container;
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text iconLevelText;
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text damageText;
        [SerializeField] private TMP_Text coastText;
        [SerializeField] private Button upgradeButton;
        [SerializeField] private Button equipButton;
        [SerializeField] private TMP_Text equipButtonText;
        [SerializeField] private Button resetButton;
        [SerializeField] private Button closeButton;

        #region Variables
        private HeroWeapon heroWeapon;
        private WeaponSettings weaponSettingsn;
        private WeaponLevelSettings weaponLevelSettingsn;
        #endregion

        private void Awake()
        {
            Hide();
            PrepereButtons();
        }

        private void OnEnable()
        {
            InventoryPanel.ShowManageEquipmentDialogAction += Show;
            InventoryPanel.HideManageEquipmentDialogAction += Hide;
            DependencyStorage.PlayerStorage.ConcretePlayer.ChangeCoinsCount += UpdateInfo;
        }

        private void OnDisable()
        {
            InventoryPanel.ShowManageEquipmentDialogAction -= Show;
            InventoryPanel.HideManageEquipmentDialogAction -= Hide;
            DependencyStorage.PlayerStorage.ConcretePlayer.ChangeCoinsCount -= UpdateInfo;
        }

        private void Show(HeroWeapon _heroWeapon, WeaponSettings _weaponSettingsn)
        {
            heroWeapon = _heroWeapon;
            weaponSettingsn = _weaponSettingsn;
            UpdateInfo();
            container.gameObject.SetActive(true);
        }

        private void UpdateInfo()
        {
            weaponLevelSettingsn = weaponSettingsn.WeaponLevelSettings.Find(w => w.Level == heroWeapon.Level);
            icon.sprite = weaponSettingsn.Icon;
            iconLevelText.text = $"{heroWeapon.Level}";
            levelText.text = $"Level: {heroWeapon.Level} / {weaponSettingsn.MaxLevel}";
            damageText.text = $"{weaponLevelSettingsn.Damage}";
            coastText.text = $"{weaponLevelSettingsn.Coast}";
            nameText.text = $"{weaponSettingsn.WeaponName}";

            upgradeButton.interactable = heroWeapon.Level < weaponSettingsn.MaxLevel && DependencyStorage.PlayerStorage.ConcretePlayer.CurrentCoins >= weaponLevelSettingsn.Coast;
            equipButtonText.text = heroWeapon.Equiped ? "take off" : "equipe";
        }

        private void PrepereButtons()
        {
            upgradeButton.onClick.RemoveAllListeners();
            upgradeButton.onClick.AddListener(() =>
            {
                UpgradeButtonReaction();
            });

            equipButton.onClick.RemoveAllListeners();
            equipButton.onClick.AddListener(() =>
            {
                EquipButtonReaction();
            });

            resetButton.onClick.RemoveAllListeners();
            resetButton.onClick.AddListener(() =>
            {
                ResetButtonReaction();
            });

            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(() =>
            {
                Hide();
            });
        }

        private void UpgradeButtonReaction()
        {
            heroWeapon.UpgradeWeapon();
            DependencyStorage.PlayerStorage.ConcretePlayer.TakeCoins(weaponLevelSettingsn.Coast);
            DependencyStorage.PlayerStorage.UpdateStats(weaponLevelSettingsn);
            UpdateInfo();
        }

        private void EquipButtonReaction()
        {
            if (heroWeapon.Equiped)
            {
                DependencyStorage.PlayerStorage.TakeOffWeapon(heroWeapon);
                DependencyStorage.PlayerStorage.TakeStats(weaponLevelSettingsn);
            }
            else
            {
                CheckEquiped();
                DependencyStorage.PlayerStorage.EquipWeapon(heroWeapon);
                DependencyStorage.PlayerStorage.UpdateStats(weaponLevelSettingsn);
            }
            UpdateInfo();
            GameManager.EquipWeaponAction?.Invoke();
        }

        private void CheckEquiped()
        {
            var equiped = DependencyStorage.PlayerStorage.ConcretePlayer.GetEquipedWeapon();
            if (equiped != null)
            {
                var tmp = gameStorage.GameBaseParameters.WeaponSettings.Find(w => w.HeroWeaponType == equiped.HeroWeaponType).WeaponLevelSettings.Find(l => l.Level == equiped.Level);
                DependencyStorage.PlayerStorage.TakeStats(tmp);
            }
        }

        private void ResetButtonReaction()
        {
            heroWeapon.ResetUpgrades();
            UpdateInfo();
        }

        private void Hide()
        {
            container.gameObject.SetActive(false);
        }
    }
}
