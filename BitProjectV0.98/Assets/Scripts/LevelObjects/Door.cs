using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Door : MonoBehaviour {

    public GameObject waveManagerObject;
    WaveManager waveManager;
    GameManager gameManager;
    public bool opened;
    public GameObject[] players;
    public Transform nextDoor;
    public int doorActivationRange;


	void Start () {
        waveManager = waveManagerObject.GetComponent<WaveManager>();
        gameManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>();
        FindPlayers();
	}
	

	void LateUpdate () {
        
        if (gameManager.playersAlive > 0)
        {
            if (opened)
            {
                FindPlayers();
                foreach (GameObject player in players)
                {
                    if (Vector3.Distance(player.transform.position, transform.position) < doorActivationRange)
                    {
                        if (Input.GetAxis("VertAllP") < -0.3f)
                        {
                            foreach (GameObject aPlayer in players)
                            {
                                aPlayer.transform.position = new Vector3(nextDoor.transform.position.x, nextDoor.transform.position.y, aPlayer.transform.position.z);

                            } 
                            waveManager.ResetWaveSystem();
                        }
                    }
                }
            }
        }
	}

    public void FindPlayers()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }

}
