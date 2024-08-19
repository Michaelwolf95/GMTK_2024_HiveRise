using System;
using MichaelWolfGames;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HiveRise
{
	//-///////////////////////////////////////////////////////////
	///
	public class ShopMenu : MonoBehaviour
	{
		[SerializeField] private CanvasGroup canvasGroup;
		[SerializeField] private Button closeButton;
		[SerializeField] private TextMeshProUGUI currentMoneyCountLabel;
		[Space]
		[SerializeField] private Button viewDeckButton;
		[SerializeField] private Button removeCardButton;
		[SerializeField] private TextMeshProUGUI removeCardCostText;
		[SerializeField] private int removeCardCost = 10;
		[Space]
		[SerializeField] private ShopCardView[] shopCards;
		

		
		
		private Action onDismiss = null;

		//-///////////////////////////////////////////////////////////
		/// 
		private void Awake()
		{
			closeButton.onClick.AddListener(()=>
			{
				HideMenu();
			});
			
			viewDeckButton.onClick.AddListener((() =>
			{
				UIManager.instance.ShowDeckPreviewMenu(DeckPreviewMode.Preview);
			}));
			
			removeCardButton.onClick.AddListener((() =>
			{
				if (GameManager.instance.currentHoneyCount >= removeCardCost)
				{
					GameManager.instance.SpendHoney(removeCardCost);
					UIManager.instance.ShowDeckPreviewMenu(DeckPreviewMode.RemoveCard);
					removeCardButton.interactable = false;
				}
			}));
		}

		//-///////////////////////////////////////////////////////////
		/// 
		public void ShowMenu(Action argOnDismiss = null)
		{
			onDismiss = argOnDismiss;
			gameObject.SetActive(true);
			InitNewShop();
			
			removeCardButton.interactable = (GameManager.instance.currentHoneyCount >= removeCardCost);
			AudioHooks.instance.shopOpen.PlayOneShot();
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void HideMenu()
		{
			gameObject.SetActive(false);
			UIManager.instance.OnShopMenuClosed();
			onDismiss.InvokeNullCheck();
			AudioHooks.instance.shopClose.PlayOneShot();
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void InitNewShop()
		{
			removeCardButton.interactable = true;
			removeCardCostText.text = removeCardCost.ToString();
			
			foreach (ShopCardView shopCardView in shopCards)
			{
				ShopCardData data = ShopCardDefinitions.instance.GetRandomShopCardData(shopCardView.rarity);
				
				shopCardView.SetData(data);
			}
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void SetHoneyCount(int argCount)
		{
			currentMoneyCountLabel.text = argCount.ToString();
		}
	}
}