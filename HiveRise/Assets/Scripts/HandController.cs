using System;
using System.Collections.Generic;
using UnityEngine;

namespace HiveRise
{
	//-///////////////////////////////////////////////////////////
	/// 
	public class HandController : MonoBehaviour
	{
		private CardView currentDragCard = null;

		[SerializeField] // ToDo: Make this unserialized later?
		private List<CardView> currentCardsInHand = new List<CardView>();

		//-///////////////////////////////////////////////////////////
		/// 
		private void Update()
		{
			UpdateCurrentDrag();
			
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
			
		}
	}
}