using UnityEngine;
using System.Collections;

public class BasicEnemy : MonoBehaviour {

    GameManager gameManager;
    WaveManager waveManager;

    public GameObject[] Players;

    public int MoveSpeed;

    public float CurHealth;
    public float MaxHealth;

    public int Damage;
    public float AttackRange;
    public float VisionRange;

    public int level;
    private int highestPlayerLevel;
    private int[] playersLevels;

    public GameObject bloodParticle;

    public float AttackCooldown;
    public int facing;
    public float XP;


    public Animator anim;
    [HideInInspector]public Rigidbody rB;

    
    public virtual void Start()
    {
        GetPlayers();
        CurHealth = MaxHealth;
        rB = GetComponent<Rigidbody>();

        gameManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>();
        waveManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<WaveManager>();
        SetLevel();
        SetXP();
    }


    public void GetPlayers()
    {

        Players = new GameObject[0];
        Players = GameObject.FindGameObjectsWithTag("Player");
    }

    public void AdjustHealth(float Dmg, float knockBackForce, GameObject killedBy)
    {
        GameObject GO = Instantiate(bloodParticle, new Vector3(transform.position.x, transform.position.y + 3, transform.position.z + 1), bloodParticle.transform.rotation) as GameObject;
        GO.transform.SetParent(transform);
        CurHealth += Dmg;
        KnockBack(knockBackForce);
        if (CurHealth <= 0)
        {
            gameManager.EnemiesSlain += 1;
            if(waveManager.enemiesLeft > 0)
            {
                waveManager.enemiesLeft -= 1;
            }
            
            Destroy(gameObject);
            foreach(GameObject player in Players)
            {
                switch (player.name)
                {
                    case "Knight":

                        Knight knight = player.GetComponent<Knight>();
                        knight.GetExperience(XP);
                        break;

                    case "Caveman":

                        Caveman caveman = player.GetComponent<Caveman>();
                        caveman.GetExperience(XP);
                        break;

                    case "Magician":

                        Magician magician = player.GetComponent<Magician>();
                        magician.GetExperience(XP);
                        break;

                    case "Gunslinger":

                        Gunslinger gunslinger = player.GetComponent<Gunslinger>();
                        gunslinger.GetExperience(XP);
                        break;
                }
            }

            switch (killedBy.name)
            {
                case "Knight":

                    killedBy.GetComponent<Knight>().GetExperience(XP * 0.10F);
                    break;

                case "Caveman":
                    killedBy.GetComponent<Caveman>().GetExperience(XP * 0.10F);
                    break;

                case "Magician":
                    killedBy.GetComponent<Magician>().GetExperience(XP * 0.10F);
                    break;

                case "Gunslinger":
                    killedBy.GetComponent<Gunslinger>().GetExperience(XP * 0.10F);
                    break;
            }
        }
    }

    private void SetLevel()
    {
        playersLevels = new int[Players.Length];

        for (int i = 0; i < Players.Length; i++)
        {
            switch (Players[i].name)
            {
                case "Knight":

                    Knight knight = Players[i].GetComponent<Knight>();
                    playersLevels[i] = knight.level;
                    break;

                case "Caveman":

                    Caveman caveman = Players[i].GetComponent<Caveman>();
                    playersLevels[i] = caveman.level;
                    break;

                case "Magician":

                    Magician magician = Players[i].GetComponent<Magician>();
                    playersLevels[i] = magician.level;
                    break;

                case "Gunslinger":

                    Gunslinger gunslinger = Players[i].GetComponent<Gunslinger>();
                    playersLevels[i] = gunslinger.level;
                    break;
            }
        }

        for (int i = 0; i < playersLevels.Length; i++)
        {
            level = level + playersLevels[i];
        }

        foreach (int playerLevel in playersLevels)
        {
            if (playerLevel > highestPlayerLevel)
            {
                highestPlayerLevel = playerLevel;
            }
        }


        level = level / Players.Length;

        int levelDifference = highestPlayerLevel - level;
        if (levelDifference <= 0)
        {
            return;
        }
        else if (levelDifference >= 1 && levelDifference < 3)
        {
            level++;
        }
        else if (levelDifference >= 3 && levelDifference < 5)
        {
            level += 3;
        }
        else if (levelDifference >= 5)
        {
            level = highestPlayerLevel - 1;
        }
    }

    private void SetXP()
    {
        XP = (XP + level) * 1.3f;
    }

    private void KnockBack(float Force)
    {
        rB.AddForce(new Vector3(-Force  * facing,  Force/7, 0), ForceMode.Impulse);
    }
}
