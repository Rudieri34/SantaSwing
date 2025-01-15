using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeItemController : MonoBehaviour
{
    public string KeyName;
    public float KeyValue;

    public bool isIncrease;
    public bool activeItem;
    public bool selfDestroy;

    public GameObject Item;

    private void Start()
    {
       // if(SaveSystem.saves[KeyName] == KeyValue && !isIncrease)
      //  {
      //      Destroy(gameObject);
      //  }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            /*
            if(isIncrease)
                SaveSystem.Change(KeyName, SaveSystem.saves[KeyName] + KeyValue);
            else
                SaveSystem.Change(KeyName, KeyValue);
            */


            if (activeItem)
                Item.SetActive(true);

           // collision.GetComponentInParent<Controle_player>().LoadSave(true);

            if (selfDestroy)
                Destroy(gameObject);
        }
    }


}
