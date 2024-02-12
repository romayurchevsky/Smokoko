using System;
using UnityEngine;

namespace Scripts.PlayerCode
{
	public class HeroAnimationController : MonoBehaviour
	{
		[Header("Animator")]
		[SerializeField] private Animator playerAnimator;

		#region Variables
		private readonly int speedKey = Animator.StringToHash("Speed");
		private readonly int finishOperationKey = Animator.StringToHash("FinishOperation");
		private readonly int playerDieKey = Animator.StringToHash("Die");
		private readonly int attackKey = Animator.StringToHash("Attack");
		private readonly int toRunKey = Animator.StringToHash("ToRun");
		private readonly int attackVariantKey = Animator.StringToHash("AttackVariant");
		#endregion

		#region Actions
		public Action EndAttackAction;
		#endregion

		public void SetSpeed(float _speed)
		{
			playerAnimator.SetFloat(speedKey, _speed, 0.1f, Time.deltaTime);
		}

		public void StopMove()
		{
			playerAnimator.SetFloat(speedKey, 0f);
		}

		public void ClearReset()
		{
			playerAnimator.ResetTrigger(finishOperationKey);
		}

		public void Attack(int _attackVariant)
		{
			playerAnimator.SetInteger(attackVariantKey, _attackVariant);
			playerAnimator.SetTrigger(attackKey);
		}

		public void FinishAttack()
		{
			playerAnimator.ResetTrigger(attackKey);
			playerAnimator.SetTrigger(toRunKey);
		}

		public void EndAttack()
		{
			EndAttackAction?.Invoke();
		}

		public void Die()
        {
			playerAnimator.SetTrigger(playerDieKey);
		}
	}
}
