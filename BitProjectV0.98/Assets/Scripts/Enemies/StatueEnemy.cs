using UnityEngine;
using System.Collections;

public class StatueEnemy : BasicEnemy
{

    public GameObject Target;


    public Vector3 TargetPos;

    public bool CanMove = false;
    public bool CanAttack = false;
    private bool lockedTarget;



    public override void Start()
    {
        base.Start();
        SetTarget(Players[0]);
    }

    void Awake()
    {
        facing = 1;
        anim.speed = 0;
    }

    void Update()
    {
        EnemyAI();
    }


    private void EnemyAI()
    {
        //Step 1:Find the closest target
        FindClosestTarget();

        //Step 2:If the enemy has a target, look at it and walk towards it, else: Idle();
        Movement();

        //Step 3:Attack the target when he is in attack range
        Attack();

    }

    private void FindClosestTarget()
    {
        if (!lockedTarget)
        {
            foreach (GameObject player in Players)
            {
                float PreviousDistance = Vector3.Distance(transform.position, Target.transform.position);
                if (Vector3.Distance(player.transform.position, transform.position) < PreviousDistance)
                {
                    SetTarget(player);
                }
            }
        }
    }

    private void Movement()
    {
        if (Target != null)
        {
            float distance = Vector3.Distance(Target.transform.position, transform.position);
            if (distance <= VisionRange && CanAttack == false && lockedTarget == false)
            {
                anim.speed = 1;
                anim.SetBool("StatuePose", true);
                StartCoroutine(WaitAndSetBool(0.5f,"CanAttack"));
                lockedTarget = true;

            }
        }
        if (CanAttack)
        {
            if (Target.transform.position.x > transform.position.x)
            {
                transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, 270, 0), 8 * Time.deltaTime);
                if (CanMove)
                {
                    transform.Translate(MoveSpeed * Time.deltaTime, 0, 0, Space.World);
                    anim.SetBool("Walking", true); 
                }
                else
                {
                    StartCoroutine(WaitAndSetBool(0.8f, "CanMove"));
                }
                facing = 1;
            }
            else
            {
                transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, 90, 0), 8 *Time.deltaTime);

                if (CanMove)
                {
                    transform.Translate(-MoveSpeed * Time.deltaTime, 0, 0, Space.World);
                    anim.SetBool("Walking", true);
                }
                else
                {
                    StartCoroutine(WaitAndSetBool(0.8f, "CanMove"));
                }
                facing = -1;
            }
        }
    }


    private void Attack()
    {
        if (Vector3.Distance(transform.position, Target.transform.position) <= AttackRange && CanAttack == true)
        {
            //Attack Animation
            anim.SetBool("Walking", false);
            anim.SetBool("Slash", true);
            //Damage player
            switch (Target.name)
            {
                case "Knight":

                    Knight knight = Target.GetComponent<Knight>();
                    knight.AdjustHealth(Damage);
                    break;

                case "Caveman":

                    Caveman caveman = Target.GetComponent<Caveman>();
                    caveman.AdjustHealth(Damage);
                    break;

                case "Magician":

                    Magician magician = Target.GetComponent<Magician>();
                    magician.AdjustHealth(Damage);
                    break;

                case "Gunslinger":

                    Gunslinger gunslinger = Target.GetComponent<Gunslinger>();
                    gunslinger.AdjustHealth(Damage);
                    break;
            }

            CanAttack = false;
            //Attack animation followed by fading away in the shadows.
            Destroy(gameObject, 0.5f);

        }
    }

    private void SetTarget(GameObject target)
    {
        Target = target;
    }


    IEnumerator WaitAndSetBool(float time, string BoolToSet)
    {
        yield return new WaitForSeconds(time);
        switch (BoolToSet)
        {
            case "CanAttack":
                CanAttack = true;
                break;
            case "CanMove":
                CanMove = true;
                break;
        }
        anim.SetBool("StatuePose", false);
    }
}