using MichaelWolfGames;
using UnityEngine;

namespace HiveRise
{
	//-///////////////////////////////////////////////////////////
	/// 
	public class CameraRigController : SceneSingleton<CameraRigController>
	{
		[SerializeField] private Camera _mainCamera = null;
		public Camera mainCamera => _mainCamera;
		
		[SerializeField] private Transform _mainCameraContainer = null;
		[SerializeField] private float topOffset = 5f;
		[SerializeField] private float minHeight = 5f;
		[SerializeField] private float moveSpeed = 5f;
		
		private float currentHeight;
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void SetCurrentHeight(float argCurrentHeight)
		{
			currentHeight = argCurrentHeight;
		}

		//-///////////////////////////////////////////////////////////
		/// 
		private void Update()
		{
			float currentPositionHeight = _mainCameraContainer.localPosition.y;

			float targetHeight = Mathf.Max(minHeight, currentHeight + topOffset);
			currentPositionHeight = Mathf.MoveTowards(currentPositionHeight, targetHeight, moveSpeed * Time.deltaTime);

			_mainCameraContainer.localPosition = new Vector3(0f, currentPositionHeight, 0f);
			
		}
	}
}