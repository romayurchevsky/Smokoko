using Scripts.PlayerCode;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.CommonCode
{
	[Serializable]
	public class GameBaseParameters
	{
		[field: SerializeField] public Stats Stats { get; private set; } 
		[field: SerializeField] public List<WeaponSettings> WeaponSettings { get; private set; } = new List<WeaponSettings>();
		[field: SerializeField] public List<HeroWeaponType> StartingWeapons { get; private set; } = new List<HeroWeaponType>();
	}

	[Serializable]
	public class Stats
	{
		[field: SerializeField] public int BaseHealth { get; private set; }
		[field: SerializeField] public int BaseDamage { get; private set; }
		[field: SerializeField] public float BaseAttackSpeed { get; private set; }
	}

	[Serializable]
	public class WeaponSettings
    {
		[field: SerializeField] public HeroWeaponType HeroWeaponType { get; private set; } = HeroWeaponType.None;
		[field: SerializeField] public Sprite Icon { get; private set; }
		[field: SerializeField] public string WeaponName { get; private set; }
		[field: SerializeField] public int MaxLevel { get; private set; }
		[field: SerializeField] public List<WeaponLevelSettings> WeaponLevelSettings { get; private set; } = new List<WeaponLevelSettings>();
	}

	[Serializable]
	public class WeaponLevelSettings
	{
		[field: SerializeField] public int Level { get; private set; }
		[field: SerializeField] public int Damage { get; private set; }
		[field: SerializeField] public int Health { get; private set; }
		[field: SerializeField] public float AttackSpped { get; private set; }
		[field: SerializeField] public int Coast { get; private set; }
	}
}
