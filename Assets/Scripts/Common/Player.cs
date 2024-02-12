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

		public void UpdateHeroStats(int _health, int _damage, int _dps)
		{
			HeroStats.UpdateStats(_health, _damage, _dps);
			UpdateStats?.Invoke();
	}
	}

	[Serializable]
	public class HeroStats
    {
		[field: SerializeField] public int Health { get; private set; } 
		[field: SerializeField] public int Damage { get; private set; } 
		[field: SerializeField] public int DPS { get; private set; }

		public void UpdateStats(int _health, int _damage, int _dps)
        {
			Health = _health;
			Damage = _damage;
			DPS = _dps;
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
		Weapon_01
    }
}
