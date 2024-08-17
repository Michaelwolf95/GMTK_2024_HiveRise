using System;
using UnityEngine;

namespace HiveRise
{
	//-///////////////////////////////////////////////////////////
	/// 
	public class CardView : MonoBehaviour
	{
		private bool isBeingDragged = false;
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void OnStartDragging()
		{
			
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void OnStopDragging()
		{
			
		}

		//-///////////////////////////////////////////////////////////
		/// 
		private void OnMouseDown()
		{
			if (HandController.instance.currentDragCard == null)
			{
				HandController.instance.StartDraggingCard(this);
			}
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		private void UpdateDrag(Vector2 mousePos)
		{
			
		}
	}
}