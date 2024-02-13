using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.PlayerCode
{
    [Serializable]
	public class Player
	{
		[field: SerializeField] public int CurrentLevel { get; private set; } = 0;
		[field: SerializeField] public int CurrentCoins { get; private set; } = 0;
		[field: SerializeField] public HeroStats HeroStats { get; private set; } = new HeroStats();
		[field: SerializeField] public List<HeroWeapon> GameWeapons { get; private set; } = new List<HeroWeapon>();

		public Action ChangeCoinsCount;
		public Action EquipWeapon;
		public Action UpdateStats;

		public void AddCoins(int _count)
        {
			CurrentCoins += _count;
			ChangeCoinsCount?.Invoke();
		}

		public void TakeCoins(int _count)
		{
			if (CurrentCoins < _count) return;
			CurrentCoins -= _count;
			ChangeCoinsCount?.Invoke();
		}

		public HeroWeapon GetEquipedWeapon()
        {
			return GameWeapons.Find(w => w.Equiped);
		}

		public void UpdateHeroStats(int _health, int _damage, float _attackSpeed)
		{
			HeroStats.UpdateStats(_health, _damage, _attackSpeed);
			UpdateStats?.Invoke();
		}
		public void TakeeHeroStats(int _health, int _damage, float _attackSpeed)
		{
			HeroStats.TakeStats(_health, _damage, _attackSpeed);
			UpdateStats?.Invoke();
		}

		public void BuyHealthStat(int _health)
		{
			HeroStats.BuyHealthStat(_health);
			UpdateStats?.Invoke();
		}

		public void BuyDamageStat(int _damage)
		{
			HeroStats.BuyDamageStat(_damage);
			UpdateStats?.Invoke();
		}
	}

	[Serializable]
	public class HeroStats
    {
		[field: SerializeField] public int Health { get; private set; } 
		[field: SerializeField] public int Damage { get; private set; } 		
		[field: SerializeField] public float AttackSpeed { get; private set; } 
		[field: SerializeField] public float DPS { get; private set; }

		public void UpdateStats(int _health, int _damage, float _attackSpeed)
        {
			Health += _health;
			Damage += _damage;
			AttackSpeed = _attackSpeed;
			DPS = Mathf.Round((Damage / AttackSpeed) * 10f) * 0.1f;
		}

		public void TakeStats(int _health, int _damage, float _attackSpeed)
		{
			Health -= _health;
			Damage -= _damage;
			AttackSpeed -= _attackSpeed;
			DPS = Mathf.Round((Damage / AttackSpeed) * 10f) * 0.1f;
		}

		public void BuyHealthStat(int _health)
		{
			Health += _health;
		}

		public void BuyDamageStat(int _damage)
		{
			Damage += _damage;
		}
	}

	[Serializable] 
	public class HeroWeapon
    {
		[field: SerializeField] public int Level { get; private set; } = 1;
		[field: SerializeField] public bool Equiped { get; private set; } = false;
		[field: SerializeField] public HeroWeaponType HeroWeaponType { get; private set; } = HeroWeaponType.None;

		public HeroWeapon(HeroWeaponType _heroWeaponType)
        {
			HeroWeaponType = _heroWeaponType;
		}

		public void UpgradeWeapon()
        {
			Level++;
		}

		public void ResetUpgrades()
        {
			Level = 1;
		}

		public void Equip()
        {
			Equiped = true;
		}

		public void TakeOFf()
		{
			Equiped = false;
		}
	}

	public enum HeroWeaponType
    {
		None,
		Weapon_01,
		Weapon_02,
		Weapon_03,
    }
}
