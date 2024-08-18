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

		private List<PieceView> allPieceViewsOnBoard = new List<PieceView>();
		private List<PieceView> pendingPieceViews = new List<PieceView>();
		
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
			
			foreach (PieceView pieceView in pendingPieceViews)
			{
				if (pieceView != argPieceView)
				{
					checkColliders.AddRange(pieceView.GetAllColliders());
				}
			}
			bool result = argPieceView.DoesPieceOverlapColliders(checkColliders) == false;
			return result;
		}
		
		
		//-///////////////////////////////////////////////////////////
		/// 
		public bool AreAllPendingPiecesValid()
		{
			foreach (PieceView pieceView in pendingPieceViews)
			{
				if (IsPieceValid(pieceView) == false)
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
			return pendingPieceViews.Count;
		}

		//-///////////////////////////////////////////////////////////
		/// 
		public bool CanAllPendingPiecesBeApplied()
		{
			return pendingPieceViews.Count <= GameManager.MAX_CARDS_PER_PLAY
			       && pendingPieceViews.Count > 0
			       && AreAllPendingPiecesValid();
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void PlacePendingCard(CardView argCardView)
		{
			if (pendingPieceViews.Contains(argCardView.linkedPieceView) == false)
			{
				pendingPieceViews.Add(argCardView.linkedPieceView);
				argCardView.transform.SetParent(pieceContainer);
				argCardView.linkedPieceView.SetAllCollidersEnabled(true);

				UIManager.instance.OnPendingPiecePlaced();
			}
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void RemovePendingCard(CardView argCardView)
		{
			if (pendingPieceViews.Contains(argCardView.linkedPieceView))
			{
				pendingPieceViews.Remove(argCardView.linkedPieceView);
				argCardView.transform.SetParent(HandController.instance.handContainer);
				argCardView.linkedPieceView.SetAllCollidersEnabled(false);
				
				UIManager.instance.OnPendingPieceReturnedToHand();
			}
		}

		//-///////////////////////////////////////////////////////////
		/// 
		public void ApplyAllPendingPieces()
		{
			foreach (PieceView pieceView in pendingPieceViews)
			{
				ApplyPiece(pieceView);
			}
			pendingPieceViews.Clear();
			HandController.instance.ClearPendingPlacementCardViews();
			
			//ToDo: Make this wait for a coroutine of effects.
			GameManager.instance.OnPiecePlacementFinished();
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void ApplyPiece(PieceView argPieceView)
		{
			allPieceViewsOnBoard.Add(argPieceView);
			argPieceView.SetPhysical(true);
		}

		// //-///////////////////////////////////////////////////////////
		// /// 
		// private IEnumerator CoApplyAllPendingPieces()
		// {
		// 	
		// }
		//
		// //-///////////////////////////////////////////////////////////
		// /// 
		// private IEnumerator CoApplyPiece()
		// {
		// 	
		// }
		
	}
}