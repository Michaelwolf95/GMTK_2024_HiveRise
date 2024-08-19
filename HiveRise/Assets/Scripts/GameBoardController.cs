using System;
using System.Collections;
using System.Collections.Generic;
using MichaelWolfGames;
using UnityEngine;

namespace HiveRise
{
	//-///////////////////////////////////////////////////////////
	/// 
	public class GameBoardController : SceneSingleton<GameBoardController>
	{
		[SerializeField] private Transform foundationRoot = null;
		[SerializeField] private Collider2D foundationCollider = null;
		[SerializeField] private List<Collider2D> environmentColliders = null;
		
		[SerializeField] private Transform pieceContainer = null;
		[Space]
		[SerializeField] private HeightTracker _heightTracker = null;
		public HeightTracker heightTracker => _heightTracker;

		private List<PieceView> allPieceViewsOnBoard = new List<PieceView>();
		private List<List<PieceView>> pieceViewsAddedPerGame = new List<List<PieceView>>();
		//private List<PieceView> pendingPieceViews = new List<PieceView>();

		public float currentTowerHeight { get; private set; }

		public bool isAnimatingPlacingPieces { get; private set; }

		public const float PENDING_PIECE_Z_POS = -5f;
		public const float SELECTED_PIECE_Z_POS = -6f;

#region Events
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void OnNewRunStarted()
		{
			currentTowerHeight = 0f;
			pieceViewsAddedPerGame = new List<List<PieceView>>();
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void OnNewGameStarted()
		{
			pieceViewsAddedPerGame.Add(new List<PieceView>());
			heightTracker.SetTargetHeight(GameManager.instance.GetCurrentTargetHeight());
			
			heightTracker.SetCurrentHeight(currentTowerHeight);
			CameraRigController.instance.SetCurrentHeight(currentTowerHeight);

			AudioHooks.instance.cardShuffle.PlayOneShot();
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void OnGameWon()
		{
			// Lock all pieces
			foreach (PieceView pieceView in allPieceViewsOnBoard)
			{
				pieceView.SetPermanentlyFrozen(true);
			}
		}
		
#endregion //Events

		
		//-///////////////////////////////////////////////////////////
		/// 
		public void RefreshValidStateOfPendingPieces()
		{
			foreach (CardView cardView in HandController.instance.pendingPlacementCardViews)
			{
				bool valid = IsPieceValid(cardView.linkedPieceView);
				cardView.SetPieceValidState(valid);
			}
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public bool IsPieceValid(PieceView argPieceView)
		{
			// ToDo: Cache this instead of rebuilding each time.
			List<Collider2D> checkColliders = new List<Collider2D>();
			checkColliders.Add(foundationCollider);
			checkColliders.AddRange(environmentColliders);
			foreach (PieceView pieceView in allPieceViewsOnBoard)
			{
				checkColliders.AddRange(pieceView.GetAllColliders());
			}

			foreach (CardView cardView in HandController.instance.pendingPlacementCardViews)
			{
				if (cardView.linkedPieceView != argPieceView)
				{
					checkColliders.AddRange(cardView.linkedPieceView.GetAllColliders());
				}
			}
			bool result = argPieceView.DoesPieceOverlapColliders(checkColliders) == false;
			return result;
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void OnPieceRotated(CardView argCardView)
		{
			Physics2D.SyncTransforms();
			bool valid = IsPieceValid(argCardView.linkedPieceView);
			argCardView.SetPieceValidState(valid);
			RefreshValidStateOfPendingPieces();
			UIManager.instance.OnPendingPieceUpdated();
			
			AudioHooks.instance.pieceRotate.PlayOneShot();
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public bool AreAllPendingPiecesValid()
		{
			foreach (CardView cardView in HandController.instance.pendingPlacementCardViews)
			{
				if (IsPieceValid(cardView.linkedPieceView) == false)
				{
					return false;
				}
			}
			return true;
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public int GetNumPendingPieces()
		{
			return HandController.instance.pendingPlacementCardViews.Count;
		}

		//-///////////////////////////////////////////////////////////
		/// 
		public bool CanAllPendingPiecesBeApplied()
		{
			return HandController.instance.pendingPlacementCardViews.Count <= GameManager.MAX_CARDS_PER_PLAY
			       && (HandController.instance.pendingPlacementCardViews.Count >= GameManager.instance.minValidPiecesToScore 
			           || HandController.instance.currentCardsInHand.Count < GameManager.instance.minValidPiecesToScore)
			       && AreAllPendingPiecesValid();
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void PlacePendingCard(CardView argCardView)
		{
			// ToDo: Check this??
			TempFreezeAllPieces();
			
			argCardView.transform.SetParent(pieceContainer);
			
			// Set pos
			Vector3 pos = argCardView.transform.position;
			pos.z = this.transform.position.z + PENDING_PIECE_Z_POS;
			argCardView.transform.position = pos;
			
			// ToDo: Is this necessary?
			// This is necessary for checking overlaps between colliders!
			argCardView.linkedPieceView.SetAllCollidersEnabled(true);

			UIManager.instance.OnPendingPiecePlaced();
			
			AudioHooks.instance.cardPlace.PlayOneShot();
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void RemovePendingCard(CardView argCardView)
		{
			argCardView.transform.SetParent(HandController.instance.handContainer);
			argCardView.linkedPieceView.SetAllCollidersEnabled(false);
				
			UIManager.instance.OnPendingPieceReturnedToHand();
		}

		//-///////////////////////////////////////////////////////////
		/// 
		public void ApplyAllPendingPieces()
		{
			ReleaseTempFreezeAllPieces();
			
			StartCoroutine(CoApplyAllPendingPieces());
		}
		//
		// //-///////////////////////////////////////////////////////////
		// /// 
		// public void ApplyPiece(PieceView argPieceView)
		// {
		// 	allPieceViewsOnBoard.Add(argPieceView);
		// 	argPieceView.SetPhysical(true);
		// }

		//-///////////////////////////////////////////////////////////
		/// 
		private IEnumerator CoApplyAllPendingPieces()
		{
			isAnimatingPlacingPieces = true;
			
			List<PieceView> placedPieceViews = new List<PieceView>();
			foreach (CardView cardView in HandController.instance.pendingPlacementCardViews)
			{
				cardView.linkedPieceView.transform.SetParent(pieceContainer);
				placedPieceViews.Add(cardView.linkedPieceView);
				cardView.OnLinkedPiecePlayed();
			}
			HandController.instance.ClearPendingPlacementCardViews();
			pieceViewsAddedPerGame[pieceViewsAddedPerGame.Count-1].AddRange(placedPieceViews);
			
			foreach (PieceView pieceView in placedPieceViews)
			{
				yield return StartCoroutine(CoApplyPiece(pieceView));
			}
			
			//pendingPieceViews.Clear();
			
			// ToDo: Wait for a coroutine of effects.
			
			// Wait for physics to settle.
			yield return new WaitForSeconds(1f);
			bool settled = false;
			while (settled == false)
			{
				settled = true;
				foreach (PieceView pieceView in allPieceViewsOnBoard)
				{
					if ((CameraRigController.instance.mainCamera.transform.position.y - pieceView.transform.position.y) >
					    ((CameraRigController.instance.mainCamera.orthographicSize) + 4f))
					{
						continue;
					}
					//pieceView.rigidbody2D.IsSleeping()
					if (pieceView.rigidbody2D.velocity.sqrMagnitude > 0.1f || pieceView.rigidbody2D.angularVelocity > 0.1f)
					{
						settled = false;
						break;
					}
				}
				yield return new WaitForSeconds(0.5f);
			}

			yield return new WaitForFixedUpdate();
			StickAllSameColorPiecesAtRest();
			yield return new WaitForFixedUpdate();
			
			// ToDo: Wait for height tracker to finish moving?
			CalculateCurrentTowerHeight();

			isAnimatingPlacingPieces = false;
			GameManager.instance.OnPiecePlacementFinished();
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		private IEnumerator CoApplyPiece(PieceView argPieceView)
		{
			// ToDo: Wait for effects.
			allPieceViewsOnBoard.Add(argPieceView);
			argPieceView.SetPhysical(true);
			yield return new WaitForSeconds(0.15f);

			HashSet<PieceView> nearbyPieces = argPieceView.GetNearbyPieces();
			foreach (PieceView nearbyPiece in nearbyPieces)
			{
				if (nearbyPiece.pieceCardData.color == argPieceView.pieceCardData.color)
				{
					argPieceView.TryStickToPiece(nearbyPiece);
					// // Stick!
					// FixedJoint2D joint = argPieceView.gameObject.AddComponent<FixedJoint2D>();
					// joint.connectedBody = nearbyPiece.rigidbody2D;
				}
			}
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		private void StickAllSameColorPiecesAtRest()
		{
			foreach (PieceView pieceView in allPieceViewsOnBoard)
			{
				HashSet<PieceView> nearbyPieces = pieceView.GetNearbyPieces();
				foreach (PieceView nearbyPiece in nearbyPieces)
				{
					if (nearbyPiece.pieceCardData.color == pieceView.pieceCardData.color)
					{
						pieceView.TryStickToPiece(nearbyPiece);
					}
				}
			}
		}

		//-///////////////////////////////////////////////////////////
		/// 
		private void TempFreezeAllPieces()
		{
			//Debug.Log("Start Temp Freeze!".RichText(Color.cyan));
			foreach (PieceView pieceView in allPieceViewsOnBoard)
			{
				pieceView.TempFreezeConstraints();
			}
		}

		//-///////////////////////////////////////////////////////////
		/// 
		private void ReleaseTempFreezeAllPieces()
		{
			//Debug.Log("Release Temp Freeze!".RichText(Color.cyan));
			foreach (PieceView pieceView in allPieceViewsOnBoard)
			{
				pieceView.ReleaseTempFreezeConstraints();
			}
		}
		

		
#region Progress Tracking

		
		//-///////////////////////////////////////////////////////////
		/// 
		private void CalculateCurrentTowerHeight()
		{
			RaycastHit2D hit = Physics2D.BoxCast(new Vector2(0, 9999f), new Vector2(10f, 1f), 0f, Vector2.down);
			if (hit.collider != null)
			{
				currentTowerHeight = hit.point.y - heightTracker.transform.position.y;
				FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Height", currentTowerHeight);

				heightTracker.SetCurrentHeight(currentTowerHeight);
				
				CameraRigController.instance.SetCurrentHeight(currentTowerHeight);
			}
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public int CalculateHoneyScore()
		{
			int score = 0;
			List<PieceView> scoredPieces = pieceViewsAddedPerGame[pieceViewsAddedPerGame.Count - 1];
			foreach (PieceView pieceView in scoredPieces)
			{
				if (IsPieceValidForScoring(pieceView))
				{
					int value = pieceView.pieceCardData.pieceData.shapePointValue;
					switch ( pieceView.pieceCardData.scoreMultiplier)
					{
						case ScoreMultiplier.Double:
							value *= 2;
							break;
						case ScoreMultiplier.Tripple:
							value *= 3;
							break;
					}
					score += value;
				}
			}
			return score;
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		private bool IsPieceValidForScoring(PieceView argPieceView)
		{
			if (argPieceView.transform.position.y > foundationRoot.transform.position.y)
			{
				// NOTE: This creates a false-positive for stacks off to the side that are off the foundation.
				return true;
			}
			
			return false;
		}

#endregion // Progress Tracking
	}
}