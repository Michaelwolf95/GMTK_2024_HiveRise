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
		[SerializeField] private ShopCardView[] shopCards;
		

		private Action onDismiss = null;

		//-///////////////////////////////////////////////////////////
		/// 
		private void Awake()
		{
			closeButton.onClick.AddListener(HideMenu);
		}

		//-///////////////////////////////////////////////////////////
		/// 
		public void ShowMenu(Action argOnDismiss = null)
		{
			onDismiss = argOnDismiss;
			gameObject.SetActive(true);
			InitNewShop();
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void HideMenu()
		{
			gameObject.SetActive(false);
			
			onDismiss.InvokeNullCheck();
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void InitNewShop()
		{
			foreach (ShopCardView shopCardView in shopCards)
			{
				ShopCardData data = ShopCardDefinitions.instance.GetRandomShopCardData(shopCardView.rarity);
				
				shopCardView.SetData(data);
			}
		}
		
	}
}