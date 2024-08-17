using System;
using UnityEngine;

namespace HiveRise
{
	//-///////////////////////////////////////////////////////////
	/// 
	public class HexCell : MonoBehaviour
	{
		[SerializeField] private PolygonCollider2D _collider = null;
		public PolygonCollider2D collider => _collider;
		
		[SerializeField] private SpriteRenderer _outlineSpriteRenderer = null;
		public SpriteRenderer outlineSpriteRenderer => _outlineSpriteRenderer;
		
		[SerializeField] private SpriteRenderer _mainSpriteRenderer = null;
		public SpriteRenderer mainSpriteRenderer => _mainSpriteRenderer;
		
		[SerializeField] private Transform _gimbal = null;

		//-///////////////////////////////////////////////////////////
		/// 
		private void Update()
		{
			_gimbal.transform.rotation = Quaternion.identity;
		}

		//-///////////////////////////////////////////////////////////
		/// 
		public void SetColliderEnabled(bool argEnabled)
		{
			collider.enabled = argEnabled;
		}
	}
}