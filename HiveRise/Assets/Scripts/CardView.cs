using System;
using MichaelWolfGames;
using TMPro;
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

		[SerializeField] private CardUIView _cardUIView = null;
		public CardUIView cardUIView => _cardUIView;
		[Space]
		[SerializeField] private GameObject pieceContainer = null;
		[Space]
		[SerializeField] private CanvasGroup rotationGizmoCanvasGroup = null;
		[SerializeField] private Button leftRotationButton = null; 
		[SerializeField] private Button rightRotationButton = null; 
		
		public PieceView linkedPieceView { get; private set; }
		public CardData pieceCardData { get; private set; }
		
		private Vector2 clickStartPos = Vector2.zero;
		private Vector2 dragStartWorldSpaceOffset = Vector2.zero;
		private const float MIN_DRAG_PIXEL_DIST = 5f;
		
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

			cardUIView.SetData(argCardData);
			// cardImage.sprite = CardDefinitions.instance.GetPieceDataForID(argCardData.pieceShapeID).pieceShapeSprite;
			// cardImage.color = cardTintColors[(int) argCardData.color];
			// cardTextLabel.text = argCardData.GetCardDescriptionForData();
			
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
			
			Vector3 pos = transform.position;
			pos.z = GameBoardController.instance.transform.position.z + GameBoardController.SELECTED_PIECE_Z_POS;
			transform.position = pos;
			
			Vector3 mouseWorldPoint = CameraRigController.instance.mainCamera.ScreenToWorldPoint(Input.mousePosition);
			dragStartWorldSpaceOffset = mouseWorldPoint - transform.position;
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
			if (isCardMode == false && IsScreenPointOverLinkedPiece(Input.mousePosition) == false)
			{
				return;
			}
			clickStartPos = Input.mousePosition;
		}

		//-///////////////////////////////////////////////////////////
		/// 
		private void OnMouseDrag()
		{
			if (isCardMode == false && IsScreenPointOverLinkedPiece(Input.mousePosition) == false)
			{
				return;
			}
			if (isBeingDragged == false && Vector2.Distance(Input.mousePosition, clickStartPos) > MIN_DRAG_PIXEL_DIST)
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
		private bool IsScreenPointOverLinkedPiece(Vector2 argScreenPoint)
		{
			Vector3 worldPoint = CameraRigController.instance.mainCamera.ScreenToWorldPoint(argScreenPoint);
			foreach (HexCell cell in linkedPieceView.hexCells)
			{
				float dist = Vector2.Distance(cell.transform.position, worldPoint);
				if (dist <= 0.7f)
				{
					return true;
				}
			}
			return false;
		}

		//-///////////////////////////////////////////////////////////
		/// 
		public void UpdateDrag(Vector2 mousePos)
		{
			Vector3 worldPoint = CameraRigController.instance.mainCamera.ScreenToWorldPoint(mousePos);
            worldPoint.z = transform.position.z;
			if (isCardMode)
			{
                transform.position = worldPoint - (Vector3)dragStartWorldSpaceOffset;
                
                transform.rotation = Quaternion.RotateTowards(transform.rotation, HandController.instance.GetDefaultCardRotation(), HandController.CARD_ROTATION_SPEED * Time.deltaTime);
                // ToDo: Transition when dragged onto board
				
                CheckForStateChange();
			}
			else
			{
				transform.position = worldPoint - (Vector3)dragStartWorldSpaceOffset;
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
				if (HandController.instance.IsWorldPointWithinHandContainer(this.transform.position) == false)
				{
					SetCardMode(false);
				}
			}
			else
			{
				if (HandController.instance.IsWorldPointWithinHandContainer(this.transform.position))
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
				//cardCanvasGroup.alpha = 1f;
				cardUIView.canvasGroup.alpha = 1f;
				
				Vector3 pos = transform.position;
				pos.z = HandController.instance.handContainer.position.z;
				transform.position = pos;

				isPendingPlacement = false;
				SetRotationGizmoShown(false);
				SetPieceValidState(true);
				
				linkedPieceView.OnCancelPlacing();
			}
			else
			{
				pieceContainer.gameObject.SetActive(true);
				linkedPieceView.OnBeginPlacing();
				
				// ToDo: Fade this
				//cardCanvasGroup.alpha = 0f;
				cardUIView.canvasGroup.alpha = 0f;
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
			isPendingPlacement = false;
			linkedPieceView.OnPiecePlaced();
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

			if (isCardMode == false)
			{
				Vector3 pos = transform.localPosition;
				pos.z = (argShow)? GameBoardController.SELECTED_PIECE_Z_POS : GameBoardController.PENDING_PIECE_Z_POS;
				transform.localPosition = pos;
			}
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