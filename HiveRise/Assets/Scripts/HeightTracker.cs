using System;
using TMPro;
using UnityEngine;

namespace HiveRise
{
	//-///////////////////////////////////////////////////////////
	/// 
	public class HeightTracker : MonoBehaviour
	{
		[SerializeField] private Transform heightBarRoot = null;
		[SerializeField] private TextMeshPro currentHeightLabel = null;
		[SerializeField] private TextMeshPro targetHeightLabel = null;
		[SerializeField] private float moveSpeed = 10f;

		private float targetHeight;
		private float currentHeight = -1f;
		
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void SetTargetHeight(float argTargetHeight)
		{
			targetHeight = argTargetHeight;
			targetHeightLabel.text = ((int)targetHeight).ToString();
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
			heightBarRoot.transform.localPosition = new Vector3(0f, currentHeight, 0f);
		}

		//-///////////////////////////////////////////////////////////
		/// 
		private void Update()
		{
			if (currentHeight <= 0f)
			{
				return;
			}
			
			float currentPositionHeight = heightBarRoot.localPosition.y;

			currentPositionHeight = Mathf.MoveTowards(currentPositionHeight, currentHeight, moveSpeed * Time.deltaTime);

			heightBarRoot.transform.localPosition = new Vector3(0f, currentPositionHeight, 0f);
			currentHeightLabel.text = (currentPositionHeight).ToString("F1");
			
		}
	}
}