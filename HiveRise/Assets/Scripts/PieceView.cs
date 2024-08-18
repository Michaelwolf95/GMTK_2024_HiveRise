﻿using System.Collections.Generic;
using UnityEngine;

namespace HiveRise
{
	//-///////////////////////////////////////////////////////////
	/// 
	public class PieceView : MonoBehaviour
	{
		[SerializeField] private Rigidbody2D _rigidbody2D = null;
		public new Rigidbody2D rigidbody2D => _rigidbody2D;

		[SerializeField] private HexCell[] _hexCells = null;
		public HexCell[] hexCells => _hexCells;

		public CardData pieceCardData { get; private set; }
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void SetPieceCardData(CardData argCardData)
		{
			pieceCardData = argCardData;

			foreach (HexCell hexCell in hexCells)
			{
				hexCell.SetColor(argCardData.color);
			}
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void SetPhysical(bool argIsPhysical)
		{
			rigidbody2D.isKinematic = !argIsPhysical;
			SetAllCollidersEnabled(argIsPhysical);
			
			// foreach (HexCell hexCell in hexCells)
			// {
			// 	//hexCell.gameObject.layer = (argIsPhysical)? 0 : 6;
			// }
		}

		//-///////////////////////////////////////////////////////////
		/// 
		public void SetAllCollidersEnabled(bool argEnabled)
		{
			//Debug.Log(argEnabled);
			foreach (HexCell hexCell in hexCells)
			{
				hexCell.SetColliderEnabled(argEnabled);
			}
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public bool DoesPieceOverlapColliders(List<Collider2D> argColliders)
		{
			foreach (HexCell cell in hexCells)
			{
				if (cell.DoesCellOverlapColliders(argColliders))
				{
					return true;
				}
			}
			return false;
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public List<Collider2D> GetAllColliders()
		{
			List<Collider2D> colliders = new List<Collider2D>();
			foreach (HexCell cell in hexCells)
			{
				colliders.Add(cell.collider);
			}
			return colliders;
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void SetValidState(bool argValid)
		{
			foreach (HexCell cell in hexCells)
			{
				cell.SetValidState(argValid);
			}
		}
		
		// //-///////////////////////////////////////////////////////////
		// /// 
		// public bool CheckForOverlap(PieceView argOtherPieceView)
		// {
		// 	
		// }
		
		// //-///////////////////////////////////////////////////////////
		// /// 
		// public float GetTopHeightOfPiece()
		// {
		// 	
		// }
	}
}