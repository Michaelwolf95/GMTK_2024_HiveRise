using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace HiveRise
{
	//-///////////////////////////////////////////////////////////
	/// 
	public class HandController : MonoBehaviour
	{
		public static HandController instance => GameManager.instance.handController;
		
		public CardView currentDragCard { get; private set; }

		[SerializeField] private List<CardView> _currentCardsInHand = new List<CardView>();
		public List<CardView> currentCardsInHand => _currentCardsInHand;

		[SerializeField] private SplineContainer handCurveSpline = null;
		
		//-///////////////////////////////////////////////////////////
		/// 
		private void Update()
		{
			UpdateCurrentDrag();
			UpdateCardHandPositions();
		}

		//-///////////////////////////////////////////////////////////
		/// 
		private void UpdateCurrentDrag()
		{
			if (currentDragCard != null)
			{
				// Update movement
				
			}
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		private void UpdateCardHandPositions()
		{
			int numCardsInHand = currentCardsInHand.Count;
			//int div = (numCardsInHand % 2 == 0) ? numCardsInHand : numCardsInHand + 1;
			for (int i = 0; i < currentCardsInHand.Count; i++)
			{
				//float t = (numCardsInHand % 2 == 0)? (((float) i) / (numCardsInHand)) : (((float) i + 1) / (numCardsInHand + 1));
				float t = (((float) i + 1) / (numCardsInHand + 1));
				float3 pos;
				float3 tangentVector;
				float3 up;
				if(handCurveSpline.Evaluate(t, out pos, out tangentVector, out up))
				{
					//Debug.Log($"{i}/{numCardsInHand}: {t}, {pos}");
					currentCardsInHand[i].transform.position = new Vector3(pos.x, pos.y, pos.z);

					Vector3 upVector = new Vector3(up.x, up.y, up.z);

					// Rotate the object by setting its rotation
					currentCardsInHand[i].transform.rotation = Quaternion.AngleAxis(Vector2.SignedAngle(Vector2.up, upVector), Vector3.forward);
				}
			}
		}

#region Dragging

		//-///////////////////////////////////////////////////////////
		/// 
		public void StartDraggingCard(CardView argCardView)
		{
			if (currentDragCard == null)
			{
				currentDragCard = argCardView;
				currentDragCard.OnStartDragging();
			}
			
		}

#endregion //Dragging

#region Debug

		//-///////////////////////////////////////////////////////////
		/// 
		private void OnDrawGizmosSelected()
		{
			Color originalGizmoColor = Gizmos.color;
			for (int i = 0; i < 10; i++)
			{
				float t = (float) i / 10f;
				float3 pos;
				float3 tangent;
				float3 up;
				if(handCurveSpline.Evaluate(t, out pos, out tangent, out up))
				{
					//Debug.Log($"{i}/{numCardsInHand}: {t}, {pos}");
					Vector3 startPos = new Vector3(pos.x, pos.y, pos.z);

					Vector3 upVector = new Vector3(up.x, up.y, up.z);
					Vector3 tangentVector = new Vector3(tangent.x, tangent.y, tangent.z);
					
					//Debug.Log(upVector);
					Gizmos.color = Color.cyan;
					Gizmos.DrawLine(startPos, startPos + upVector.normalized);
					Gizmos.color = Color.magenta;
					Gizmos.DrawLine(startPos, startPos + tangentVector.normalized);
				}
			}
			Gizmos.color = originalGizmoColor;
		}
		

#endregion
	}
}