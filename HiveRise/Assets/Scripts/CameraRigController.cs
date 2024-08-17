using MichaelWolfGames;
using UnityEngine;

namespace HiveRise
{
	//-///////////////////////////////////////////////////////////
	/// 
	public class CameraRigController : SceneSingleton<CameraRigController>
	{
		[SerializeField] private Camera _mainCamera = null;
		public Camera mainCamera => _mainCamera;
		
		
	}
}