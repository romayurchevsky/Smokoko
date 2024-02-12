using Scripts.CommonCode;
using System.Collections;
using UnityEngine;

namespace Scripts.CombatCode
{
	public class BulletController : MonoBehaviour
	{
		[Header("Parameters")]
		[SerializeField] private float speedFactor = 0.5f;

		[Header("Components")]
		[SerializeField] private Transform bulletBody;
		[SerializeField] private Collider collider;

		#region Variables
		public bool IsBusy { get; private set; } = false;
		private BulletType concreteShotType = BulletType.None;
		private int bulletPower;
		private TargetStorage targetStorage;
		private BulletPool bulletPool;
		#endregion

		private void Awake()
        {
			HideBullet();
		}

		private void OnEnable()
        {
			targetStorage = DependencyStorage.TargetStorage;
			bulletPool = DependencyStorage.BulletPool;
			Init();
		}

		private void OnDisable()
        {
			HideBullet();
		}

        private void OnTriggerEnter(Collider _other)
		{
			switch (concreteShotType)
			{
				case BulletType.PlayerBullet:
					if (targetStorage.IsAliveEnemy(_other.transform))
					{
						targetStorage.AttackEnemy(_other.transform, bulletPower, transform.position);
						HideBullet();
					}
					break;
				default:
					break;
			}
		}

		public void Init()
		{
			bulletPool.RegisterBullet(this);
		}

		public void ShowBullet(BulletType _bulletType, Vector3 _shotPosition, Vector3 _shotDirection, int _power)
		{
			IsBusy = true;
			bulletPower = _power;
			concreteShotType = _bulletType;
			transform.position = _shotPosition;
			transform.forward = _shotDirection - _shotPosition;
			bulletBody.gameObject.SetActive(true);
			collider.enabled = true;
			StartCoroutine(MoveBulletProcess(_shotDirection));
		}

		private IEnumerator MoveBulletProcess(Vector3 _shotDirection)
        {
            while (Vector3.Distance(transform.position, _shotDirection) > 0)
            {
				transform.position = Vector3.Lerp(transform.position, _shotDirection, speedFactor * Time.deltaTime);
				yield return null;
			}

			HideBullet();
		}

		private void HideBullet()
		{
			StopAllCoroutines();
			IsBusy = false;
			bulletBody.gameObject.SetActive(false);
			collider.enabled = false;
		}
	}
}
