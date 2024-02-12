using Scripts.CommonCode;
using UnityEngine;

namespace Scripts.PlayerCode
{
	public class PlayerStorage : MonoBehaviour
	{
		[SerializeField] private GameStorage gameStorage; 

		[Header("Basic")]
		[SerializeField] private string playerPrefsSaveString = "_playerSave";

		[Header("ConcretePlayer")]
		[SerializeField] private Player concretePlayer = new Player();

		#region Geters/Seters
		public Player ConcretePlayer { get => concretePlayer; }
        public bool IsPlayerIsLoaded { get; private set; }
        #endregion

        private void Awake()
        {
			DependencyStorage.PlayerStorage = this;
        }

        public void SavePlayer()
		{
			if (IsPlayerIsLoaded)
			{
                PlayerPrefs.SetString(playerPrefsSaveString, JsonUtility.ToJson(concretePlayer));
                Debug.Log($"player-saved");
            }
		}

		public void LoadPlayer()
		{
			var playerString = PlayerPrefs.GetString(playerPrefsSaveString, "");
			if (playerString != "")
			{
				concretePlayer = JsonUtility.FromJson<Player>(playerString);
			}
			else
			{
				concretePlayer = new Player();
				StartingSettings();
			}

			IsPlayerIsLoaded = true;
            GameManager.PlayerLoadedAction?.Invoke();
		}

		private void StartingSettings()
        {
			SetStartWeapons();
			SetStartingStats();
		}

		private void SetStartWeapons()
        {
			var weaponsTmp = gameStorage.GameBaseParameters.StartingWeapons;
			for (int i = 0; i < weaponsTmp.Count; i++)
            {
				concretePlayer.GameWeapons.Add(new HeroWeapon(weaponsTmp[i]));
			}
		}

		private void SetStartingStats()
        {
			var tmp = gameStorage.GameBaseParameters.Stats;
			concretePlayer.HeroStats.UpdateStats(tmp.BaseHealth, tmp.BaseDamage, tmp.BaseDPS);
		}

		public void UpdateStats(WeaponLevelSettings weaponLevelSettings)
		{
			var tmp = gameStorage.GameBaseParameters.Stats;
			concretePlayer.UpdateHeroStats(tmp.BaseHealth + weaponLevelSettings.Health, tmp.BaseDamage + weaponLevelSettings.Damage, tmp.BaseDPS + (weaponLevelSettings.Damage * weaponLevelSettings.AttackSpped));
		}

		public void ResetStats(WeaponLevelSettings weaponLevelSettings)
		{
			var tmp = gameStorage.GameBaseParameters.Stats;
			concretePlayer.UpdateHeroStats(tmp.BaseHealth, tmp.BaseDamage, tmp.BaseDPS);
		}
	}
}
