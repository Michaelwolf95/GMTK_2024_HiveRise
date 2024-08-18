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
		public bool isPendingPlacement { get; set; }

		[SerializeField] private CanvasGroup canvasGroup = null;
		[SerializeField] private GameObject pieceContainer = null;
		[SerializeField] private PieceView _linkedPieceView = null;
		public PieceView linkedPieceView => _linkedPieceView;
		
		public CardData pieceCardData { get; private set; }
		
		//-///////////////////////////////////////////////////////////
		/// 
		private void Awake()
		{
			//Init();
		}

		
#region Initialization

		//-///////////////////////////////////////////////////////////
		/// 
		public void SetData(CardData argCardData)
		{
			pieceCardData = argCardData;
			
			if (_linkedPieceView != null)
			{
				Destroy(_linkedPieceView.gameObject);
			}
			_linkedPieceView = Instantiate(CardDefinitions.instance.GetPiecePrefabForID(argCardData.pieceShapeID),pieceContainer.transform);
			linkedPieceView.SetPieceCardData(argCardData);
			
			Init();
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		private void Init()
		{
			SetCardMode(true);
			linkedPieceView.SetPhysical(false);
		}

#endregion //Initialization

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
				HandController.instance.TryStartDraggingCard(this);
			}
		}

		//-///////////////////////////////////////////////////////////
		/// 
		private void OnMouseUp()
		{
			if (isBeingDragged)
			{
				HandController.instance.TryStopDraggingCard(this);
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
				
                CheckForStateChange();
			}
			else
			{
				transform.position = worldPoint;
				transform.rotation = HandController.instance.GetDefaultCardRotation();
				
				CheckForStateChange();
			}
		}

		//-///////////////////////////////////////////////////////////
		/// 
		public void CheckForStateChange()
		{
			if (isCardMode)
			{
				if (HandController.instance.IsPointWithinHandContainer(this.transform.position) == false)
				{
					SetCardMode(false);
				}
			}
			else
			{
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

				isPendingPlacement = false;
			}
			else
			{
				pieceContainer.gameObject.SetActive(true);
				
				// ToDo: Fade this
				canvasGroup.alpha = 0f;
			}
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void OnLinkedPiecePlaced()
		{
			isPendingPlacement = true;
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void OnLinkedPiecePlayed()
		{
			// ToDo: Hook this up!
			isPendingPlacement = false;
		}
		
#endregion //Interaction


#region Debug

		//-///////////////////////////////////////////////////////////
		/// 
		private void OnDrawGizmos()
		{
			if (isBeingDragged)
			{
				//UnityEditor.Handles.Label(transform.position, $"{HandController.instance.IsPointWithinHandContainer(this.transform.position)}");
			}
		}

#endregion //Debug
	}
}