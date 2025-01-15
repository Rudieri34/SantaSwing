using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    float moveSpeed = 2f;
    public Animator anim;
    public Rigidbody2D rigidbody;
    public float maxSpeed;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        rigidbody.velocity = new Vector2(maxSpeed * moveSpeed, rigidbody.velocity.y);
        
        anim.SetBool("Running", true);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Arrow")
        {
            Rigidbody2D rigidbodyBullet = collision.gameObject.GetComponent<Rigidbody2D>();
            Destroy(gameObject);
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
            moveSpeed = moveSpeed * -1;
        }
        if (collision.gameObject.tag == "Whip")
        {
            Destroy(gameObject);
        }
    }
}
