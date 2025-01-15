using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour 
{
    public Vector2 position;
    void OnTriggerEnter2D(Collider2D col)
    {
        col.gameObject.transform.position = position;
        
    }
}
