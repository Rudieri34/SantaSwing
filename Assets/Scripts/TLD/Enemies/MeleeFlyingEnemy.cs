using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeFlyingEnemy : MonoBehaviour
{
    IAstarAI ai;

    public Transform[] patrolPositions;
    public int followDistance;
    public int attackDistance;

    GameObject player;
    float distance;

    bool isPatrol;
    bool isPersuing;

    void Start()
    {
        ai = GetComponent<IAstarAI>();
        player = GameObject.FindGameObjectWithTag("Player");
        OnDetinationReached();
    }

    // Update is called once per frame
    void Update()
    {

        distance = Vector2.Distance(gameObject.transform.position, player.transform.position);


        if (distance < followDistance)
        {
            if (!isPersuing)
            {
                ai.destination = player.transform.position;
                isPatrol = false;
                isPersuing = true;
                ai.maxSpeed = 10;
            }
        }

        if (ai.reachedDestination)
        {
            OnDetinationReached();
        }

    }



    void OnDetinationReached()
    {
        if (isPatrol)
            isPersuing = false;
        else
            isPatrol = true;

        ai.destination = patrolPositions[Random.Range(0, patrolPositions.Length)].position;

    }
}
