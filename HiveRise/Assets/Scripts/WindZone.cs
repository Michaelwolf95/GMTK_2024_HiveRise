using System;
using System.Collections.Generic;
using UnityEngine;

namespace HiveRise
{
	//-//////////////////////////////////////////////////////////////////////
	///
	public class WindZone : MonoBehaviour
	{
		[SerializeField] private BoxCollider2D triggerBox = null;
		[SerializeField] private SpriteRenderer triggerSprite = null;
		
		[SerializeField] private Vector2 _windForceVector = new Vector2(1f, 0f);
		public Vector2 windForceVector => _windForceVector;

		private Collider2D[] overlapColliders = new Collider2D[99];
		private ContactFilter2D contactFilter = new ContactFilter2D();

		//-//////////////////////////////////////////////////////////////////////
		///
		private void Awake()
		{
			SetHeight(triggerBox.size.y);
		}

		//-//////////////////////////////////////////////////////////////////////
		///
		public void SetHeight(float argHeight)
		{
			triggerBox.size = new Vector2(triggerBox.size.x, argHeight);
			triggerBox.offset = new Vector2(0f, argHeight / 2f);

			triggerSprite.transform.localPosition = new Vector3(0f, argHeight / 2f, triggerSprite.transform.localPosition.z);
			triggerSprite.size = triggerBox.size;
		}
		

		//-//////////////////////////////////////////////////////////////////////
		///
		private void FixedUpdate()
		{
			int numColliders = Physics2D.OverlapArea(triggerBox.bounds.min, triggerBox.bounds.max, contactFilter, overlapColliders);
			if (numColliders > 0)
			{
				//HashSet<Rigidbody2D> uniqueRigidbodies = new HashSet<Rigidbody2D>();
				for (int i = 0; i < numColliders; i++)
				{
					
					//uniqueRigidbodies.Add(overlapColliders[i].attachedRigidbody);

					HexCell hexCell = overlapColliders[i].gameObject.GetComponentInParent<HexCell>();
					if (hexCell != null)
					{
						hexCell.collider.attachedRigidbody.AddForceAtPosition(windForceVector, hexCell.transform.position, ForceMode2D.Force);
					}
				}

				// foreach (Rigidbody2D rb in uniqueRigidbodies)
				// {
				// 	if (rb != null)
				// 	{
				// 		
				// 	}
				// }
			}
		}
	}
}