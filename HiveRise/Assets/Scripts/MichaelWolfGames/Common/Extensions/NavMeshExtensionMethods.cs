using UnityEngine;
using UnityEngine.AI;

namespace MichaelWolfGames
{
	//-///////////////////////////////////////////////////////////
	/// 
	public static class NavMeshExtensionMethods
	{
		//-///////////////////////////////////////////////////////////
		/// 
		public static float GetPathLength(this NavMeshPath path)
		{
			float lng = 0.0f;
       
			if ((path.status != NavMeshPathStatus.PathInvalid) && (path.corners.Length > 1))
			{
				for ( int i = 1; i < path.corners.Length; ++i )
				{
					lng += Vector3.Distance(path.corners[i-1], path.corners[i] );
				}
			}
			return lng;
		}
	}
}