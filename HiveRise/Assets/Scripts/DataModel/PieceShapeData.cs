using System;
using UnityEngine;

namespace HiveRise
{
	
	//-///////////////////////////////////////////////////////////
	///
	[Serializable]
	public class PieceShapeData
	{
		public string pieceName = "";
		public int pieceShapeID = 0;
		public Sprite pieceShapeSprite;
		public PieceView pieceViewPrefab;
	}
}