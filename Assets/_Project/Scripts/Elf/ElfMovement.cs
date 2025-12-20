using System;
using NavMeshPlus.Extensions;
using UnityEngine;
using UnityEngine.AI;

public class ElfMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform player;
    private NavMeshAgent agent;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Update()
    {
        if (!agent.enabled || !PlayerManager.Instance) return;
        
        player = PlayerManager.Instance.transform;
        
        agent.SetDestination(player.position);
    }
}
