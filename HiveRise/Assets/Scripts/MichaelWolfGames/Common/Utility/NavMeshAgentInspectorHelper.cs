using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MichaelWolfGames
{
	public class NavMeshAgentInspectorHelper : MonoBehaviour 
	{
        //private NavMeshAgent
	    public bool UpdatePosition;
	    public bool UpdateRotation;

	    private NavMeshAgent _agent;

	    private void Awake()
	    {
            _agent = GetComponent<NavMeshAgent>();
	    }

	    private void Update()
	    {
	        UpdatePosition = _agent.updatePosition;
	        UpdateRotation = _agent.updateRotation;
	    }
	}
}