using UnityEngine;

namespace HiveRise
{
	//-///////////////////////////////////////////////////////////
	/// 
	public class PieceView : MonoBehaviour
	{
		[SerializeField] private Rigidbody2D _rigidbody2D = null;
		public Rigidbody2D rigidbody2D => _rigidbody2D;

		[SerializeField] private SpriteRenderer[] _spriteRenderer = null;
		public SpriteRenderer[] spriteRenderer => _spriteRenderer;
		
		
	}
}