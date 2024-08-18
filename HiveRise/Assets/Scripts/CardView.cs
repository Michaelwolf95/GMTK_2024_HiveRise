using System;
using MichaelWolfGames;
using UnityEngine;
using UnityEngine.UI;

namespace HiveRise
{
	//-///////////////////////////////////////////////////////////
	/// 
	public class CardView : MonoBehaviour
	{
		public bool isBeingDragged { get; private set; }
		public bool isCardMode { get; private set; }
		public bool isPendingPlacement { get; set; }

		[SerializeField] private CanvasGroup cardCanvasGroup = null;
		[SerializeField] private GameObject pieceContainer = null;
		[Space]
		[SerializeField] private CanvasGroup rotationGizmoCanvasGroup = null;
		[SerializeField] private Button leftRotationButton = null; 
		[SerializeField] private Button rightRotationButton = null; 
		
		public PieceView linkedPieceView { get; private set; }
		public CardData pieceCardData { get; private set; }
		
		private Vector2 clickStartPos = Vector2.zero;
		
		//-///////////////////////////////////////////////////////////
		/// 
		private void Awake()
		{
			leftRotationButton.onClick.AddListener(RotatePieceLeft);
			rightRotationButton.onClick.AddListener(RotatePieceRight);
		}

		
#region Initialization

		//-///////////////////////////////////////////////////////////
		/// 
		public void SetData(CardData argCardData)
		{
			pieceCardData = argCardData;
			
			if (linkedPieceView != null)
			{
				Destroy(linkedPieceView.gameObject);
			}
			linkedPieceView = Instantiate(CardDefinitions.instance.GetPiecePrefabForID(argCardData.pieceShapeID),pieceContainer.transform);
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
			SetRotationGizmoShown(false);
			
			linkedPieceView.SetAllCollidersEnabled(false);
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
			// if (IsShowingRotationGizmo())
			// {
			// 	return;
			// }
			clickStartPos = Input.mousePosition;
		}

		//-///////////////////////////////////////////////////////////
		/// 
		private void OnMouseDrag()
		{
			// if (IsShowingRotationGizmo())
			// {
			// 	return;
			// }
			if (isBeingDragged == false && Vector2.Distance(Input.mousePosition, clickStartPos) > 20)
			{
				if (HandController.instance.currentDragCard == null)
				{
					HandController.instance.TryStartDraggingCard(this);
				}
			}
		}

		//-///////////////////////////////////////////////////////////
		/// 
		private void OnMouseUp()
		{
			// if (IsShowingRotationGizmo())
			// {
			// 	return;
			// }
			if (isBeingDragged)
			{
				HandController.instance.TryStopDraggingCard(this);
			}
			else if (isPendingPlacement)
			{
				if (IsShowingRotationGizmo() == false)
				{
					SetRotationGizmoShown(true);
				}
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
				cardCanvasGroup.alpha = 1f;

				isPendingPlacement = false;
				SetRotationGizmoShown(false);
				SetPieceValidState(true);
			}
			else
			{
				pieceContainer.gameObject.SetActive(true);
				
				// ToDo: Fade this
				cardCanvasGroup.alpha = 0f;
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
			SetRotationGizmoShown(false);
		}
		
#endregion //Interaction

#region Rotation Gizmo

		//-///////////////////////////////////////////////////////////
		/// 
		public void SetRotationGizmoShown(bool argShow)
		{
			if (argShow)
			{
				HandController.instance.ClearAllRotationGizmos();
			}
			rotationGizmoCanvasGroup.gameObject.SetActive(argShow);
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		private bool IsShowingRotationGizmo()
		{
			return rotationGizmoCanvasGroup.gameObject.activeSelf;
		}

		//-///////////////////////////////////////////////////////////
		/// 
		private void RotatePieceLeft()
		{
			if (IsShowingRotationGizmo())
			{
				linkedPieceView.transform.Rotate(new Vector3(0f, 0f, 30f), Space.Self);
				
				GameBoardController.instance.OnPieceRotated(this);
			}
		}

		//-///////////////////////////////////////////////////////////
		/// 
		private void RotatePieceRight()
		{
			if (IsShowingRotationGizmo())
			{
				
				linkedPieceView.transform.Rotate(new Vector3(0f, 0f, -30f), Space.Self);
				
				GameBoardController.instance.OnPieceRotated(this);
			}
		}

#endregion //Rotation Gizmo

#region Valid State

		//-///////////////////////////////////////////////////////////
		/// 
		public void SetPieceValidState(bool argIsValid)
		{
			linkedPieceView.SetValidState(argIsValid);
			linkedPieceView.SetAllCollidersEnabled(argIsValid == false);
		}

#endregion //Valid State
		
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