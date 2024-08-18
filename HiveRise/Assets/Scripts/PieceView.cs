using System;
using System.Collections.Generic;
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

		private List<Joint2D> currentStickJoints = new List<Joint2D>();
		private List<PieceView> currentAttachedPieces = new List<PieceView>();
		
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
		
		//-///////////////////////////////////////////////////////////
		/// 
		public HashSet<PieceView> GetNearbyPieces()
		{
			HashSet<PieceView> uniquePieces = new HashSet<PieceView>();
			foreach (HexCell cell in hexCells)
			{
				Collider2D[] colliders = Physics2D.OverlapCircleAll(cell.transform.position, 0.5f);
				foreach (Collider2D c in colliders)
				{
					if (c.attachedRigidbody != null)
					{
						var piece = c.attachedRigidbody.GetComponent<PieceView>();
						if (piece != null && piece != this)
						{
							uniquePieces.Add(piece);
						}
					}
				}
			}

			return uniquePieces;
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void TryStickToPiece(PieceView argPieceView)
		{
			if (argPieceView != this && currentAttachedPieces.Contains(argPieceView) == false)
			{
				// Stick!
				FixedJoint2D joint = this.gameObject.AddComponent<FixedJoint2D>();
				currentStickJoints.Add(joint);
				joint.connectedBody = argPieceView.rigidbody2D;
				
				AddAttachedPiece(argPieceView);
				argPieceView.AddAttachedPiece(this);
			}
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void AddAttachedPiece(PieceView argPieceView)
		{
			currentAttachedPieces.Add(argPieceView);
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