using Scripts.CommonCode;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Scripts.CombatCode
{
	public class BulletPool : MonoBehaviour
	{
		[Header("Storages")]
		[SerializeField] private ResourcesStorage resourcesStorage;

		[Header("Bullets")] 
		[SerializeField] private int startBulletCount = 100;

		[Header("Components")]
		[SerializeField] private Transform poolContainer;

		#region Actions
		public static Action<BulletType, Vector3, Vector3, int> ShotBulletAction;
		#endregion

		#region Variables
		private List<BulletController> bullets = new List<BulletController>();
		private AsyncOperationHandle<GameObject> bulletControllerHandle;
		#endregion

		private void Awake()
		{
			DependencyStorage.BulletPool = this;
			PreparePool();
		}

		private void OnEnable()
		{
			ShotBulletAction += Shot;
		}

		private void OnDisable()
		{
			ShotBulletAction -= Shot;
		}

		private void PreparePool()
		{
			for (int i = 0; i < startBulletCount; i++)
			{
				AddBullet();
			}
		}

		public void RegisterBullet(BulletController _bulletController)
        {
			bullets.Add(_bulletController);
		}

		private void AddBullet()
		{
			Instantiate(DependencyStorage.AssetLoader.GetAssetHandle(AssetType.BulletAsset).Result, poolContainer);
		}

		private void Shot(BulletType _bulletType, Vector3 _shotPosition, Vector3 _shotDirection, int _power)
		{
			GetFreeBullet().ShowBullet(_bulletType, _shotPosition, _shotDirection, _power);
		}

		private BulletController GetFreeBullet()
		{
			var someBullet = bullets.Find(_someBullet => !_someBullet.IsBusy);
			if (someBullet != null)
			{
				return someBullet;
			}

			AddBullet();
			return GetFreeBullet();
		}
	}
}
