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
		Any = 10,
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
	public enum ScoreMultiplier
	{
		Normal = 0,
		Double = 1,
		Tripple = 2,
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
		public ScoreMultiplier scoreMultiplier;
		public bool sticky;

		public PieceShapeData pieceData => CardDefinitions.instance.GetPieceDataForID(pieceShapeID);

		//-///////////////////////////////////////////////////////////
		/// 
		public string GetCardDescriptionForData()
		{
			return color.ToString() + " " + pieceData.pieceName;
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public int GetScoringValue()
		{
			int value = pieceData.shapePointValue;
			switch (scoreMultiplier)
			{
				case ScoreMultiplier.Double:
					value *= 2;
					break;
				case ScoreMultiplier.Tripple:
					value *= 3;
					break;
			}

			return value;
		}
	}
}