using System;
using System.Collections;
using Cinemachine;
using Scripts.CommonCode;
using UnityEngine;

namespace Scripts.CaneraCode
{
	public class CameraController : MonoBehaviour
	{
		[Header("Main Camera")]
		[SerializeField] private Camera mainCamera;

		[Header("Virtual Cameras")]
		[SerializeField] private CinemachineVirtualCamera playerCamera;
		[SerializeField] private float shakeDuration;

		#region get/set
		public Camera GetCamera => mainCamera;
		#endregion

		#region Actions
		public static Action<Transform> SetFollowAction;
		public static Action ShakeCameraAction;
		#endregion

		#region Variables
		private CinemachineBasicMultiChannelPerlin shakeChanel;
		#endregion

		private void Awake()
        {
			DependencyStorage.CameraController = this;
			shakeChanel = playerCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
		}

        private void OnEnable()
		{
			SetFollowAction += SetFollow;
			ShakeCameraAction += ShakeCamera;
		}

		private void OnDisable()
		{
			SetFollowAction -= SetFollow;
			ShakeCameraAction -= ShakeCamera;
		}

		private void SetFollow(Transform _transform)
		{
			playerCamera.Follow = _transform;
			playerCamera.LookAt = _transform;
		}

		private void ShakeCamera()
        {
			StopAllCoroutines();
			StartCoroutine(ShakeProcess());
        }

		private IEnumerator ShakeProcess()
        {
			shakeChanel.m_AmplitudeGain = 1;
			yield return new WaitForSeconds(shakeDuration);
			shakeChanel.m_AmplitudeGain = 0;
		}
	}
}