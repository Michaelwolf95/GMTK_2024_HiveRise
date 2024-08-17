using System;
using UnityEngine;

namespace MichaelWolfGames
{
    public class SceneSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T instance { get; protected set; }
    
        protected virtual void Awake()
        {
            if (instance != null && instance != this)
            {
                Debug.LogError(string.Format("[Duplicate SceneSingleton Error]: Duplicate SceneSingleton '{0}' found in scene! Only one should exist!", typeof(T).Name));
                return;
            }
            
            instance = this.GetComponent<T>();
        }

        protected virtual void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }
        }
    }
}