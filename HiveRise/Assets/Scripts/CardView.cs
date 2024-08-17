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
		public bool isCardMode { get; private set; }

		[SerializeField] private CanvasGroup canvasGroup = null;
		[SerializeField] private GameObject pieceContainer = null;
		[SerializeField] private PieceView linkedPieceView = null;
		
		//-///////////////////////////////////////////////////////////
		/// 
		private void Awake()
		{
			Init();
		}

		//-///////////////////////////////////////////////////////////
		/// 
		private void Init()
		{
			SetCardMode(true);
			linkedPieceView.SetPhysical(false);
		}

#region Interaction

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
			Vector3 worldPoint = CameraRigController.instance.mainCamera.ScreenToWorldPoint(mousePos);
            worldPoint.z = transform.position.z;
			if (isCardMode)
			{
                transform.position = worldPoint;
                
                transform.rotation = Quaternion.RotateTowards(transform.rotation, HandController.instance.GetDefaultCardRotation(), HandController.CARD_ROTATION_SPEED * Time.deltaTime);
                // ToDo: Transition when dragged onto board
                
                if (HandController.instance.IsPointWithinHandContainer(this.transform.position) == false)
                {
	                SetCardMode(false);
                }
			}
			else
			{
				transform.position = worldPoint;
				transform.rotation = HandController.instance.GetDefaultCardRotation();
				
				if (HandController.instance.IsPointWithinHandContainer(this.transform.position))
				{
					SetCardMode(true);
				}
			}
			
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		private void SetCardMode(bool argCardMode)
		{
			isCardMode = argCardMode;

			if (isCardMode)
			{
				pieceContainer.gameObject.SetActive(false);
				
				// ToDo: Fade this
				canvasGroup.alpha = 1f;
			}
			else
			{
				pieceContainer.gameObject.SetActive(true);
				
				// ToDo: Fade this
				canvasGroup.alpha = 0f;
			}
		}
		
#endregion //Interaction


#region Debug

		//-///////////////////////////////////////////////////////////
		/// 
		private void OnDrawGizmos()
		{
			if (isBeingDragged)
			{
				UnityEditor.Handles.Label(transform.position, $"{HandController.instance.IsPointWithinHandContainer(this.transform.position)}");
			}
		}

#endregion //Debug
	}
}