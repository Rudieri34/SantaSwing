using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    float moveSpeed = 1f;
    public Animator anim;
    public Rigidbody2D rigidBody2d;
    public float maxSpeed;
    public int persuingBoost;
    public int followDistance;
    public int attackDistance;

    bool isPatrol;
    bool isPersuing;
    bool isWaiting;



    float originalMaxSpeed;
    float distance;
    GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        originalMaxSpeed = maxSpeed;
        player = GameObject.FindGameObjectWithTag("Player");
        anim.SetBool("Running", true);
        isPatrol = true;

    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(gameObject.transform.position, player.transform.position);


        if (distance < followDistance &&!isWaiting)
        {
            if (!isPersuing)
            {
                MoveToPos(player.transform.position.x);
                isPatrol = false;
                isPersuing = true;
                maxSpeed += persuingBoost;
            }
        }
        else
        {
            isPersuing = false;
            maxSpeed = originalMaxSpeed;

            isPatrol = true;
        }


        if (!isWaiting)
            rigidBody2d.velocity = new Vector2(maxSpeed * moveSpeed, rigidBody2d.velocity.y);


    }

    void MoveToPos(float pos)
    {
        if (!isPatrol)
        {
            if (pos > transform.position.x)
            {
                 GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;

                if (moveSpeed < 0)
                    moveSpeed = moveSpeed * -1;
            }
            else
            {
                GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
                moveSpeed = moveSpeed * -1;
                if (moveSpeed > 0)
                    moveSpeed = moveSpeed * -1;
            }
        }

    }

    private async void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PatrolLimit" && isPatrol)
        {
            rigidBody2d.velocity = new Vector2(0, rigidBody2d.velocity.y);
            isWaiting = true;
            await UniTask.Delay(500);
            GetComponentInChildren<SpriteRenderer>().flipX = !GetComponentInChildren<SpriteRenderer>().flipX;
            moveSpeed = moveSpeed * -1;
            isWaiting = false;
        }
        if (collision.gameObject.tag == "PatrolMaxLimit")
        {
            rigidBody2d.velocity = new Vector2(0, rigidBody2d.velocity.y);
            isWaiting = true;
            await UniTask.Delay(500);
            GetComponentInChildren<SpriteRenderer>().flipX = !GetComponentInChildren<SpriteRenderer>().flipX;
            moveSpeed = moveSpeed * -1;
            isWaiting = false;

            await UniTask.Delay(2000);
            isPersuing = false;
            maxSpeed = originalMaxSpeed;

        }
    }

}
