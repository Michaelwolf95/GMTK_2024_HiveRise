using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace HiveRise
{
	///-///////////////////////////////////////////////////////////
	///
	[CreateAssetMenu(fileName = "AudioHooks", menuName = "HiveRise/Audio Hooks", order = 0)]
	public class AudioHooks : ScriptableObject
	{
		private static AudioHooks instance => GameManager.instance.audioHooks;
		
		public EventReference cardDraw;
		public EventReference cardPlace;
		public EventReference cardDiscard;
		public EventReference cardShuffle;
		[Space]
		public EventReference pieceBump;
		public EventReference pieceStick;
		public EventReference piecePaint;
		public EventReference pieceRotate;
		[Space]
		public EventReference shopOpen;
		public EventReference shopBuy;
		public EventReference moneyPayout;
		[Space]
		public EventReference winGame;
		public EventReference loseGame;
		//public EventReference loseGame;

	}
}