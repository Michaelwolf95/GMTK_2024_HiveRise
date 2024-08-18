using System;
using TMPro;
using UnityEngine;

namespace HiveRise
{
	//-///////////////////////////////////////////////////////////
	/// 
	[Serializable]
	public class ShopCardView
	{
		public CardUIView cardView = null;
		public TextMeshProUGUI priceLabel = null;
		public CardRarity rarity = CardRarity.Common;
	}
	
	//-///////////////////////////////////////////////////////////
	///
	public class ShopMenu : MonoBehaviour
	{
		[SerializeField] private ShopCardView[] shopCards;

		
		//-///////////////////////////////////////////////////////////
		/// 
		public void Init()
		{
			foreach (ShopCardView shopCardView in shopCards)
			{
				ShopCardData data = ShopCardDefinitions.instance.GetRandomShopCardData(shopCardView.rarity);
				
				shopCardView.cardView.SetData(data.cardData);
				shopCardView.priceLabel.text = data.cost.ToString();
				
				shopCardView.cardView.cardButton.onClick.RemoveAllListeners();
				shopCardView.cardView.cardButton.onClick.AddListener((() =>
				{
					
				}));
			}
		}
		
		//private bool CanPurchaseCard()
		
	}
}