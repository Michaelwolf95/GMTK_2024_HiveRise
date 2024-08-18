using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HiveRise
{
	//-///////////////////////////////////////////////////////////
	/// 
	[Serializable]
	public class ShopCardData
	{
		public CardData cardData = null;
		public int cost = 4;
	}
	
	//-///////////////////////////////////////////////////////////
	///
	[CreateAssetMenu(fileName="ShopDefinitions", menuName="HiveRise/ShopDefinitions")]
	public class ShopCardDefinitions : ScriptableObject
	{
		public static ShopCardDefinitions instance => GameManager.instance.shopCardDefinitions;
		
		[SerializeField] private TextAsset shopDataCSV = null;
		[SerializeField] private List<ShopCardData> shopCards = null;

		//-///////////////////////////////////////////////////////////
		/// 
		public ShopCardData GetRandomShopCardData(CardRarity argRarity)
		{
			List<ShopCardData> set = shopCards.Where(x => x.cardData.rarity == argRarity).ToList();
			if (set.Count <= 0 && argRarity != CardRarity.Common)
			{
				return GetRandomShopCardData(CardRarity.Common);
			}
			return set[UnityEngine.Random.Range(0, set.Count)];
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		[ContextMenu("Generate Data from CSV")]
		private void GenerateDataFromCSV()
		{
			if (shopDataCSV == null)
			{
				return;
			}

			shopCards = new List<ShopCardData>();

			// ToDo: Parse cards.

			string[] data = shopDataCSV.text.Split(new string[] {"\n"}, StringSplitOptions.None);

			for (int i = 0; i < data.Length; i++)
			{
				string[] dataEntries = data[i].Split(new string[] {","}, StringSplitOptions.None);
				
				ShopCardData shopCardData = new ShopCardData();
				shopCardData.cardData = new CardData();

				shopCardData.cost = int.Parse(dataEntries[0]);
				shopCardData.cardData.pieceShapeID = int.Parse(dataEntries[1]);
				shopCardData.cardData.rarity = (CardRarity)int.Parse(dataEntries[2]);
				shopCardData.cardData.color = (PieceColor)int.Parse(dataEntries[3]);
				shopCardData.cardData.weightType = (WeightType)int.Parse(dataEntries[4]);
				shopCardData.cardData.scoreMultiplier = (ScoreMultiplier)int.Parse(dataEntries[5]);
				shopCardData.cardData.sticky = int.Parse(dataEntries[6]) == 1;
				
				shopCards.Add(shopCardData);
			}

		}
	}
}