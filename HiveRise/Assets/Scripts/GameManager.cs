using System;
using System.Collections;
using System.Collections.Generic;
using MichaelWolfGames;
using UnityEngine;

namespace HiveRise
{
	//-///////////////////////////////////////////////////////////
	/// 
	public class GameManager : SceneSingleton<GameManager>
	{
		[SerializeField] private HandController _handController = null;
		public HandController handController => _handController;

		//-///////////////////////////////////////////////////////////
		/// 
		protected override void Awake()
		{
			base.Awake();
		}
	}
}