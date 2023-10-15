using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Customer : MonoBehaviour
{
    // Debug
    [SerializeField] Transform targetToMove;

    // NavMesh Agent
    [field: SerializeField] NavMeshAgent Agent { get; set; }

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        Agent.SetDestination(targetToMove.position);   
    }
}
