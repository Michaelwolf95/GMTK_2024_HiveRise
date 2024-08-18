using System;

namespace HiveRise
{
	//-///////////////////////////////////////////////////////////
	/// 
	[Serializable]
	public enum CardRarity
	{
		Common = 0,
		Uncommon = 1,
		Rare = 2,
	}
	
	//-///////////////////////////////////////////////////////////
	/// 
	[Serializable]
	public enum PieceColor
	{
		Yellow = 0,
		Blue = 1,
		Pink = 2,
		Purple = 3,
	}
	
	//-///////////////////////////////////////////////////////////
	/// 
	[Serializable]
	public enum WeightType
	{
		Normal = 0,
		Heavy = 1,
		Weight = 2,
	}
	
	//-///////////////////////////////////////////////////////////
	/// 
	[Serializable]
	public class CardData
	{
		public int pieceShapeID = 0;
		public CardRarity rarity;
		public PieceColor color;
		public WeightType weightType;
		public bool sticky;

		public PieceShapeData pieceData => CardDefinitions.instance.GetPieceDataForID(pieceShapeID);
	}
}