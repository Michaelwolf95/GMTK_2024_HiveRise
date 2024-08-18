using System;
using System.Collections.Generic;
using UnityEngine;

namespace HiveRise
{
	//-///////////////////////////////////////////////////////////
	/// 
	public class HexCell : MonoBehaviour
	{
		[SerializeField] private PolygonCollider2D _collider = null;
		public new PolygonCollider2D collider => _collider;
		
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
		
		//-///////////////////////////////////////////////////////////
		/// 
		public bool DoesCellOverlapColliders(List<Collider2D> argOtherColliders)
		{
			bool result = false;

			bool wasColliderEnabled = collider.enabled;
			collider.enabled = true;
			List<Collider2D> overlapResults = new List<Collider2D>();
			if (collider.Overlap(overlapResults) > 0)
			{
				foreach (Collider2D otherCollider in argOtherColliders)
				{
					if (overlapResults.Contains(otherCollider))
					{
						result = true;
					}
				}
			}

			collider.enabled = wasColliderEnabled;
			return result;
		}
	}
}