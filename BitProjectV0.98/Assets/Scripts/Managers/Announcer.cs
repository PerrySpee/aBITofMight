using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Announcer : MonoBehaviour
{


    public Text announceMessage;
    public Text announceMessage2;
    AudioSource audioSource;
    public AudioClip letterSound;

    private bool playingMessage1;

    GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>();
        audioSource = GetComponent<AudioSource>();

    }
    public void GetMessage(string newMessage)
    {
        StartCoroutine(AnnounceMessage(newMessage));
    }

    public IEnumerator AnnounceMessage(string message)
    {
        if (!gameManager.gameOver)
        {
            if (!playingMessage1)
            {
                playingMessage1 = true;
                announceMessage.text = "";
                foreach (char letter in message)
                {
                    announceMessage.text += letter;
                    audioSource.PlayOneShot(letterSound);

                    yield return new WaitForSeconds(0.05f);
                }

                yield return new WaitForSeconds(3.5f);

                announceMessage.text = "";
                playingMessage1 = false;
            }
            else
            {
                announceMessage2.text = "";
                foreach (char letter in message)
                {
                    announceMessage2.text += letter;
                    audioSource.PlayOneShot(letterSound);

                    yield return new WaitForSeconds(0.05f);
                }

                yield return new WaitForSeconds(3.5f);

                announceMessage2.text = "";
            }
        }
    }
}
