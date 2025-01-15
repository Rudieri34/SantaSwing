using Cysharp.Threading.Tasks;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedFlyingEnemy : MonoBehaviour
{
    IAstarAI ai;

    public Transform[] patrolPositions;
    public int attackDistance;
    public GameObject bullet;
    public GameObject attackPosition;
    public GameObject attackRotation;
    public LayerMask IgnoreMe;


    GameObject player;
    float distance;

    bool isShooting;
    RaycastHit2D hit;

    void Start()
    {
        ai = GetComponent<IAstarAI>();
        player = GameObject.FindGameObjectWithTag("Player");
        ai.destination = patrolPositions[Random.Range(0, patrolPositions.Length)].position;

    }

    // Update is called once per frame
    void Update()
    {

        distance = Vector2.Distance(gameObject.transform.position, player.transform.position);

        Vector3 dir = player.transform.position - attackRotation.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        attackRotation.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (distance < attackDistance)
        {
            if (!isShooting)
            {
                Shoot();
            }
        }

        if (ai.reachedDestination)
        {
            ai.destination = patrolPositions[Random.Range(0, patrolPositions.Length)].position;
        }


    }
    async void Shoot()
    {
        hit = Physics2D.Linecast(attackPosition.transform.position, player.transform.position);

        if (hit.transform.gameObject.CompareTag("Player"))
        {
            isShooting = true;
            //GetComponent<Animator>().SetTrigger("Shooting");
            GameObject a = Instantiate(bullet) as GameObject;
            Rigidbody2D rigidbodyBullet = a.GetComponent<Rigidbody2D>();
            a.transform.position = transform.position;
            var heading = player.transform.position - a.transform.position;
            var distance = heading.magnitude;
            var direction = heading / distance;
            rigidbodyBullet.velocity = direction * 10;
            await UniTask.Delay(1500);

            isShooting = false;

        }
    }
}
