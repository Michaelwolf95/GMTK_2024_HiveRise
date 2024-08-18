﻿using System;
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
		[Space]
		[SerializeField] private TextMeshProUGUI honeyCounter = null;
		[SerializeField] private TextMeshProUGUI deckCounter = null;
		[SerializeField] private string deckCounterFormatString = "{0}/{1}";
		[Space]
		[SerializeField] private ShopMenu shopMenu;
		
		//-///////////////////////////////////////////////////////////
		/// 
		protected override void Awake()
		{
			base.Awake();
			submitButton.onClick.AddListener(OnSubmitButtonPressed);
			submitButton.interactable = false;
			
			shopMenu.gameObject.SetActive(false);
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
			placementTrackerLabel.text = string.Format(placementTrackerFormatString, GameBoardController.instance.GetNumPendingPieces(), GameManager.MAX_CARDS_PER_PLAY);
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void UpdateDeckTrackerLabel()
		{
			deckCounter.text = string.Format(deckCounterFormatString, DeckController.instance.GetNumRemainingInDeck(), DeckController.instance.GetTotalCardsInDeck());
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void SetHoneyCount(int argCount)
		{
			honeyCounter.text = argCount.ToString();
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void ShowShopMenu(Action argOnDismiss = null)
		{
			shopMenu.ShowMenu(argOnDismiss);
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void DismissShopMenu()
		{
			shopMenu.HideMenu();
		}
		
	}
}