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
		
		public bool isMenuOpen { get; private set; }
		
		//-///////////////////////////////////////////////////////////
		/// 
		protected override void Awake()
		{
			base.Awake();
			submitButton.onClick.AddListener(OnSubmitButtonPressed);
			submitButton.interactable = false;
			
			shopMenu.gameObject.SetActive(false);
			deckPreviewMenu.gameObject.SetActive(false);
			
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
			isMenuOpen = true;
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void OnShopMenuClosed()
		{
			//shopMenu.HideMenu();
			honeyCounterContainer.gameObject.SetActive(true);
			isMenuOpen = false;
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void ShowRunLostMenu()
		{
			// ToDo: Open this menu
			isMenuOpen = true;
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void OnRunLostMenuClosed()
		{
			// ToDo: Close this menu
			isMenuOpen = false;
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void ShowDeckPreviewMenu(DeckPreviewMode argPreviewMode)
		{
			deckPreviewMenu.ShowMenu(argPreviewMode);
			// ToDo: Open this menu
			isMenuOpen = true;
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void OnDeckPreviewMenuClosed()
		{
			// ToDo: Close this menu
			isMenuOpen = false;
		}
		
	}
}