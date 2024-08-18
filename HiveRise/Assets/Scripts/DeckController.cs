using System.Collections.Generic;
using MichaelWolfGames;
using UnityEngine;

namespace HiveRise
{
	//-///////////////////////////////////////////////////////////
	/// 
	public class DeckController : MonoBehaviour
	{
		public static DeckController instance => GameManager.instance.deckController;
		
		[SerializeField] private List<CardData> _initialDeckCardsData = null;
		
		public List<CardData> deckCardsData { get; private set; }
		public List<CardData> activeDeckCardsData { get; private set; }
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void InitDeckForNewRun()
		{
			deckCardsData = new List<CardData>(_initialDeckCardsData);
		}

		//-///////////////////////////////////////////////////////////
		/// 
		public void InitDeckForNewGame()
		{
			activeDeckCardsData = new List<CardData>(deckCardsData);
			activeDeckCardsData.Shuffle();
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public CardData DrawCard()
		{
			if (activeDeckCardsData != null && activeDeckCardsData.Count > 0)
			{
				CardData card = activeDeckCardsData[0];
				activeDeckCardsData.RemoveAt(0);
				return card;
			}

			return null;
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public int GetNumRemainingInDeck()
		{
			return activeDeckCardsData.Count;
		}
	}
}