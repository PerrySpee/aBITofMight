using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject knight;

    private float seconds;
    private float minutes;

    public int EnemiesSlain;
    private float gametime;
    private string displayTime;

    public Text timeText;
    public Text enemiesSlain;

    public GameObject[] PlayersUI;
    public Text[] playerStartUI;
    public GameObject[] Players;
    [HideInInspector]public int playersAlive;

    public GameObject[] Enemies;
    public GameObject[] doors;

    public Text pausedText;
    public GameObject gameOverText;

    CameraFocus camFocus;

    private bool paused;
    public bool gameOver;
    private AudioSource audioSource;

    public float gameOverTimer;

    public AudioClip pauseMusic, gameMusic;


    void Awake()
    {
        ReloadUI();
    }

    void Start()
    {
        audioSource = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>();
        camFocus = GetComponent<CameraFocus>();
        doors = GameObject.FindGameObjectsWithTag("Door");
        playersAlive = Players.Length;
    }

    void Update()
    {
        gametime += Time.deltaTime;
        timeText.text = "<i>" + displayTime + "</i>";

        TimeDisplay();
        GetControls();
        ShowEnemiesSlain();
        ActivatePlayer();

        playersAlive = GameObject.FindGameObjectsWithTag("Player").Length;

        if (playersAlive <= 0 && gameOver == false)
        {
            GameOver();
        }

        if (gameOver)
        {
            gameOverTimer -= Time.deltaTime;
            if (gameOverTimer <= 0)
            {
                Application.LoadLevel("menu");
            }
        }
    }

    public void ReloadUI()
    {
        Players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < Players.Length; i++)
        {
            PlayersUI[i].SetActive(true);
        }
        ShowActivePlayers();
        foreach (GameObject door in doors)
        {
            door.GetComponent<Door>().FindPlayers();
        }
    }

    void ActivatePlayer()
    {
        switch (Players.Length)
        {
            case 1:
                if (Input.GetButtonDown("StartP2") || Input.GetButtonDown("StartP3") || Input.GetButtonDown("StartP4"))
                {
                    GameObject go = Instantiate(knight, new Vector3(Players[0].transform.position.x, Players[0].transform.position.y + 2, Players[0].transform.position.z), Players[0].transform.rotation) as GameObject;
                    go.GetComponent<Knight>().PlayerID = 2;
                    go.name = "Knight";
                    ReloadUI();
                    camFocus.ResetPlayers();
                    Enemies = GameObject.FindGameObjectsWithTag("Enemy");
                    foreach (GameObject enemy in Enemies)
                    {
                        switch (enemy.name)
                        {
                            case "StatueEnemy":
                                enemy.GetComponent<StatueEnemy>().GetPlayers();
                                break;
                            case "MeleeEnemy(Clone)":
                                enemy.GetComponent<MeleeEnemy>().GetPlayers();
                                break;
                            case "RangedEnemy(Clone)":
                                enemy.GetComponent<RangedEnemy>().GetPlayers();
                                break;
                            case "TrollEnemy(Clone)":
                                enemy.GetComponent<TrollEnemy>().GetPlayers();
                                break;
                            case "Firespitter":
                                enemy.GetComponent<FireSpitter>().GetPlayers();
                                break;
                        }
                    }
                    foreach (GameObject door in doors)
                    {
                        door.GetComponent<Door>().FindPlayers();
                    }
                }  
                break;
            case 2:
                if (Input.GetButtonDown("StartP3") || Input.GetButtonDown("StartP4"))
                {
                    GameObject go = Instantiate(knight, new Vector3(Players[0].transform.position.x, Players[0].transform.position.y + 2, Players[0].transform.position.z), Players[0].transform.rotation) as GameObject;
                    go.GetComponent<Knight>().PlayerID = 3;
                    go.name = "Knight";
                    ReloadUI();
                    camFocus.ResetPlayers();
                    Enemies = GameObject.FindGameObjectsWithTag("Enemy");

                    foreach (GameObject enemy in Enemies)
                    {
                        switch (enemy.name)
                        {
                            case "StatueEnemy":
                                enemy.GetComponent<StatueEnemy>().GetPlayers();
                                break;
                            case "MeleeEnemy(Clone)":
                                enemy.GetComponent<MeleeEnemy>().GetPlayers();
                                break;
                            case "RangedEnemy(Clone)":
                                enemy.GetComponent<RangedEnemy>().GetPlayers();
                                break;
                            case "TrollEnemy(Clone)":
                                enemy.GetComponent<TrollEnemy>().GetPlayers();
                                break;
                            case "Firespitter":
                                enemy.GetComponent<FireSpitter>().GetPlayers();
                                break;
                        }
                    }

                    foreach (GameObject door in doors)
                    {
                        door.GetComponent<Door>().FindPlayers();
                    }
                }  
                break;
            case 3:
                if (Input.GetButtonDown("StartP4"))
                {
                    GameObject go = Instantiate(knight, new Vector3(Players[0].transform.position.x, Players[0].transform.position.y + 2, Players[0].transform.position.z), Players[0].transform.rotation) as GameObject;
                    go.GetComponent<Knight>().PlayerID = 4;
                    go.name = "Knight";
                    ReloadUI();
                    camFocus.ResetPlayers();
                    Enemies = GameObject.FindGameObjectsWithTag("Enemy");
                    foreach (GameObject enemy in Enemies)
                    {
                        switch (enemy.name)
                        {
                            case "StatueEnemy":
                                enemy.GetComponent<StatueEnemy>().GetPlayers();
                                break;
                            case "MeleeEnemy(Clone)":
                                enemy.GetComponent<MeleeEnemy>().GetPlayers();
                                break;
                            case "RangedEnemy(Clone)":
                                enemy.GetComponent<RangedEnemy>().GetPlayers();
                                break;
                            case "TrollEnemy(Clone)":
                                enemy.GetComponent<TrollEnemy>().GetPlayers();
                                break;
                            case "Firespitter":
                                enemy.GetComponent<FireSpitter>().GetPlayers();
                                break;
                        }
                    }

                    foreach (GameObject door in doors)
                    {
                        door.GetComponent<Door>().FindPlayers();
                    }
                }  
                break;
        }
    }

    void ShowActivePlayers()
    {
        switch(Players.Length)
        {
            case 1:
                for (int i = 0; i < playerStartUI.Length; i++)
                {
                    playerStartUI[i].gameObject.SetActive(true);
                }
                break;
            case 2:
                playerStartUI[0].gameObject.SetActive(false);
                playerStartUI[1].gameObject.SetActive(true);
                playerStartUI[2].gameObject.SetActive(true);
                break;
            case 3:
                playerStartUI[0].gameObject.SetActive(false);
                playerStartUI[1].gameObject.SetActive(false);
                playerStartUI[2].gameObject.SetActive(true);
                break;
            case 4:
                for (int i = 0; i < playerStartUI.Length; i++)
                {
                    playerStartUI[i].gameObject.SetActive(false);
                }
                break;
        }
    }

    void ShowEnemiesSlain()
    {
        enemiesSlain.text = "Enemies slain: " + EnemiesSlain.ToString();
    }

    void TimeDisplay()
    {
        seconds = Mathf.RoundToInt(gametime) - (60 * minutes);
        minutes = Mathf.RoundToInt(Mathf.Floor(gametime / 60));

        if (minutes < 10 && seconds < 10)
        {
            displayTime = "0" + minutes + ":0" + seconds;
        }

        if (minutes > 9 && seconds > 9)
        {
            displayTime = minutes + ":" + seconds;
        }

        if (minutes > 9 && seconds < 10)
        {
            displayTime = minutes + ":0" + seconds;
        }

        if (minutes < 10 && seconds > 9)
        {
            displayTime = "0" + minutes + ":" + seconds;
        }
    }

    void GetControls()
    {
        switch (Players.Length)
        {
            case 1:
                if (Input.GetButtonDown("StartP1"))
                {
                    SetPause();
                }
                break;
            case 2:
                if (Input.GetButtonDown("StartP1") || Input.GetButtonDown("StartP2"))
                {
                    SetPause();
                }
                break;
            case 3:
                if (Input.GetButtonDown("StartP1") || Input.GetButtonDown("StartP2") || Input.GetButtonDown("StartP3"))
                {
                    SetPause();
                }
                break;
            case 4:
                if (Input.GetButtonDown("StartP1") || Input.GetButtonDown("StartP2") || Input.GetButtonDown("StartP3") || Input.GetButtonDown("StartP4"))
                {
                    SetPause();
                }
                break;
        }

        if (paused)
        {
            if (Input.GetButton("Pause") && (Input.GetButton("Atk4P1") || Input.GetButton("Atk4P2") || Input.GetButton("Atk4P3") || Input.GetButton("Atk4P4")) 
                && (Input.GetButton("SkillP1") || Input.GetButton("SkillP2") || Input.GetButton("SkillP3") || Input.GetButton("SkillP4")))
            {
                Time.timeScale = 1;
                Application.LoadLevel("menu");
            }
        }
    }

    void GameOver()
    {
        Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < Enemies.Length; i++)
        {
            Destroy(Enemies[i].gameObject);
        }
        
        gameOver = true;
        gameOverText.SetActive(true);
    }

    void SetPause()
    {
        if (paused)
        {
            audioSource.Stop();
            audioSource.clip = gameMusic;
            audioSource.Play();
            pausedText.gameObject.SetActive(false);
            paused = false;
            Time.timeScale = 1;
        }
        else
        {
            audioSource.Stop();
            audioSource.clip = pauseMusic;
            audioSource.Play();
            pausedText.gameObject.SetActive(true);
            paused = true;
            Time.timeScale = 0;
        }
    }
}