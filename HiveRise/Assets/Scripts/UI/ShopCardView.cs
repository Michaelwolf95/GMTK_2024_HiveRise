using TMPro;
using UnityEngine;

namespace HiveRise
{
	//-///////////////////////////////////////////////////////////
	/// 
	public class ShopCardView : MonoBehaviour
	{
		public CardUIView cardView = null;
		public TextMeshProUGUI priceLabel = null;
		public CardRarity rarity = CardRarity.Common;

		public ShopCardData shopCardData = null;
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void SetData(ShopCardData argShopCardData)
		{
			shopCardData = argShopCardData;
			cardView.SetData(shopCardData.cardData);
			priceLabel.text = shopCardData.cost.ToString();

			cardView.canvasGroup.interactable = true;
			cardView.cardButton.enabled = true;	
			cardView.cardButton.onClick.RemoveAllListeners();
			cardView.cardButton.onClick.AddListener(TryPurchaseCard);

			cardView.cardButton.interactable = CanPurchaseCard();
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void RefreshAfterPurchase()
		{
			cardView.cardButton.interactable = CanPurchaseCard();
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		private void TryPurchaseCard()
		{
			if (CanPurchaseCard())
			{
				GameManager.instance.SpendHoney(shopCardData.cost);
				
				DeckController.instance.deckCardsData.Add(shopCardData.cardData);

				cardView.canvasGroup.interactable = false;
				
				AudioHooks.instance.shopBuy.PlayOneShot();
			}
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		private bool CanPurchaseCard()
		{
			return GameManager.instance.currentHoneyCount >= shopCardData.cost;
		}
	}
}