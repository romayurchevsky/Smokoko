using UnityEngine;

namespace Scripts.CombatCode
{
	public interface ITarget
	{
		public Transform TargetTransform { get; }
		public bool IsAlive { get; }
		public void GetDamage(int _damage, Vector3 _from);
		public void SelectTarget(bool _select);
	}
}
