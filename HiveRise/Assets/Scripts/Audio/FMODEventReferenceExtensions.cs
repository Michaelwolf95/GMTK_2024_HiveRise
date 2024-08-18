using FMODUnity;
using UnityEngine;

namespace HiveRise
{
	///-///////////////////////////////////////////////////////////
	///
	public static class FMODEventReferenceExtensions
	{
		///-///////////////////////////////////////////////////////////
		///
		public static void PlayOneShot(this EventReference argEvent)
		{
			AudioManager.PlayOneShot(argEvent);
		}
		
		///-///////////////////////////////////////////////////////////
		///
		public static void PlayOneShot(this EventReference argEvent, Vector3 argPosition)
		{
			AudioManager.PlayOneShot(argEvent, argPosition);
		}
		
		///-///////////////////////////////////////////////////////////
		///
		public static void PlayOneShot(this EventReference argEvent, Vector3 argPosition, AudioEventParam argEventParam)
		{
			AudioManager.PlayOneShot(argEvent, argPosition, argEventParam);
		}
		
		///-///////////////////////////////////////////////////////////
		///
		public static void PlayOneShot(this EventReference argEvent, Vector3 argPosition, AudioEventParam[] argEventParams)
		{
			AudioManager.PlayOneShot(argEvent, argPosition, argEventParams);
		}
		
		///-///////////////////////////////////////////////////////////
		///
		public static void PlayOneShot(this EventReference argEvent, GameObject argGameObject)
		{
			AudioManager.PlayOneShot(argEvent, argGameObject);
		}
		
		///-///////////////////////////////////////////////////////////
		///
		public static void PlayOneShot(this EventReference argEvent, GameObject argGameObject, AudioEventParam argEventParam)
		{
			AudioManager.PlayOneShot(argEvent, argGameObject, argEventParam);
		}
		
		///-///////////////////////////////////////////////////////////
		///
		public static void PlayOneShot(this EventReference argEvent, GameObject argGameObject, AudioEventParam[] argEventParams)
		{
			AudioManager.PlayOneShot(argEvent, argGameObject, argEventParams);
		}
		
		///-///////////////////////////////////////////////////////////
		///
		public static void Play(this EventReference argEvent)
		{
			AudioManager.PlayEvent(argEvent);
		}

		///-///////////////////////////////////////////////////////////
		///
		public static void Play(this EventReference argEvent, GameObject gameObj, Rigidbody rb = null)
		{
			AudioManager.Set3DParameter(argEvent, gameObj, rb);
			AudioManager.PlayEvent(argEvent);
		}

		///-///////////////////////////////////////////////////////////
		///
		public static void PlayOrResume(this EventReference argEvent)
		{
			if (!AudioManager.IsPlaying(argEvent))
			{
				AudioManager.PlayEvent(argEvent);
			}
			else
			{
				AudioManager.SetPaused(argEvent, false);
			}
		}

		///-///////////////////////////////////////////////////////////
		///
		public static void PlayOrResume(this EventReference argEvent, GameObject gameObj, Rigidbody rb = null)
		{
			PlayOrResume(argEvent);
			AudioManager.Set3DParameter(argEvent, gameObj, rb);
		}

		///-///////////////////////////////////////////////////////////
		///
		public static void Pause(this EventReference argEvent)
		{
			AudioManager.SetPaused(argEvent, true);
		}

		///-///////////////////////////////////////////////////////////
		///
		public static void Stop(this EventReference argEvent)
		{
			// Looks up the event instance of the ref, and stops it.
			AudioManager.StopEvent(argEvent);
		}
		
		///-///////////////////////////////////////////////////////////
		///
		public static void Stop(this EventReference argEvent, GameObject gameObj)
		{
			// Looks up the event instance of the ref, and stops it.
			AudioManager.StopEvent(argEvent);
		}
		
		///-///////////////////////////////////////////////////////////
		///
		public static bool IsPlaying(this EventReference argEvent)
		{
			return AudioManager.IsPlaying(argEvent);
		}

		///-///////////////////////////////////////////////////////////
		///
		public static void Set3DParameter(this EventReference argEvent, GameObject gameObj, Rigidbody rb = null)
        {
			AudioManager.Set3DParameter(argEvent, gameObj, rb);
        }

		///-///////////////////////////////////////////////////////////
		///
		public static void PlaySingleSource(this EventReference argEvent, ref FMOD.Studio.EventInstance _eventInstance, GameObject gameObj)
        {
			AudioManager.PlaySingleSource(argEvent, ref _eventInstance, gameObj);
        }

		///-///////////////////////////////////////////////////////////
		///
		public static void StopSingleSource(this EventReference argEvent, ref FMOD.Studio.EventInstance _eventInstance, GameObject gameObj)
        {
			AudioManager.StopSingleSource(argEvent, ref _eventInstance, gameObj);
        }
		
		///-///////////////////////////////////////////////////////////
		///
		public static void StopSingleSource(this FMOD.Studio.EventInstance _eventInstance, GameObject gameObj = null)
		{
			AudioManager.StopSingleSource(ref _eventInstance, gameObj);
		}
		
		///-///////////////////////////////////////////////////////////
		///
		public static void ReleaseInstance(this FMOD.Studio.EventInstance _eventInstance, GameObject gameObj = null)
		{
			AudioManager.ReleaseInstance(ref _eventInstance, gameObj);
		}
	}
}