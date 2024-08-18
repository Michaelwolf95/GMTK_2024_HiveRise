using System;
using System.Collections.Generic;
using MichaelWolfGames;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace HiveRise
{
	//-///////////////////////////////////////////////////////////
	/// 
	public class HandController : MonoBehaviour
	{
		public static HandController instance => GameManager.instance.handController;

		[SerializeField] private List<CardView> _currentCardsInHand = new List<CardView>();
		public List<CardView> currentCardsInHand => _currentCardsInHand;

		[SerializeField] private SplineContainer handCurveSpline = null;
		[SerializeField] private Transform _handContainer = null;
		public Transform handContainer => _handContainer;

		public List<CardView> pendingPlacementCardViews { get; set; }
		
		public const float CARD_MOVE_SPEED = 25f;
		public const float CARD_ROTATION_SPEED = 20f;
		
		public CardView currentDragCard { get; private set; }
		private bool didCurrentDragCardStartInHand = false;

		//-///////////////////////////////////////////////////////////
		/// 
		private void Awake()
		{
			pendingPlacementCardViews = new List<CardView>();
		}

		//-///////////////////////////////////////////////////////////
		/// 
		private void Update()
		{
			UpdateCurrentDrag();
			UpdateCardHandPositions();
		}

#region Drawing

		//-///////////////////////////////////////////////////////////
		/// 
		public void OnNewTurnStarted()
		{
			DrawCards(GameManager.MAX_CARDS_IN_HAND - currentCardsInHand.Count);
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void DrawCards(int numCards)
		{
			for (int i = 0; i < numCards; i++)
			{
				if (TryDrawCard() != null)
				{
					this.InvokeAction((() =>
					{
						AudioHooks.instance.cardDraw.PlayOneShot();
					}), i * 0.1f);
				}
			}
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public CardView TryDrawCard()
		{
			CardData cardData = DeckController.instance.DrawCard();
			if (cardData != null)
			{
				CardView cardView = Instantiate(CardDefinitions.instance.cardViewPrefab, handContainer);
				cardView.transform.rotation = GetDefaultCardRotation();
				cardView.SetData(cardData);
				currentCardsInHand.Add(cardView);
				return cardView;
			}
			return null;
		}
		
		//-///////////////////////////////////////////////////////////
		///
		public void ClearCurrentCards()
		{
			foreach (CardView cardView in currentCardsInHand)
			{
				Destroy(cardView.gameObject);
			}
			currentCardsInHand.Clear();
		}

#endregion // Drawing
		
#region Dragging
		
		//-///////////////////////////////////////////////////////////
		/// 
		private void UpdateCurrentDrag()
		{
			if (currentDragCard != null)
			{
				// Update movement
				currentDragCard.UpdateDrag(Input.mousePosition);
			}
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		private void UpdateCardHandPositions()
		{
			int numCardsInHand = currentCardsInHand.Count;
			for (int i = 0; i < currentCardsInHand.Count; i++)
			{
				if (currentCardsInHand[i].isBeingDragged == false)
				{
					float t = (((float) i + 1) / (numCardsInHand + 1));
					float3 pos, up;
					if(handCurveSpline.Evaluate(t, out pos, out _, out up))
					{
						//Debug.Log($"{i}/{numCardsInHand}: {t}, {pos}");
						Vector3 targetPos = new Vector3(pos.x, pos.y, pos.z);
						targetPos.z += (i * 0.2f);
						
						currentCardsInHand[i].transform.position = Vector3.MoveTowards(currentCardsInHand[i].transform.position, targetPos, CARD_MOVE_SPEED * Time.deltaTime);

						Vector3 upVector = new Vector3(up.x, up.y, up.z);
						Quaternion targetRotation = Quaternion.AngleAxis(Vector2.SignedAngle(Vector2.left, upVector), Vector3.forward);
						// Rotate the object by setting its rotation
						currentCardsInHand[i].transform.rotation = Quaternion.RotateTowards(currentCardsInHand[i].transform.rotation, targetRotation, HandController.CARD_ROTATION_SPEED * Time.deltaTime);

						if (currentCardsInHand[i].isCardMode == false)
						{
							currentCardsInHand[i].CheckForStateChange();
						}
					}
				}
			}
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public Quaternion GetDefaultCardRotation()
		{
			//return handCurveSpline.transform.rotation;
			return Quaternion.identity;
		}


		//-///////////////////////////////////////////////////////////
		/// 
		public void TryStartDraggingCard(CardView argCardView)
		{
			if (GameManager.instance.CanDragCards() && currentDragCard == null && (pendingPlacementCardViews.Contains(argCardView) || pendingPlacementCardViews.Count < GameManager.MAX_CARDS_PER_PLAY))
			{
				currentDragCard = argCardView;
				didCurrentDragCardStartInHand = currentCardsInHand.Contains(currentDragCard);
				
				//dragStartWorldSpaceOffset
				
				currentDragCard.OnStartDragging();

				ClearAllRotationGizmos();
			}
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void TryStopDraggingCard(CardView argCardView)
		{
			if (currentDragCard != null && currentDragCard == argCardView)
			{
				currentDragCard.OnStopDragging();

				if (IsPointWithinHandContainer(currentDragCard.transform.position))
				{
					// Add back to hand if it wasn't already.
					if (currentCardsInHand.Contains(currentDragCard) == false)
					{
						currentCardsInHand.Add(currentDragCard);
					}
					if (pendingPlacementCardViews.Contains(currentDragCard))
					{
						pendingPlacementCardViews.Remove(currentDragCard);
						GameBoardController.instance.RemovePendingCard(currentDragCard);
					}
				}
				else
				{
					if (GameBoardController.instance.IsPieceValid(currentDragCard.linkedPieceView))
					{
						currentDragCard.SetPieceValidState(true);
						if (currentCardsInHand.Contains(currentDragCard))
						{
							currentCardsInHand.Remove(currentDragCard);
						}
						if (pendingPlacementCardViews.Contains(currentDragCard) == false)
						{
							pendingPlacementCardViews.Add(currentDragCard);
							//GameBoardController.instance.PlacePendingCard(currentDragCard);
						}
						GameBoardController.instance.PlacePendingCard(currentDragCard);
						
						OnCardPlacedAsPendingPiece(currentDragCard);
					}
					else
					{
						// ToDo: Update as invalid placement state.
						currentDragCard.SetPieceValidState(false);
						UIManager.instance.OnPendingPieceUpdated();
					}
					GameBoardController.instance.RefreshValidStateOfPendingPieces();
				}
				
				currentDragCard = null;
				didCurrentDragCardStartInHand = false;
			}
		}

		//-///////////////////////////////////////////////////////////
		/// 
		public bool IsPointWithinHandContainer(Vector3 argWorldPoint)
		{
			Vector2 screenPoint = CameraRigController.instance.mainCamera.WorldToScreenPoint(argWorldPoint);
			return RectTransformUtility.RectangleContainsScreenPoint(UIManager.instance.handContainerRect, screenPoint);
		}

#endregion //Dragging

#region Pending Cards

		//-///////////////////////////////////////////////////////////
		///
		private void OnCardPlacedAsPendingPiece(CardView argCardView)
		{
			argCardView.OnLinkedPiecePlaced();

			if (didCurrentDragCardStartInHand)
			{
				// Show rotation gizmo, etc.
				argCardView.SetRotationGizmoShown(true);
			}
			
			// ToDo: Tint cards if full, etc.
		}
		
		//-///////////////////////////////////////////////////////////
		///
		public void ClearPendingPlacementCardViews()
		{
			 // !!! NOTE: This assumes that the pieces have been removed.
			 foreach (CardView cardView in pendingPlacementCardViews)
			 {
				 // ToDo: Pool these?
				 Destroy(cardView.gameObject);
			 }
			 pendingPlacementCardViews.Clear();
		}

		//-///////////////////////////////////////////////////////////
		/// 
		public void ClearAllRotationGizmos()
		{
			foreach (CardView cardView in pendingPlacementCardViews)
			{
				cardView.SetRotationGizmoShown(false);
			}
		}

#endregion // Pending Cards
		
#region Debug

		//-///////////////////////////////////////////////////////////
		/// 
		private void OnDrawGizmosSelected()
		{
			Color originalGizmoColor = Gizmos.color;
			for (int i = 0; i < 10; i++)
			{
				float t = (float) i / 10f;
				float3 pos;
				float3 tangent;
				float3 up;
				if(handCurveSpline.Evaluate(t, out pos, out tangent, out up))
				{
					//Debug.Log($"{i}/{numCardsInHand}: {t}, {pos}");
					Vector3 startPos = new Vector3(pos.x, pos.y, pos.z);

					Vector3 upVector = new Vector3(up.x, up.y, up.z);
					Vector3 tangentVector = new Vector3(tangent.x, tangent.y, tangent.z);
					
					//Debug.Log(upVector);
					Gizmos.color = Color.cyan;
					Gizmos.DrawLine(startPos, startPos + upVector.normalized);
					Gizmos.color = Color.magenta;
					Gizmos.DrawLine(startPos, startPos + tangentVector.normalized);
				}
			}
			Gizmos.color = originalGizmoColor;
		}
		

#endregion
	}
}