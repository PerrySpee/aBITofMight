using UnityEngine;
using System.Collections;

public class BasicPlayer : MonoBehaviour
{
    Announcer announcer;
    GameManager gameManager;
    CameraFocus camFocus;
    public float MovementSpeed;

    public float CurHealth,
                    MaxHealth,
                    facing;

    public int PlayerID;

    public float curExp,
                    maxExp;

    public int level,
                    skillPoints;

    public float jumpForce,
                    doubleJumpForce,
                    landHeight,
                    height;

    public bool CanJump,
                    CanDoubleJump,
                    landing;

    public float SafityCooldown;
    private float TimeToSafity;

    public GameObject levelUpParticle;
    public GameObject doubleJumpParticle;
    public GameObject bloodParticle;

    public AudioClip levelUpSkillSound;

    public int HealthToRegen;
    public float RegenCooldown;
    private float RegenTimer;
    private bool CanHeal;

    //Abilities CD's
    public float Ability1Cooldown,
                    Ability2Cooldown,
                    Ability3Cooldown,
                    Ability4Cooldown;

    private float Ability2BaseCD,
                    Ability3BaseCD,
                    Ability4BaseCD;


    [HideInInspector]
    public float AbilityTime1,
                    AbilityTime2,
                    AbilityTime3,
                    AbilityTime4;

    public float Agility,
                    Power,
                    Defense;

    public float AgilityCDR,
                    DefenseArmorMultiplier,
                    DefenseHealth,
                    PowerMultiplier;

    public float Damage;
    [HideInInspector]
    public float xAxis;


    public bool jumpButton;
    public bool CanUseSkill;
    public bool dead = false;

    [HideInInspector]
    public Rigidbody rbody;

    [HideInInspector]
    public AudioSource audioSource;

    void Awake()
    {
        CanJump = false;
    }

    public virtual void Start()
    {
        audioSource = GetComponent<AudioSource>();
        announcer = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Announcer>();
        camFocus = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFocus>();
        gameManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>();
        rbody = GetComponent<Rigidbody>();
        Ability2BaseCD = Ability2Cooldown;
        Ability3BaseCD = Ability3Cooldown;
        Ability4BaseCD = Ability4Cooldown;
        AbilityTime1 = -0.002f;
        AbilityTime2 = -0.002f;
        AbilityTime3 = -0.002f;
        AbilityTime4 = -0.002f;
    }

    public virtual void FixedUpdate()
    {
        Movement();
        CheckHeight();
    }

    public virtual void Update()
    {
        xAxis = Input.GetAxis("HorP" + PlayerID);
        jumpButton = Input.GetButtonDown("JumpP" + PlayerID);

        if (TimeToSafity <= 0)
        {
            TimeToSafity = 0;
            CanHeal = true;
        }
        else
        {
            CanHeal = false;
            TimeToSafity -= Time.deltaTime;
        }

        if (CanHeal && CurHealth < MaxHealth)
        {
            RegenerateHealth();
        }

        SkillLevel();
    }

    public void AdjustHealth(float Dmg)
    {
        if (Dmg > (MaxHealth - CurHealth))
        {
            Dmg = (MaxHealth - CurHealth);
        }

        if (Dmg > 0)
        {

            CurHealth += Dmg;
        }
        else
        {
            GameObject GO = Instantiate(bloodParticle, new Vector3(transform.position.x, transform.position.y + 3, transform.position.z + 1), bloodParticle.transform.rotation) as GameObject;
            GO.transform.SetParent(transform);
            if (Defense < 10)
            {
                Dmg += (Defense * DefenseArmorMultiplier + 1);
            }
            else
            {
                Dmg += (10 * DefenseArmorMultiplier + 1);
            }
            CurHealth += Dmg;
            TimeToSafity = SafityCooldown;
        }

        CurHealth = (int)CurHealth;

        if (CurHealth <= 0 && !dead)
        {
            CurHealth = 0;
            gameManager.ReloadUI();
            camFocus.ResetPlayers();
            Die();
            Debug.Log("Dead.");

        }

        if (CurHealth >= MaxHealth)
        {
            CurHealth = MaxHealth;
        }
    }

    public void AdjustHealthPercentage(float percentOfHealth)
    {
        CurHealth -= (MaxHealth / 100 * percentOfHealth);

        CurHealth = (int)CurHealth;

        if (CurHealth <= 0 && !dead)
        {
            CurHealth = 0;
            gameManager.ReloadUI();
            camFocus.ResetPlayers();
            Die();
            Debug.Log("Dead.");

        }

        if (CurHealth >= MaxHealth)
        {
            CurHealth = MaxHealth;
        }
    }

    public virtual void CheckHeight()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, Mathf.Infinity))
        {

            if (hit.collider.tag == "Ground")
            {
                if (hit.distance < 1.5f)
                {
                    CanJump = true;
                }
                else
                {
                    CanJump = false;
                }

                if (hit.distance < landHeight)
                {
                    landing = true;
                }
                else
                {
                    landing = false;
                }

                height = hit.distance;
            }
        }
    }

    private void Movement()
    {
        if (xAxis < -0.75f)
        {
            transform.Translate(-MovementSpeed * Time.deltaTime, 0, 0, Space.World);
            transform.eulerAngles = new Vector3(0, 180, 0);
            facing = -1;
        }
        if (xAxis > 0.75f)
        {
            transform.Translate(MovementSpeed * Time.deltaTime, 0, 0, Space.World);
            transform.eulerAngles = new Vector3(0, 0, 0);
            facing = 1;
        }


        if (jumpButton)
        {
            if (CanJump)
            {
                CanJump = false;
                CanDoubleJump = true;
                rbody.velocity = new Vector3(0, jumpForce, 0);
            }
            else if (CanDoubleJump && rbody.velocity.y < 18F)
            {
                rbody.velocity = new Vector3(0, doubleJumpForce, 0);
                Instantiate(doubleJumpParticle, new Vector3(transform.position.x, transform.position.y, transform.position.z + 1), doubleJumpParticle.transform.rotation);
                CanDoubleJump = false;
            }
        }
    }

    private void RegenerateHealth()
    {
        if (RegenTimer <= 0)
        {
            AdjustHealth(HealthToRegen);
            RegenTimer = RegenCooldown;
        }
        else
        {
            RegenTimer -= Time.deltaTime;
        }
    }

    public void GetExperience(float expAmount)
    {
        curExp += expAmount;
        if (curExp >= maxExp)
        {
            curExp = (curExp - maxExp);
            level++;
            GameObject GO = Instantiate(levelUpParticle, new Vector3(transform.position.x, transform.position.y, transform.position.z + 1), levelUpParticle.transform.rotation) as GameObject;
            GO.transform.SetParent(transform);
            maxExp *= 1.2f;
            skillPoints++;
        }
    }

    public void SkillLevel()
    {
        if (skillPoints > 0)
        {
            //show skillpoint  ready UI per player

            if (Input.GetButton("SkillP" + PlayerID))
            {
                CanUseSkill = false;

                if (Input.GetButtonDown("Atk1P" + PlayerID))
                {
                    MovementSpeed += MovementSpeed / 100 * 5f;
                    Agility++;
                    skillPoints--;
                    audioSource.PlayOneShot(levelUpSkillSound);
                    ResetCooldowns();
                }

                if (Input.GetButtonDown("Atk2P" + PlayerID))
                {
                    Power++;
                    skillPoints--;
                    audioSource.PlayOneShot(levelUpSkillSound);
                }

                if (Input.GetButtonDown("Atk3P" + PlayerID))
                {
                    Defense++;
                    skillPoints--;
                    MaxHealth += DefenseHealth;
                    CurHealth += DefenseHealth;
                    audioSource.PlayOneShot(levelUpSkillSound);
                }
            }

        }

        if (Input.GetButtonUp("SkillP" + PlayerID))
        {

            CanUseSkill = true;

        }
    }

    public void ResetCooldowns()
    {
        if (Ability2Cooldown > 5)
        {
            Ability2Cooldown -= (Ability2BaseCD / 100 * AgilityCDR);
        }
        else if (Ability2Cooldown <= 5)
        {
            Ability2Cooldown = 5;
        }
        if (Ability3Cooldown > 2)
        {
            Ability3Cooldown -= (Ability3BaseCD / 100 * AgilityCDR);
        }
        else if (Ability3Cooldown <= 2)
        {
            Ability3Cooldown = 2;
        }
        if (Ability4Cooldown > 4)
        {
            Ability4Cooldown -= (Ability4BaseCD / 100 * AgilityCDR);
        }
        else if (Ability4Cooldown <= 4)
        {
            Ability4Cooldown = 4;
        }
    }

    private void Die()
    {
        dead = true;
        //Destroy
        announcer.GetMessage("Player " + PlayerID + " died.");
        Destroy(gameObject, 0.2f);

    }
}