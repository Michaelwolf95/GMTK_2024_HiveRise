using System.Collections.Generic;
using UnityEngine;

namespace HiveRise
{
	//-///////////////////////////////////////////////////////////
	///
	[CreateAssetMenu(fileName="CardDefinitions", menuName="HiveRise/CardDefinitions")]
	public class CardDefinitions : ScriptableObject
	{
		public static CardDefinitions instance => GameManager.instance.cardDefinitions;
		
		[SerializeField] private CardView _cardViewPrefab = null;
		public CardView cardViewPrefab => _cardViewPrefab;

		[SerializeField] private PieceShapeData[] pieceShapeDefinitions;
		private Dictionary<int, PieceShapeData> pieceDataDict = null;
		
		[SerializeField] private CardData[] shopCards = null;
		
		
		//-///////////////////////////////////////////////////////////
		/// 
		public PieceView GetPiecePrefabForID(int argID)
		{
			PieceShapeData data = GetPieceDataForID(argID);
			if (data != null)
			{
				return data.pieceViewPrefab;
			}
			return null;
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public PieceShapeData GetPieceDataForID(int argID)
		{
			if (pieceDataDict == null)
			{
				pieceDataDict = new Dictionary<int, PieceShapeData>();
				foreach (PieceShapeData pieceData in pieceShapeDefinitions)
				{
					pieceDataDict.Add(pieceData.pieceShapeID, pieceData);
				}
			}

			return pieceDataDict[argID];
		}
	}
}