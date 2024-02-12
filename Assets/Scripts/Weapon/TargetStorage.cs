using Scripts.CommonCode;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.CombatCode
{
	public class TargetStorage : MonoBehaviour
	{
		#region Variables
		private Dictionary<Transform, ITarget> enemies = new Dictionary<Transform, ITarget>();
		#endregion

		private void Awake()
		{
			DependencyStorage.TargetStorage = this;
			enemies.Clear();
		}

		public void RegisterEnemy(ITarget _target)
		{
			if (!enemies.ContainsKey(_target.TargetTransform))
			{
				enemies.Add(_target.TargetTransform, _target);
			}
		}

		public void UnregisterEnemy(ITarget _target)
		{
			if (enemies.ContainsKey(_target.TargetTransform))
			{
				enemies.Remove(_target.TargetTransform);
			}
		}

		public ITarget GetNearestEnemy(Vector3 _position, float _distance = float.PositiveInfinity)
		{
			ITarget tmpEnemy = null;
			float minDistance = float.PositiveInfinity;
			foreach (var kvp in enemies.ToArray())
			{
				var tmpDistance = Vector3.Distance(_position, kvp.Key.position);
				if (kvp.Value != null && tmpDistance <= _distance && tmpDistance < minDistance)
				{
					minDistance = tmpDistance;
					tmpEnemy = kvp.Value;
				}
			}
			return tmpEnemy;
		}

		public ITarget GetEnemy(Transform _enemyTransform)
		{
			return enemies[_enemyTransform];
		}

		public bool IsAliveEnemy(Transform _transform)
		{
			var tmp = enemies;
			return enemies.ContainsKey(_transform) && enemies[_transform].IsAlive;
		}

		public void AttackEnemy(Transform _transform, int _power, Vector3 _from)
		{
			if (enemies.ContainsKey(_transform))
			{
				enemies[_transform].GetDamage(_power, _from);
			}
		}
	}
}
