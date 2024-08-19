using System;
using System.Collections;
using System.Collections.Generic;
using MichaelWolfGames;
using UnityEngine;
using UnityEngine.SceneManagement;

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

		[SerializeField] private ShopCardDefinitions _shopCardDefinitions = null;
		public ShopCardDefinitions shopCardDefinitions => _shopCardDefinitions;
		
		[SerializeField] private ProgressionConfig _progressionConfig = null;
		public ProgressionConfig progressionConfig => _progressionConfig;
		
		[SerializeField] private AudioHooks _audioHooks = null;
		public AudioHooks audioHooks => _audioHooks;
		
		[Header("Rules")]
		[SerializeField] private int _minValidPiecesToScore = 3;
		public int minValidPiecesToScore => _minValidPiecesToScore;
		
		public int currentHoneyCount { get; set; }
		public int currentProgressionTierIndex { get; set; }
		
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
			//StartNewRun();
			currentHoneyCount = 0;
			currentProgressionTierIndex = 0;
			InitOnMainMenu();
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		private void Update()
		{
			UpdateDebugKeys();
		}

#region High-Level Game Logic

		//-///////////////////////////////////////////////////////////
		/// 
		public void InitOnMainMenu()
		{
			UIManager.instance.SetGameUIVisible(false);
			CameraRigController.instance.JumpToMainMenuHeight();
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void OnMainMenuStartPressed()
		{
			CameraRigController.instance.SetCurrentHeight(0f, () =>
			{
				this.InvokeAction((() =>
				{
					UIManager.instance.SetGameUIVisible(true, true);
					StartNewRun();
				}), 0.5f);
			});
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void OnGameOverScreenRetryPressed()
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}

		//-///////////////////////////////////////////////////////////
		/// 
		private void StartNewRun()
		{
			currentHoneyCount = 0;
			currentProgressionTierIndex = 0;
			deckController.InitDeckForNewRun();
			HandController.instance.ClearCurrentCards();
			UIManager.instance.SetHoneyCount(currentHoneyCount);
			GameBoardController.instance.OnNewRunStarted();
			StartNewGame();
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		private void StartNewGame()
		{
			deckController.InitDeckForNewGame();
			UIManager.instance.OnStartNewGame();
			
			// ToDo: Reset everything for new game.
			GameBoardController.instance.OnNewGameStarted();
			
			StartNewTurn();
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		private void StartNewTurn()
		{
			HandController.instance.OnNewTurnStarted();
			UIManager.instance.UpdateDeckTrackerLabel();
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		private void OnGameWon()
		{
			HandController.instance.ClearCurrentCards();
			
			currentProgressionTierIndex++;
			
			GameBoardController.instance.OnGameWon();
			
			
			// ToDo: Animate this before showing shop
			AddHoney(GameBoardController.instance.CalculateHoneyScore());
				
			AudioHooks.instance.checkpoint.PlayOneShot();
			
			this.InvokeAction((() =>
			{
				AudioHooks.instance.moneyPayout.PlayOneShot();
				
				CameraRigController.instance.SetCurrentHeight(GameBoardController.instance.currentTowerHeight, (() =>
				{
					if (currentProgressionTierIndex >= progressionConfig.progressionTiers.Length)
					{
						OnRunComplete();
					}
					else
					{
						UIManager.instance.ShowShopMenu((() =>
						{
							StartNewGame();
						}));
					}
				}));
			}), 0.2f);
		}

		//-///////////////////////////////////////////////////////////
		/// 
		private void OnGameLost()
		{
			// ToDo: UI popup for losing the run.
			UIManager.instance.ShowRunLostMenu();
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
			return progressionConfig.GetHeightRequirementForTierIndex(currentProgressionTierIndex);
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		private void OnRunComplete()
		{
			// ToDo: UI popup for winning the run.
			
			UIManager.instance.ShowRunWonMenu();
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
			return GameBoardController.instance.isAnimatingPlacingPieces == false 
			       && UIManager.instance.isMenuOpen == false;
		}

#region Economy

		//-///////////////////////////////////////////////////////////
		/// 
		public void AddHoney(int argCount)
		{
			currentHoneyCount += argCount;
			UIManager.instance.SetHoneyCount(currentHoneyCount);
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void SpendHoney(int argCount)
		{
			currentHoneyCount -= argCount;
			UIManager.instance.SetHoneyCount(currentHoneyCount);
		}

#endregion //Economy

#region Debug

		//-///////////////////////////////////////////////////////////
		/// 
		private void UpdateDebugKeys()
		{
			bool control = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) || 
			               Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand);
			if (control)
			{
				if (Input.GetKeyDown(KeyCode.W))
				{
					OnGameWon();
				}
				if (Input.GetKeyDown(KeyCode.L))
				{
					OnGameLost();
				}
			}
		}

#endregion //Debug
	}
}