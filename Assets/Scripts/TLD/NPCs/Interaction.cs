using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interaction : MonoBehaviour
{
    public Controle_player player;
    public string interactionString;
    public GameObject phraseObject;
    public Text interactionText;
    public Text phrasesText;
    public string[] phrases;
    public int id;
    public bool interactable;
    public bool onTrigger;
    public Quests questManager;
    public string questToLiberate;

    // Start is called before the first frame update
    void Start()
    {
        interactionText.text = interactionString;
        id = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (onTrigger && !phraseObject.activeSelf)
        {
            if (Input.GetButtonDown("Interaction"))
            {
                phraseObject.SetActive(true);
            }
        }

        if (phraseObject.activeSelf)
        {
            interactionText.gameObject.SetActive(false);
            if (Input.GetButtonDown("Interaction"))
            {
                if (id < phrases.Length - 1)
                {
                    id++;
                    phrasesText.text = phrases[id];
                }
                else
                {
                    if (questToLiberate != null)
                    {
                        questManager.CompleteQuest(questToLiberate);
                    }
                    id = -1;
                    phraseObject.SetActive(false);
                }
                
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            id = -1;
            interactionText.gameObject.SetActive(true);
            onTrigger = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            phraseObject.SetActive(false);
            interactionText.gameObject.SetActive(false);
            onTrigger = false;
        }
    }
}
