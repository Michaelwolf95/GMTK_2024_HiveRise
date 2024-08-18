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
		[SerializeField] private float moveFromMainMenuSpeed = 50f;
		
		private float currentHeight;
		//private bool isMovingBetweenMainMenu = false;
		private float camVelocity = 0f;

		private Action onReachedHeight = null;

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
			//isMovingBetweenMainMenu = true;
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void SetCurrentHeight(float argCurrentHeight, Action argOnReachedHeight = null)
		{
			currentHeight = argCurrentHeight;

			onReachedHeight = argOnReachedHeight;
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
			float speed = moveSpeed;
			float currentPositionHeight = _mainCameraContainer.localPosition.y;
			float targetHeight = GetTargetHeight();

			if (Mathf.Abs(currentPositionHeight - targetHeight) < 0.05f)
			{
				if (onReachedHeight != null)
				{
					onReachedHeight.InvokeNullCheck();
					onReachedHeight = null;
				}
				return;
			}
			
			if (currentPositionHeight > targetHeight)
			{
				speed = moveFromMainMenuSpeed;
			}

			currentPositionHeight = Mathf.SmoothDamp(currentPositionHeight, targetHeight, ref camVelocity, 1f, speed);
			//currentPositionHeight = Mathf.MoveTowards(currentPositionHeight, targetHeight, speed * Time.deltaTime);

			_mainCameraContainer.localPosition = new Vector3(0f, currentPositionHeight, 0f);
		}

		//-///////////////////////////////////////////////////////////
		/// 
		private float GetTargetHeight()
		{
			return Mathf.Max(minHeight, currentHeight + topOffset);
		}
		
	}
}