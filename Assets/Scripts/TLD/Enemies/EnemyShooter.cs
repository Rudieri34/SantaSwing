using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public int shotCadence;
    public GameObject bullet;
    Transform player;
    float shotTiming;
    public bool shoot;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (shotTiming < 0)
        {
            anim.SetTrigger("Attack");
            if (shoot)
            {


                GameObject fire = Instantiate(bullet) as GameObject;
                if (GetComponent<SpriteRenderer>().flipX == false)
                {
                    fire.transform.position = new Vector2(transform.position.x - .3f, transform.position.y);
                    Rigidbody2D rigidbodyBullet = fire.GetComponent<Rigidbody2D>();
                    fire.GetComponent<SpriteRenderer>().flipX = true;
                    rigidbodyBullet.velocity = new Vector2(-10, 0);
                }
                else
                {
                    fire.transform.position = new Vector2(transform.position.x + .3f, transform.position.y);
                    Rigidbody2D rigidbodyBullet = fire.GetComponent<Rigidbody2D>();
                    rigidbodyBullet.velocity = new Vector2(10, 0);
                }

                shotTiming = shotCadence;
            }
        }


        shotTiming -= Time.fixedDeltaTime;
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
        if (collision.gameObject.tag == "Whip")
        {
            Destroy(gameObject);
        }
    }
}
