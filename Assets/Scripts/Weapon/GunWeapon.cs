using Scripts.CommonCode;
using UnityEngine;

namespace Scripts.CombatCode
{
    public class GunWeapon : WeaponController
    {
        [Header("GunWeapon")]
        [SerializeField] private Transform shootPoint;

        #region Variables
        private int damage;
        #endregion

        protected override void Awake()
        {
            base.Awake();
            damage = DependencyStorage.PlayerStorage.ConcretePlayer.HeroStats.Damage;
        }

        public override void Attack(ITarget target)
        {
            base.Attack(target);
            
            BulletPool.ShotBulletAction?.Invoke(BulletType.PlayerBullet, shootPoint.position, target.TargetTransform.position, damage);
        }
    }
}
