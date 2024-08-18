using System;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace WaxHeart
{
	//-///////////////////////////////////////////////////////////
	/// 
	public class AudioEventParam
	{
		public string paramName;
		public float paramValue;

		public AudioEventParam(string argParamName, float argParamValue)
		{
			paramName = argParamName;
			paramValue = argParamValue;
		}
	}
	
	
	///-///////////////////////////////////////////////////////////
	///
	public static class AudioManager
	{
		private static Dictionary<EventReference, EventInstance> eventInstanceDict = new Dictionary<EventReference, EventInstance>();
		private static Dictionary<string, PARAMETER_ID> globalParamIDDict = new Dictionary<string, PARAMETER_ID>();

		private const string MASTER_BUS = "bus:/MASTER";
		private const string MUSIC_BUS = "bus:/MASTER/OUTOFGAME/MUS";
		private const string SFX_BUS = "bus:/MASTER/INGAME/SFX";
		
		///-///////////////////////////////////////////////////////////
		///
		public static void PlayOneShot(EventReference eventRef, GameObject gameObject)
		{
			if (!eventRef.IsNull)
			{
				RuntimeManager.PlayOneShotAttached(eventRef.Guid, gameObject);
			}
		}

		///-///////////////////////////////////////////////////////////
		///
		public static void PlayOneShot(EventReference eventRef, GameObject gameObject, AudioEventParam eventParam)
		{
			PlayOneShot(eventRef, gameObject, new []{eventParam});
		}
		
		///-///////////////////////////////////////////////////////////
		///
		public static void PlayOneShot(EventReference eventRef, GameObject gameObject, AudioEventParam[] eventParams)
		{
			if (!eventRef.IsNull)
			{
				EventInstance instance = FMODUnity.RuntimeManager.CreateInstance(eventRef);
				RuntimeManager.AttachInstanceToGameObject(instance, gameObject.transform, gameObject.GetComponent<Rigidbody>());
				foreach (AudioEventParam eventParam in eventParams)
				{
					instance.setParameterByName(eventParam.paramName, eventParam.paramValue);
				}
				instance.start();
				instance.release();
			}
		}
		
		///-///////////////////////////////////////////////////////////
		///
		public static void PlayOneShot(EventReference eventRef, Vector3 position)
		{
			if (!eventRef.IsNull)
			{
				RuntimeManager.PlayOneShot(eventRef.Guid, position);
			}
		}

		///-///////////////////////////////////////////////////////////
		///
		public static void PlayOneShot(EventReference eventRef, Vector3 position, AudioEventParam eventParam)
		{
			PlayOneShot(eventRef, position, new []{eventParam});
		}
		
		///-///////////////////////////////////////////////////////////
		///
		public static void PlayOneShot(EventReference eventRef, Vector3 position, AudioEventParam[] eventParams)
		{
			if (!eventRef.IsNull)
			{
				//RuntimeManager.PlayOneShot(eventRef.Guid, position);
				
				EventInstance instance = FMODUnity.RuntimeManager.CreateInstance(eventRef);
				instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(position));
				foreach (AudioEventParam eventParam in eventParams)
				{
					instance.setParameterByName(eventParam.paramName, eventParam.paramValue);
				}
				instance.start();
				instance.release();
			}
		}

		///-///////////////////////////////////////////////////////////
		///
		public static void PlayOneShot(EventReference eventRef)
		{
			if (!eventRef.IsNull)
			{
				RuntimeManager.PlayOneShot(eventRef.Guid);
			}
		}

		///-///////////////////////////////////////////////////////////
		///
		public static void PlayEvent(EventReference eventRef)
		{
			if (eventRef.IsNull) return;
			if (eventInstanceDict.ContainsKey(eventRef) == false)
			{
				eventInstanceDict[eventRef] = RuntimeManager.CreateInstance(eventRef);
			}

			try
			{
				eventInstanceDict[eventRef].start();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}

		///-///////////////////////////////////////////////////////////
		///
		public static bool IsPlaying(EventReference eventRef)
		{
			if (eventRef.IsNull) return false;
			if (eventInstanceDict.ContainsKey(eventRef))
			{
				eventInstanceDict[eventRef].getPlaybackState(out PLAYBACK_STATE state);
				if (state != PLAYBACK_STATE.STOPPED)
                {
					return true;
				}
			}
			return false;
		}

		///-///////////////////////////////////////////////////////////
		///
		public static void SetPaused(EventReference eventRef, bool argPaused)
		{
			if (eventRef.IsNull) return;
			if (eventInstanceDict.ContainsKey(eventRef))
			{
				eventInstanceDict[eventRef].setPaused(argPaused);
			}
		}

		///-///////////////////////////////////////////////////////////
		///
		public static void StopEvent(EventReference eventRef)
		{
			if (eventRef.IsNull) return;
			if (eventInstanceDict.ContainsKey(eventRef))
			{
				eventInstanceDict[eventRef].stop(STOP_MODE.ALLOWFADEOUT);
			}
		}

		///-///////////////////////////////////////////////////////////
		///
		public static void SetEventParameter(EventReference eventRef, string paramName, float value)
		{
			if (!eventInstanceDict.ContainsKey(eventRef))
			{
				eventInstanceDict[eventRef] = RuntimeManager.CreateInstance(eventRef);
			}

			eventInstanceDict[eventRef].setParameterByName(paramName, value);

		}

		///-///////////////////////////////////////////////////////////
		///
		public static void SetGlobalParameter(string paramName, float value)
		{
			if (globalParamIDDict.ContainsKey(paramName) == false)
			{
				RuntimeManager.StudioSystem.getParameterDescriptionByName(paramName, out var parameterDescription);
				globalParamIDDict.Add(paramName, parameterDescription.id);
			}

			FMODUnity.RuntimeManager.StudioSystem.setParameterByID(globalParamIDDict[paramName], value);
		}

		///-///////////////////////////////////////////////////////////
		/// Sets the event instance's position and velocity based on a gameobject
		///
		public static void Set3DParameter(EventReference eventRef, GameObject gameObj, Rigidbody rb = null)
		{
			if (eventRef.IsNull) return;
			if (eventInstanceDict.ContainsKey(eventRef) == false)
			{
				eventInstanceDict[eventRef] = RuntimeManager.CreateInstance(eventRef);
			}
			eventInstanceDict[eventRef].set3DAttributes(RuntimeUtils.To3DAttributes(gameObj, rb));
			RuntimeManager.AttachInstanceToGameObject(eventInstanceDict[eventRef], gameObj.transform);
		}

		///-//////////////////////////////////////////////////////////////////////
		///
		/// Sets bus volumes to values saved in PlayerGameSettings
		///
		public static void LoadVolumeSettings()
        {
			// SetMasterVolume(PlayerGameSettings.masterVolume);
			// SetMusicVolume(PlayerGameSettings.musicVolume);
			// SetSFXVolume(PlayerGameSettings.sfxVolume);
		}

		///-///////////////////////////////////////////////////////////
		///
		public static float GetMasterVolume()
        {
			try
            {
				float masterVolume;
				RuntimeManager.GetBus(MASTER_BUS).getVolume(out masterVolume);

				return masterVolume;
            }
			catch (BusNotFoundException)
			{
				Debug.LogWarning($"Attempt to get Master Bus failed with path: '{MASTER_BUS}'");
			}

			return 0f;
		}

		///-///////////////////////////////////////////////////////////
		///
		public static float GetMusicVolume()
		{
			try
			{
				float musicVolume;
				RuntimeManager.GetBus(MUSIC_BUS).getVolume(out musicVolume);

				return musicVolume;
			}
			catch (BusNotFoundException)
			{
				Debug.LogWarning($"Attempt to get Music Bus failed with path: '{MUSIC_BUS}'");
			}

			return 0f;
		}

		///-///////////////////////////////////////////////////////////
		///
		public static float GetSFXVolume()
		{
			try
			{
				float sfxVolume;
				RuntimeManager.GetBus(MASTER_BUS).getVolume(out sfxVolume);

				return sfxVolume;
			}
			catch (BusNotFoundException)
			{
				Debug.LogWarning($"Attempt to get SFX Bus failed with path: '{SFX_BUS}'");
			}

			return 0f;
		}

		///-///////////////////////////////////////////////////////////
		///
		public static void SetMasterVolume(float argValue)
        {
			try
            {
				RuntimeManager.GetBus(MASTER_BUS).setVolume(argValue);
			}
			catch (BusNotFoundException)
            {
				Debug.LogWarning($"Attempt to get Master Bus failed with path: '{MASTER_BUS}'");
            }
        }

		///-///////////////////////////////////////////////////////////
		///
		public static void SetMusicVolume(float argValue)
		{
			try
			{
				RuntimeManager.GetBus(MUSIC_BUS).setVolume(argValue);
			}
			catch (BusNotFoundException)
			{
				Debug.LogWarning($"Attempt to get Music Bus failed with path: '{MUSIC_BUS}'");
			}
		}

		///-///////////////////////////////////////////////////////////
		///
		public static void SetSFXVolume(float argValue)
		{
			try
			{
				RuntimeManager.GetBus(SFX_BUS).setVolume(argValue);
			}
			catch (BusNotFoundException)
			{
				Debug.LogWarning($"Attempt to get SFX Bus failed with path: '{SFX_BUS}'");
			}
		}

		///-///////////////////////////////////////////////////////////
		///
		public static void PlaySingleSource(EventReference eventRef, ref EventInstance _eventInstance, GameObject gameObj)
        {
			if (eventRef.IsNull) return;
			_eventInstance = RuntimeManager.CreateInstance(eventRef);
			//_eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObj));
			RuntimeManager.AttachInstanceToGameObject(_eventInstance, gameObj.transform);
			_eventInstance.start();
        }

		///-///////////////////////////////////////////////////////////
		///
		public static void StopSingleSource(EventReference eventRef, ref EventInstance _eventInstance, GameObject gameObj)
        {
			if (eventRef.IsNull) return;
			StopSingleSource(ref _eventInstance, gameObj);
        }
		
		///-///////////////////////////////////////////////////////////
		///
		public static void StopSingleSource(ref EventInstance _eventInstance, GameObject gameObj = null)
		{
			if (_eventInstance.isValid() == false) return;
			
			RuntimeManager.DetachInstanceFromGameObject(_eventInstance);
			_eventInstance.stop(STOP_MODE.ALLOWFADEOUT);
			_eventInstance.release();
			_eventInstance.clearHandle();
			
		}
		
		///-///////////////////////////////////////////////////////////
		///
		public static void ReleaseInstance(ref EventInstance _eventInstance, GameObject gameObj = null)
		{
			if (_eventInstance.isValid() == false) return;
			
			RuntimeManager.DetachInstanceFromGameObject(_eventInstance);
			_eventInstance.stop(STOP_MODE.IMMEDIATE);
			_eventInstance.release();
			_eventInstance.clearHandle();
		}

		///-///////////////////////////////////////////////////////////
		///
		public static void StopAllMusic()
        {
			try
			{
				RuntimeManager.GetBus(MUSIC_BUS).stopAllEvents(STOP_MODE.ALLOWFADEOUT);
			}
			catch (BusNotFoundException)
			{
				Debug.LogWarning($"Attempt to get Music Bus failed with path: '{MUSIC_BUS}'");
			}
		}
	}
}