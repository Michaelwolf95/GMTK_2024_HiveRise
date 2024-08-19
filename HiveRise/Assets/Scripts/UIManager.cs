using System;
using MichaelWolfGames;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HiveRise
{
	//-///////////////////////////////////////////////////////////
	/// 
	public class UIManager : SceneSingleton<UIManager>
	{
		[SerializeField] private RectTransform _handContainerRect = null;
		public RectTransform handContainerRect => _handContainerRect;

		[SerializeField] private CanvasGroup mainCanvasGroup = null;
		
		[Header("Buttons")]
		[SerializeField] private Button submitButton = null;
		[SerializeField] private TextMeshProUGUI placementTrackerLabel = null;
		[SerializeField] private string placementTrackerFormatString = "{0}/{1} PLACED";
		[SerializeField] private RectTransform[] placementCounters = null;
		[Space]
		[SerializeField] private GameObject honeyCounterContainer = null;
		[SerializeField] private TextMeshProUGUI honeyCounter = null;
		[SerializeField] private TextMeshProUGUI deckCounter = null;
		[SerializeField] private TextMeshProUGUI deckCounter2 = null;
		[SerializeField] private string deckCounterFormatString = "{0}/{1}";
		[SerializeField] private Button deckCounterButton;
		[Space]
		[SerializeField] private ShopMenu shopMenu;
		[SerializeField] private DeckPreviewMenu deckPreviewMenu;
		[Header("MainMenu")]
		[SerializeField] private CanvasGroup mainMenuCanvasGroup;
		[SerializeField] private Button mainMenuStartButton;
		[Header("Game Over")]
		[SerializeField] private CanvasGroup gameOverScreenCanvasGroup;
		[SerializeField] private Button retryButton;
		[SerializeField] private TextMeshProUGUI gameOverScreenTextLabel;
		[SerializeField] private string gameOverScreenTextFormatString = "Your hive was {0}m tall";
		[Header("Game Won")]
		[SerializeField] private CanvasGroup gameWonScreenCanvasGroup;
		[SerializeField] private Button retryWinButton;
		[SerializeField] private TextMeshProUGUI gameWonScreenTextLabel;
		[SerializeField] private string gameWonScreenTextFormatString = "Your hive was {0}m tall";

		public int numMenusOpen { get; private set; }
		public bool isMenuOpen => numMenusOpen > 0;

		
		//-///////////////////////////////////////////////////////////
		/// 
		protected override void Awake()
		{
			base.Awake();
			submitButton.onClick.AddListener(OnSubmitButtonPressed);
			submitButton.interactable = false;
			
			shopMenu.gameObject.SetActive(false);
			deckPreviewMenu.gameObject.SetActive(false);
			
			SetHoneyCount(0);
			
			deckCounterButton.onClick.AddListener((() =>
			{
				if (isMenuOpen == false)
				{
					ShowDeckPreviewMenu(DeckPreviewMode.Preview);
				}
				else
				{
					deckPreviewMenu.DismissMenu();
				}
			}));
			
			mainMenuStartButton.onClick.AddListener((() =>
			{
				GameManager.instance.OnMainMenuStartPressed();
				
				this.InvokeAction((() =>
				{
					mainMenuCanvasGroup.gameObject.SetActive(false);
				}), 1f);
			}));
			
			retryButton.onClick.AddListener(() =>
			{
				GameManager.instance.OnGameOverScreenRetryPressed();
			});
			
			retryWinButton.onClick.AddListener(() =>
			{
				GameManager.instance.OnGameOverScreenRetryPressed();
			});
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void SetGameUIVisible(bool argVisible)
		{
			mainCanvasGroup.gameObject.SetActive(argVisible);
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void OnStartNewGame()
		{
			UpdateDeckTrackerLabel();
			UpdatePlacementTrackerLabel();
		}

		//-///////////////////////////////////////////////////////////
		/// 
		private void OnSubmitButtonPressed()
		{
			if (GameBoardController.instance.CanAllPendingPiecesBeApplied())
			{
				GameManager.instance.PlayPendingPieces();
				
				UpdatePlacementTrackerLabel();
				submitButton.interactable = GameBoardController.instance.CanAllPendingPiecesBeApplied();
			}
		}

		//-///////////////////////////////////////////////////////////
		/// 
		public void OnPendingPiecePlaced()
		{
			UpdatePlacementTrackerLabel();
			submitButton.interactable = GameBoardController.instance.CanAllPendingPiecesBeApplied();
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void OnPendingPieceUpdated()
		{
			submitButton.interactable = GameBoardController.instance.CanAllPendingPiecesBeApplied();
		}

		//-///////////////////////////////////////////////////////////
		/// 
		public void OnPendingPieceReturnedToHand()
		{
			UpdatePlacementTrackerLabel();
			submitButton.interactable = GameBoardController.instance.CanAllPendingPiecesBeApplied();
		}

		//-///////////////////////////////////////////////////////////
		/// 
		private void UpdatePlacementTrackerLabel()
		{
			int numPending = GameBoardController.instance.GetNumPendingPieces();
			placementTrackerLabel.text = string.Format(placementTrackerFormatString, numPending, GameManager.MAX_CARDS_PER_PLAY);

			for (int i = 0; i < placementCounters.Length; i++)
			{
				placementCounters[i].gameObject.SetActive(i < numPending);
			}
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void UpdateDeckTrackerLabel()
		{
			deckCounter.text = string.Format(deckCounterFormatString, DeckController.instance.GetNumRemainingInDeck(), DeckController.instance.GetTotalCardsInDeck());
			deckCounter2.text = string.Format(deckCounterFormatString, DeckController.instance.GetNumRemainingInDeck(), DeckController.instance.GetTotalCardsInDeck());
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void SetHoneyCount(int argCount)
		{
			honeyCounter.text = argCount.ToString();
			shopMenu.SetHoneyCount(argCount);
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void ShowShopMenu(Action argOnDismiss = null)
		{
			shopMenu.ShowMenu(argOnDismiss);
			honeyCounterContainer.gameObject.SetActive(false);
			numMenusOpen++;
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void OnShopMenuClosed()
		{
			//shopMenu.HideMenu();
			honeyCounterContainer.gameObject.SetActive(true);
			numMenusOpen--;
		}

		//-///////////////////////////////////////////////////////////
		/// 
		public void ShowRunLostMenu()
		{
			// ToDo: Open this menu
			numMenusOpen++;
			gameOverScreenCanvasGroup.gameObject.SetActive(true);

			gameOverScreenTextLabel.text = string.Format(gameOverScreenTextFormatString, GameBoardController.instance.currentTowerHeight.ToString("F1"));

			AudioHooks.instance.loseGame.PlayOneShot();
		}

		//-///////////////////////////////////////////////////////////
		/// 
		public void ShowRunWonMenu()
		{
			// ToDo: Open this menu
			numMenusOpen++;
			gameWonScreenCanvasGroup.gameObject.SetActive(true);

			gameWonScreenTextLabel.text = string.Format(gameWonScreenTextFormatString, GameBoardController.instance.currentTowerHeight.ToString("F1"));

			AudioHooks.instance.winGame.PlayOneShot();
		}

		//-///////////////////////////////////////////////////////////
		///
		public void OnRunLostMenuClosed()
		{
			// ToDo: Close this menu
			numMenusOpen--;
		}

		//-///////////////////////////////////////////////////////////
		///
		public void OnRunWonMenuClosed()
		{
			// ToDo: Close this menu
			numMenusOpen--;
		}

		//-///////////////////////////////////////////////////////////
		/// 
		public void ShowDeckPreviewMenu(DeckPreviewMode argPreviewMode)
		{
			deckPreviewMenu.ShowMenu(argPreviewMode);
			numMenusOpen++;
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void OnDeckPreviewMenuClosed()
		{
			// ToDo: Close this menu
			numMenusOpen--;
		}
		
	}
}