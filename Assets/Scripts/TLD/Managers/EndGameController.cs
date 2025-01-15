using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameController : MonoBehaviour
{
    public GameObject[] Gifts;
    public GameObject EndCanvas;
    Controle_player player;


    private async void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player = collision.gameObject.GetComponent<Controle_player>();

            player.pause = true;

            foreach(GameObject gb in Gifts){
                gb.transform.DOScale(new Vector3(1, 1, 1), 1);
                await UniTask.Delay(500);

            }

            EndCanvas.SetActive(true);
        }
    }
}
