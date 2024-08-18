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
		
		[SerializeField] private DeckController _deckController = null;
		public DeckController deckController => _deckController;
		
		[SerializeField] private CardDefinitions _cardDefinitions = null;
		public CardDefinitions cardDefinitions => _cardDefinitions;
		
		public const int MAX_CARDS_PER_PLAY = 3;
		public const int MAX_CARDS_IN_HAND = 5;

		//-///////////////////////////////////////////////////////////
		/// 
		protected override void Awake()
		{
			base.Awake();
		}

		//-///////////////////////////////////////////////////////////
		/// 
		private void Start()
		{
			// ToDo: Change this to work in menu flow.
			StartNewRun();
		}

#region High-Level Game Logic

		
		//-///////////////////////////////////////////////////////////
		/// 
		private void StartNewRun()
		{
			deckController.InitDeckForNewRun();
			StartNewGame();
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		private void StartNewGame()
		{
			deckController.InitDeckForNewGame();
			
			// ToDo: Reset everything for new game.
			GameBoardController.instance.OnNewGameStarted();
			
			StartNewTurn();
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		private void StartNewTurn()
		{
			HandController.instance.OnNewTurnStarted();
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		private void OnGameWon()
		{
			// ToDo: UI popup for winning the game.
		}

		//-///////////////////////////////////////////////////////////
		/// 
		private void OnGameLost()
		{
			// ToDo: UI popup for losing the run.
		}

		//-///////////////////////////////////////////////////////////
		/// 
		private bool IsGameWon()
		{
			return GameBoardController.instance.currentTowerHeight >= GetCurrentTargetHeight();
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		private bool IsGameLost()
		{
			// Check if deck is empty?
			return deckController.GetNumRemainingInDeck() <= 0;
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public float GetCurrentTargetHeight()
		{
			// ToDo: Get this from a config file.
			return 100f;
		}
		
#endregion //High-Level Game Logic
		
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
			if (IsGameWon())
			{
				OnGameWon();
			}
			else if (IsGameLost())
			{
				OnGameLost();
			}
			else
			{
				StartNewTurn();
			}
		}

		//-///////////////////////////////////////////////////////////
		/// 
		public bool CanDragCards()
		{
			return GameBoardController.instance.isAnimatingPlacingPieces == false;
		}
	}
}