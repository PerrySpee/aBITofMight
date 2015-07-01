using UnityEngine;
using System.Collections;

public class Knight : BasicPlayer
{


    public float ChargeForce;
    public Animator KnightAnim;


    public GameObject myCamera;
    public GameObject Weapon;
    public GameObject Shield;


    public AudioClip[] soundEffects;

    SoundEffects soundEffect;
    enum SoundEffects
    {
        Slash = 0,
        Block = 1,
        SpinAttack = 2,
        Dash = 3
    }

    void Awake()
    {
        CanUseSkill = true;
        facing = 1;
        Weapon.gameObject.GetComponent<Collider>().enabled = false;
        Shield.gameObject.GetComponent<Collider>().enabled = false;
        Shield.gameObject.GetComponent<MeshRenderer>().enabled = false;


    }

    public override void Start()
    {
        base.Start();
    }


    public override void Update()
    {
        base.Update();

        PlayerInput();
        CoolDownCounter();
        //myCamera.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 10);

    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (xAxis < -0.75f || xAxis > 0.75f)
        {
            KnightAnim.SetBool("Walk", true);
        }
        else
        {
            KnightAnim.SetBool("Walk", false);
        }

        if (landing)
        {
            KnightAnim.SetBool("Jump", false);
            KnightAnim.SetBool("DoubleJump", false);
        }


        KnightAnim.SetFloat("JumpHeight", height);

        if (jumpButton)
        {
            if (CanJump)
            {
                KnightAnim.SetBool("Jump", true);
            }
            else if (rbody.velocity.y < -0.1F)
            {
                KnightAnim.SetBool("DoubleJump", CanDoubleJump);
            }
        }
    }

    void CoolDownCounter()
    {
        if (AbilityTime1 < Ability1Cooldown && AbilityTime1 > -0.001f)
        {
            AbilityTime1 += Time.deltaTime;
        }
        else
        {
            AbilityTime1 = -0.002f;
        }

        if (AbilityTime2 < Ability2Cooldown && AbilityTime2 > -0.001f)
        {
            AbilityTime2 += Time.deltaTime;
        }
        else
        {
            AbilityTime2 = -0.002f;
        }

        if (AbilityTime3 < Ability3Cooldown && AbilityTime3 > -0.001f)
        {
            AbilityTime3 += Time.deltaTime;
        }
        else
        {
            AbilityTime3 = -0.002f;
        }

        if (AbilityTime4 < Ability4Cooldown && AbilityTime4 > -0.001f)
        {
            AbilityTime4 += Time.deltaTime;
        }
        else
        {
            AbilityTime4 = -0.002f;
        }
    }

    void UseSkill(int skillID)
    {
        switch (skillID)
        {
            //basic Attack "Slash"
            case 1:

                //attack anim here || blocking = attack anim
                KnightAnim.SetBool("Slash", true);
                Weapon.gameObject.GetComponent<Collider>().enabled = true;
                StartCoroutine(AbilityTimer(0.5f, Weapon, "Slash"));
                Weapon.GetComponent<WeaponScript>().damage = Damage * (Power * 1.1f);
                audioSource.PlayOneShot(soundEffects[(int)SoundEffects.Slash]);
                break;

            //blocking
            case 2:
                Shield.gameObject.GetComponent<Collider>().enabled = true;
                Shield.gameObject.GetComponent<MeshRenderer>().enabled = true;
                audioSource.PlayOneShot(soundEffects[(int)SoundEffects.Block]);
                KnightAnim.SetBool("Block", true);
                CanUseSkill = false;
                StartCoroutine(AbilityTimer(5, Shield, "Block"));

                break;

            //spin
            case 3:
                Weapon.GetComponent<WeaponScript>().damage = Damage * (Power * 1.3f);
                Weapon.gameObject.GetComponent<Collider>().enabled = true;
                audioSource.PlayOneShot(soundEffects[(int)SoundEffects.SpinAttack]);
                KnightAnim.SetBool("Spin", true);
                StartCoroutine(AbilityTimer(1f, Weapon, "Spin"));


                break;

            //shield charge
            case 4:
                CanUseSkill = false;
                rbody.AddForce(ChargeForce * facing, ChargeForce / 3, 0, ForceMode.Impulse);
                Shield.gameObject.GetComponent<Collider>().enabled = true;
                Shield.gameObject.GetComponent<MeshRenderer>().enabled = true;
                audioSource.PlayOneShot(soundEffects[(int)SoundEffects.Dash]);
                KnightAnim.SetBool("Block", true);
                StartCoroutine(AbilityTimer(0.7f, Shield, "Block"));
                break;
        }
    }

    IEnumerator AbilityTimer(float time, GameObject ColliderOBj, string AnimCancelString)
    {
        yield return new WaitForSeconds(time - 0.1f);
        ColliderOBj.GetComponent<Collider>().enabled = false;
        if (ColliderOBj.CompareTag("Shield"))
        {
            Shield.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
        KnightAnim.SetBool(AnimCancelString, false);
        CanUseSkill = true;
    }

    IEnumerator AbilityTimer(float time, string AnimCancelString)
    {
        yield return new WaitForSeconds(time - 0.1f);
        KnightAnim.SetBool(AnimCancelString, false);
    }


    //manage player input
    void PlayerInput()
    {
        if (CanUseSkill)
        {
            if (Input.GetButtonDown("Atk1P" + PlayerID) && AbilityTime1 < 0)
            {
                UseSkill(1);
                AbilityTime1 = 0;
            }
            else if (Input.GetButtonDown("Atk2P" + PlayerID) && AbilityTime2 < 0)
            {
                UseSkill(2);
                AbilityTime2 = 0;
            }
            else if (Input.GetButtonDown("Atk3P" + PlayerID) && AbilityTime3 < 0)
            {
                UseSkill(3);
                AbilityTime3 = 0;
            }
            else if (Input.GetButtonDown("Atk4P" + PlayerID) && AbilityTime4 < 0)
            {
                UseSkill(4);
                AbilityTime4 = 0;
            }
        }

    }
}
