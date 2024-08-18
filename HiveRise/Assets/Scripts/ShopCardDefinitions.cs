using System;
using System.Collections.Generic;
using UnityEngine;

namespace HiveRise
{
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
		[ContextMenu("Generate Data from CSV")]
		private void GenerateDataFromCSV()
		{
			if (shopDataCSV == null)
			{
				return;
			}

			shopCards = new List<ShopCardData>();

			// ToDo: Parse cards.

		}
	}
}