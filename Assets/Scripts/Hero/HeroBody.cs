using Scripts.CombatCode;
using Scripts.CommonCode;
using UnityEngine;

namespace Scripts.PlayerCode
{
    public class HeroBody : MonoBehaviour
    {
        [Header("Storages")]
        [SerializeField] private ResourcesStorage resourcesStorage;

        [Header("Components")]
        [SerializeField] private Transform weaponPoint;
        [field: SerializeField] public HeroAnimationController HeroAnimatorController { get; private set; }

        #region Variables
        protected WeaponController weaponController;
        #endregion

        private void Awake()
        {
            DependencyStorage.BaseHeroController.InitBody(this);
            LoadWeapon();
        }

        private void LoadWeapon()
        {
            Instantiate(DependencyStorage.AssetLoader.GetAssetHandle(AssetType.WeaponAsset).Result, weaponPoint);
        }

        public void InitWeapon(WeaponController _weaponController)
        {
            weaponController = _weaponController;
        }

        public bool HasWeapon()
        {
            return DependencyStorage.PlayerStorage.ConcretePlayer.GetEquipedWeapon() != null;
        }

        public bool ReadyToAttack()
        {
            return weaponController.ReadyToAttack;
        }

        public void Attack(ITarget _target)
        {
            weaponController.Attack(_target);
        }
    }
}
