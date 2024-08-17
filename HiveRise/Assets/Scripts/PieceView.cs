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
		
		
		
	}
}