
using TMPro;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;

public class CutsceneTextOnly : MonoBehaviour
{
    public string CutsceneName;
    public GameObject Dialog;
    public TextMeshProUGUI lore;
    public int index = 0;
    public string[] dialogues;


    Controle_player player;
    bool playing;

    CancellationTokenSource _cancellationTokenSource;
    // Start is called before the first frame update

    private void Start()
    {
        _cancellationTokenSource = new CancellationTokenSource();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player = collision.gameObject.GetComponent<Controle_player>();

            if (!Dialog.activeInHierarchy)
                Dialog.SetActive(true);

            CutscenePlay(dialogues[index]);

        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.pause = true;
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Interaction"))
        {
            if (index >= dialogues.Length)
            {
                player.pause = false;
                Destroy(this.gameObject);
                return;

            }
            CutscenePlay(dialogues[index]);
        }
    }

    public async void CutscenePlay(string currentPhrase)
    {

        

        if (!playing)
        {
            playing = true;
            string currentText = "";
            for (int i = 0; i <= currentPhrase.Length; i++)
            {
                currentText = currentPhrase.Substring(0, i);
                lore.text = currentText;
                await UniTask.Delay(50, cancellationToken: _cancellationTokenSource.Token);
                //FindObjectOfType<AudioManager>().Play("KeyPress");
            }
            playing = false;
        }
        else
        {
            playing = false;

            _cancellationTokenSource.Cancel(false);
            _cancellationTokenSource = new CancellationTokenSource();
            lore.text = currentPhrase;

        }
        index++;
        
    }

}
