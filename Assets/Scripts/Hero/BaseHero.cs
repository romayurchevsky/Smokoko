using Scripts.CombatCode;
using Scripts.CommonCode;
using UnityEngine;

namespace Scripts.PlayerCode
{
    public class BaseHero : MonoBehaviour
    {
        [Header("Storages")]
        [SerializeField] protected ResourcesStorage resourcesStorage;

        #region Variables
        protected HeroBody heroBody;
        #endregion

        protected virtual void Awake()
        {
            DependencyStorage.BaseHeroController = this;
            LoadBody();
        }

        protected void LoadBody()
        {
            Instantiate(DependencyStorage.AssetLoader.GetAssetHandle(AssetType.HeroBodyAsset).Result, transform);
        }

        public virtual void InitBody(HeroBody _heroBody)
        {
            heroBody = _heroBody;
        }

        public virtual void InitWeapon(WeaponController _weapon)
        {
            heroBody.InitWeapon(_weapon);
        }
    }
}
