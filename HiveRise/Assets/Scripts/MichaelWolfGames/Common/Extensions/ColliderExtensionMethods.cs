using UnityEngine;

namespace MichaelWolfGames
{
	public static class ColliderExtensionMethods
	{
		public static T GetComponentOnColliderOrRigidbody<T>(this Collider collider)
		{
			T result = collider.GetComponent<T>();
			if (result == null && collider.attachedRigidbody)
			{
				result = collider.attachedRigidbody.GetComponent<T>();
			}
			return result;
		}
	}
}