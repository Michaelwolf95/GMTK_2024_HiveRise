using System;
using MichaelWolfGames;
using UnityEngine;

namespace HiveRise
{
	//-///////////////////////////////////////////////////////////
	/// 
	public class CardView : MonoBehaviour
	{
		public bool isBeingDragged { get; private set; }

		[SerializeField] private CanvasGroup canvasGroup = null;
		[SerializeField] private PieceView pieceView = null;
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void OnStartDragging()
		{
			isBeingDragged = true;
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void OnStopDragging()
		{
			isBeingDragged = false;
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

		private void OnMouseUp()
		{
			if (isBeingDragged)
			{
				HandController.instance.StopDraggingCard(this);
			}
		}

		//-///////////////////////////////////////////////////////////
		/// 
		public void UpdateDrag(Vector2 mousePos)
		{
			Vector3 worldPoint = Camera.main.ScreenToWorldPoint(mousePos);
			worldPoint.z = transform.position.z;
			transform.position = worldPoint;
			
			//transform.position = Vector3.MoveTowards(transform.position, worldPoint, )
			
			transform.rotation = Quaternion.RotateTowards(transform.rotation, HandController.instance.GetDefaultCardRotation(), HandController.CARD_ROTATION_SPEED * Time.deltaTime);
			// ToDo: Transition when dragged onto board

			if (HandController.instance.IsPointWithinHandContainer(this.transform.position) == false)
			{
				
			}
		}
		
		
	}
}