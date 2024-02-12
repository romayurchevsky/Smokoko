using Scripts.CommonCode;
using Scripts.PlayerCode;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UserInterface
{
    public class InventorySlot: MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Image icon;
        [SerializeField] private Button button;

        #region Variables
        private HeroWeapon heroWeapon;
        private WeaponSettings weaponSettingsn;
        #endregion

        #region get/set
        public bool IsFree { get; private set; } = true;
        #endregion

        private void Awake()
        {
            PrepereButtons();
        }

        private void PrepereButtons()
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                InventoryPanel.ShowManageEquipmentDialogAction?.Invoke(heroWeapon, weaponSettingsn);
            });
        }

        public void SetSlot(HeroWeapon _heroWeapon, WeaponSettings _weaponSettingsn)
        {
            if (_heroWeapon != null)
            {
                IsFree = false;
                heroWeapon = _heroWeapon;
                weaponSettingsn = _weaponSettingsn;
                icon.sprite = _weaponSettingsn.Icon;
            }
        }
    }
}
