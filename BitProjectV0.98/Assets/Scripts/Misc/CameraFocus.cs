using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraFocus : MonoBehaviour 
{

    public GameObject[] players;
    public Vector3[] playerPositions;
    GameManager gameManager;

    float newXPos;
    float newYPos;


    void Start()
    {
        gameManager = GetComponent<GameManager>();
        players = GameObject.FindGameObjectsWithTag("Player");
        playerPositions = new Vector3[players.Length];
	}
	

	void Update () {
        FocusAllPlayers();
	}

    public void ResetPlayers()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        playerPositions = new Vector3[players.Length];
    }

    private void FocusAllPlayers()
    {
        if (gameManager.playersAlive > 0)
        {

            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] == null)
                {
                    ResetPlayers();
                    return;
                }
                playerPositions[i] = players[i].transform.position;
            }
            for (int i = 0; i < playerPositions.Length; i++)
            {
                newXPos = newXPos + playerPositions[i].x;
                newYPos = newYPos + playerPositions[i].y;
            }

            newXPos = newXPos / playerPositions.Length;
            newYPos = newYPos / playerPositions.Length;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(newXPos, newYPos, -10), 100 * Time.deltaTime);

            newXPos = 0;
            newYPos = 0;
        }
    }
}
