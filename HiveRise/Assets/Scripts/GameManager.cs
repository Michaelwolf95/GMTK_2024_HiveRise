using System;
using System.Collections;
using System.Collections.Generic;
using MichaelWolfGames;
using UnityEngine;

namespace HiveRise
{
	//-///////////////////////////////////////////////////////////
	/// 
	public class GameManager : SceneSingleton<GameManager>
	{
		[SerializeField] private HandController _handController = null;
		public HandController handController => _handController;
		
		public const int MAX_CARDS_PER_PLAY = 3;

		//-///////////////////////////////////////////////////////////
		/// 
		protected override void Awake()
		{
			base.Awake();
		}

		//-///////////////////////////////////////////////////////////
		/// 
		public void PlayPendingPieces()
		{
			if (GameBoardController.instance.CanAllPendingPiecesBeApplied())
			{
				GameBoardController.instance.ApplyAllPendingPieces();
				
				// ToDo: Game Logic Updates!
			}
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void OnPiecePlacementFinished()
		{
			// ToDo: Evaluate game state.
			
			// ToDo: Draw more cards
			
			// ToDo: Reset everything.
		}
	}
}