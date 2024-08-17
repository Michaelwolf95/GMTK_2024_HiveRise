using System;
using UnityEngine;

namespace HiveRise
{
	//-///////////////////////////////////////////////////////////
	/// 
	public class HexCell : MonoBehaviour
	{
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
	}
}