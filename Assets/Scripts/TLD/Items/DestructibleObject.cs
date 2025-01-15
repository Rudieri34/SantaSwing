using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{

    public GameObject destroyEffect;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Arrow" || collision.gameObject.tag == "Player")
        {
            SoundManager.Instance.PlaySFX("Glass");

            GameObject a = Instantiate(destroyEffect) as GameObject;
            a.transform.position = transform.position;
            Destroy(a, .5f);
            Destroy(gameObject);
        }
    }
}
