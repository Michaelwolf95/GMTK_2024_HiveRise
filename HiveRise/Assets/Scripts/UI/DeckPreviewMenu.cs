using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

namespace HiveRise
{
	//-///////////////////////////////////////////////////////////
	/// 
	[Serializable]
	public enum DeckPreviewMode
	{
		Preview = 0,
		RemoveCard = 1,
	}
	
	//-///////////////////////////////////////////////////////////
	/// 
	public class DeckPreviewMenu : MonoBehaviour
	{
		[SerializeField] private CanvasGroup _canvasGroup = null;
		public CanvasGroup canvasGroup => _canvasGroup;

		[SerializeField] private RectTransform _cardsContainer = null;
		public RectTransform cardsContainer => _cardsContainer;

		[SerializeField] private CardUIView _cardViewPrefab = null;
		public CardUIView cardViewPrefab => _cardViewPrefab;

		[SerializeField] private Button _closeButton = null;
		public Button closeButton => _closeButton;

		private List<CardUIView> cardViews = new List<CardUIView>();

		private DeckPreviewMode menuMode = DeckPreviewMode.Preview;

		//-///////////////////////////////////////////////////////////
		/// 
		private void Awake()
		{
			closeButton.onClick.AddListener(DismissMenu);
		}

		//-///////////////////////////////////////////////////////////
		/// 
		public void ShowMenu(DeckPreviewMode argMode)
		{
			menuMode = argMode;
			
			InitDeck();
			
			gameObject.SetActive(true);

			switch (menuMode)
			{
				case DeckPreviewMode.Preview:
					// Nothing?
					if (closeButton != null)
					{
						closeButton.gameObject.SetActive(true);
					}
					break;
				case DeckPreviewMode.RemoveCard:
					if (closeButton != null)
					{
						closeButton.gameObject.SetActive(false);
					}
					break;
			}
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void DismissMenu()
		{
			gameObject.SetActive(false);
			UIManager.instance.OnDeckPreviewMenuClosed();
		}

		//-///////////////////////////////////////////////////////////
		/// 
		private void InitDeck()
		{
			foreach (CardUIView cardView in cardViews)
			{
				Destroy(cardView.gameObject);
			}
			cardViews.Clear();
			foreach (CardData cardData in DeckController.instance.deckCardsData)
			{
				CardUIView view = Instantiate(cardViewPrefab, cardsContainer);
				cardViews.Add(view);
				view.SetData(cardData);
				view.cardButton.enabled = true;
				view.cardButton.onClick.AddListener((() =>
				{
					switch (menuMode)
					{
						case DeckPreviewMode.Preview:
							// Nothing
							break;
						case DeckPreviewMode.RemoveCard:
							DeckController.instance.deckCardsData.Remove(cardData);
							DismissMenu();
							break;
					}
				}));
			}
		}

	}
}