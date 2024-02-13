using Scripts.CommonCode;
using Scripts.PlayerCode;
using System;
using System.Collections;
using UnityEngine;

namespace Scripts.CombatCode
{
    public class WeaponController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] protected HeroWeaponType heroWeaponType;
        [SerializeField] protected Transform container;

        [Header("Parameters")]
        [SerializeField] protected float reloadTime;

        #region get/set
        public bool ReadyToAttack { get; private set; } = true;
        #endregion

        #region Variables
        protected HeroWeapon heroWeapon;
        #endregion

        protected virtual void Awake()
        {
            container.gameObject.SetActive(false);

            CheckWeapon();

            GameManager.LobbyStartAction += CheckWeapon;
            GameManager.LevelStartAction += CheckWeapon;
            GameManager.EquipWeaponAction += CheckWeapon;
        }

        private void OnDestroy()
        {
            GameManager.LobbyStartAction -= CheckWeapon;
            GameManager.LevelStartAction -= CheckWeapon;
            GameManager.EquipWeaponAction -= CheckWeapon;
        }

        public void Equip()
        {
            container.gameObject.SetActive(true);
            DependencyStorage.BaseHeroController.InitWeapon(this);
        }

        public void Hide()
        {
            container.gameObject.SetActive(false);
        }

        public virtual void Attack(ITarget target)
        {
            ReadyToAttack = false;
            StartCoroutine(ReloadProcess());
        }

        private IEnumerator ReloadProcess()
        {
            yield return new WaitForSeconds(reloadTime);
            ReadyToAttack = true;
        }

        private void CheckWeapon()
        {
            heroWeapon = DependencyStorage.PlayerStorage.ConcretePlayer.GetEquipedWeapon();
            if (heroWeapon != null && heroWeapon.HeroWeaponType == heroWeaponType)
            {
                Equip();
            }
            else
            {
                Hide();
            }
        }
    }
}
