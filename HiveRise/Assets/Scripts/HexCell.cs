﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace HiveRise
{
	//-///////////////////////////////////////////////////////////
	/// 
	[Serializable]
	public class HexCellColorConfig
	{
		public Sprite mainSprite;
		public Color outlineColor;
	}
	
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

		[SerializeField] private HexCellColorConfig[] colorConfigs = null;

		//-///////////////////////////////////////////////////////////
		/// 
		public void SetColor(PieceColor argPieceColor)
		{
			HexCellColorConfig config =colorConfigs[(int)argPieceColor];
			mainSpriteRenderer.sprite = config.mainSprite;
			outlineSpriteRenderer.color = config.outlineColor;
		}
		
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