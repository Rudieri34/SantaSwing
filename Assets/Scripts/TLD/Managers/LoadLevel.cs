using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    public Animator anim;
    public string mapName;
    public bool teleport;
    public Quests questManager;

    // Update is called once per frame
    void Update()
    {
        if (teleport)
        {
            SceneManager.LoadScene(mapName);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (questManager != null)
            {
                if (questManager.allComplete)
                {
                    anim.SetTrigger("Fade");
                }
            }
            else
            {
                anim.SetTrigger("Fade");
            }
        }
    }
}
