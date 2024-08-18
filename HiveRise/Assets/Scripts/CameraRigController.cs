using System;
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
		[Space]
		[SerializeField] private float mainMenuHeight = 75f;
		
		private float currentHeight;

		//-///////////////////////////////////////////////////////////
		/// 
		public void Start()
		{
			
		}
		
		//-///////////////////////////////////////////////////////////
		///
		public void JumpToMainMenuHeight()
		{
			SetToMainMenuHeight();
			JumpToCurrentHeight();
		}

		//-///////////////////////////////////////////////////////////
		/// 
		public void SetToMainMenuHeight()
		{
			SetCurrentHeight(mainMenuHeight - topOffset);
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void SetCurrentHeight(float argCurrentHeight)
		{
			currentHeight = argCurrentHeight;
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void JumpToCurrentHeight()
		{
			_mainCameraContainer.localPosition = new Vector3(0f, GetTargetHeight(), 0f);
		}

		//-///////////////////////////////////////////////////////////
		/// 
		private void Update()
		{
			float currentPositionHeight = _mainCameraContainer.localPosition.y;

			float targetHeight = GetTargetHeight();
			currentPositionHeight = Mathf.MoveTowards(currentPositionHeight, targetHeight, GetMoveSpeed() * Time.deltaTime);

			_mainCameraContainer.localPosition = new Vector3(0f, currentPositionHeight, 0f);
		}

		//-///////////////////////////////////////////////////////////
		/// 
		private float GetTargetHeight()
		{
			return Mathf.Max(minHeight, currentHeight + topOffset);
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		private float GetMoveSpeed()
		{
			return moveSpeed;
		}
	}
}