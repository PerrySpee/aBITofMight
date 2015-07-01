using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{

    public GameObject[] spawnPoints;
    public GameObject[] enemyUnits;


    public int totalUnitPoints;
    public int currentUnitPoints;


    private int spawnPointNumber;
    private int randomcase;

    public int maxWaveNumber;
    private int currentWavenumber;
    public bool canSpawn;
    public bool spawningEnded;
    private bool waveStarted = false;

    public GameObject[] enemiesRemaining;
    
    public int enemiesLeft;


    private Text enemiesLeftText;

    private GameObject[] doors;
    public bool doorsOpened;

    public string entranceMessage;
    public string exitMessage;

    public bool isLevel0;
    public bool isTriggered;

    Announcer announcer;

    void Start()
    {
        announcer = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Announcer>();
        doors = GameObject.FindGameObjectsWithTag("Door");
        enemiesLeftText = GameObject.Find("RemainingEnemies").GetComponent<Text>();
        if (isLevel0)
        {
            StartCoroutine(announcer.AnnounceMessage("Welcome to the lobby! \n Prepare for battle..."));
        }
    }


    void Update()
    {
        if (spawningEnded == true && enemiesLeft <= 0)
        {
            SetDoorState();
            spawningEnded = false;
        }

       
        enemiesLeftText.text = "Remaining: " + enemiesLeft; 
        

        enemiesRemaining = GameObject.FindGameObjectsWithTag("Enemy");
        enemiesLeft = enemiesRemaining.Length;
    }

    void OnTriggerEnter(Collider other)
    {
       
        if (other.gameObject.tag == "Player")
        {
            if (!isTriggered)
            {
                StartCoroutine(announcer.AnnounceMessage(entranceMessage));
                isTriggered = true;
            }
            Knight knight = other.GetComponent<Knight>();
            switch (knight.PlayerID)
            {
                case 1:
                    if (waveStarted == false)
                    {
                        Spawn();
                        currentWavenumber = 1;
                        waveStarted = true;
                    }
                    break;
                case 2:
                    if (waveStarted == false)
                    {
                        Spawn();
                        currentWavenumber = 1;
                        waveStarted = true;
                    }
                    break;
                case 3:
                    if (waveStarted == false)
                    {
                        Spawn();
                        currentWavenumber = 1;
                        waveStarted = true;
                    }
                    break;
                case 4:
                    if (waveStarted == false)
                    {
                        Spawn();
                        currentWavenumber = 1;
                        waveStarted = true;
                    }
                    break;
            }
        }
    }


    void ChooseSpawnPoint()
    {
        spawnPointNumber = Random.Range(0, spawnPoints.Length - 1);
    }

    void SetNewWave()
    {
        currentWavenumber++;
        currentUnitPoints = 0;
        totalUnitPoints = (int)(totalUnitPoints * 1.3f);
        StartCoroutine(WaveTimer(8));
    }

    void Spawn()
    {
        if (currentUnitPoints >= totalUnitPoints)
        {
            
            spawningEnded = true;
        }

        if (currentUnitPoints < totalUnitPoints)
        {
            ChooseSpawnPoint();
            randomcase = Random.Range(0, 3);
            if (randomcase >= 3)
            {
                randomcase = 2;
            }
            Instantiate(enemyUnits[randomcase], spawnPoints[spawnPointNumber].transform.position, enemyUnits[randomcase].transform.rotation);
            currentUnitPoints += randomcase + 1;
            StartCoroutine(WaveTimer(2));
        }
        else if(spawningEnded == false)
        {
            SetNewWave();
        }
    }

    public void ResetWaveSystem()
    {
        currentUnitPoints = 0;
        currentWavenumber = 0;
        spawningEnded = false;
        canSpawn = true;
        isTriggered = false;
        waveStarted = false;
        SetDoorState();
    }

    public void SetDoorState()
    {
        if (doorsOpened == true)
        {
            foreach (GameObject door in doors)
            {
                door.GetComponent<Door>().opened = false;
                door.transform.GetChild(0).gameObject.SetActive(false);
            }
            doorsOpened = false;
        }
        else
        {
            foreach (GameObject door in doors)
            {
                door.GetComponent<Door>().opened = true;
                if (door.transform.GetChild(0) == null)
                {
                    Debug.LogError(door.name + "WERKT NIEE");
                }
                else
                {
                    door.transform.GetChild(0).gameObject.SetActive(true);
                }
            }
            doorsOpened = true;
            StartCoroutine(announcer.AnnounceMessage(exitMessage));
        }
    }

    IEnumerator WaveTimer(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Spawn();
    }
}
