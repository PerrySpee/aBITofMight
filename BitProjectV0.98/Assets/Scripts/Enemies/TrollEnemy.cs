using UnityEngine;
using System.Collections;

public class TrollEnemy : BasicEnemy {

    public GameObject Target;



    public Vector3 TargetPos;
    public float IdleWalkDistance; 
    
    private float AttackTimer;
    private bool CanAttack = true;
    

    private float DestinationTimer = 3;

    public override void Start()
    {
        base.Start();
        SetTarget(Players[0]);
        SetIdleDestination();

        for (int i = 0; i < level; i++)
        {
            Damage -= 6;
            MaxHealth += MaxHealth / 100 * 10;
        }
        CurHealth = MaxHealth;
    }

    void Awake()
    {
        facing = 1;
    }

    void Update()
    {
        EnemyAI();
        AttackCooldownTimer();

    }


    private void EnemyAI()
    {
        //Step 1:Find the closest target
        FindClosestTarget();

        //Step 2:If the enemy has a target, look at it and walk towards it, else: Idle();
        Movement();

        //Step 3:Attack the target when he is in attack range
        Attack();

        //Step 4:Run away if close to dying
    }

    private void FindClosestTarget()
    {
        foreach (GameObject player in Players)
        {
            if (Target != null)
            {
                float PreviousDistance = Vector3.Distance(transform.position, Target.transform.position);
                if (Vector3.Distance(player.transform.position, transform.position) < PreviousDistance)
                {
                    SetTarget(player);
                }
            }
            else
            {
                GetPlayers();
                SetTarget(Players[0]);

                return;
            }
        }
    }

    private void Movement()
    {
        if (Target != null)
        {
            float distance = Vector3.Distance(Target.transform.position, transform.position);
            if (distance >= AttackRange && distance <= VisionRange)
            {
                if (Target.transform.position.x > transform.position.x)
                {
                    transform.Translate(MoveSpeed * Time.deltaTime, 0, 0, Space.World);
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    facing = 1;
                    
                }
                else
                {
                    transform.Translate(-MoveSpeed * Time.deltaTime, 0, 0, Space.World);
                    transform.eulerAngles = new Vector3(0, 180, 0);
                    facing = -1;
                }

                anim.SetBool("Walking", true);
            }
            else if(distance > VisionRange)
            {
                Idle();
                
            }
        }
        else
        {
            Idle();
        }

    }

    void SetIdleDestination()
    {
        float rx = Random.Range(-5f, 5f);
        TargetPos = new Vector3(transform.position.x + (rx * IdleWalkDistance), transform.position.y, transform.position.z);
        DestinationTimer = 3;
    }

    private void Idle() 
    {
        anim.SetBool("Walking", true);

        if (TargetPos.x < transform.position.x)
        {

            transform.Translate(MoveSpeed * Time.deltaTime, 0, 0);

            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            transform.Translate(MoveSpeed * Time.deltaTime, 0, 0);

            transform.eulerAngles = new Vector3(0, 180, 0);

        }
        
        float targetX = TargetPos.x;
        float myX = transform.position.x;
        float distance;
        if (targetX > myX)
        {
            distance = targetX - myX;
        }
        else
        {
            distance = myX - targetX;
        }
        

        if (distance < 1)
        {
            SetIdleDestination();

        }
        else if( DestinationTimer <= 0)
        {
            SetIdleDestination();
        }
        DestinationTimer -= Time.deltaTime;
        
    }

    private void Attack()
    {
        if (Vector3.Distance(transform.position, Target.transform.position) <= AttackRange && CanAttack == true)
        {
            //Attack Animation
            anim.SetBool("Slash", true);
            StartCoroutine(AbilityTimer(0.5f,"Slash"));
            anim.SetBool("Walking", false);
            //Damage player
            switch(Target.name)
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
            AttackTimer = AttackCooldown;
        }
    }

    private void AttackCooldownTimer() 
    {
        if (AttackTimer > 0)
        {
            AttackTimer -= Time.deltaTime;
        }
        else if (AttackTimer <= 0)
        {
            AttackTimer = 0;
            CanAttack = true;
        }
    }

    private void SetTarget(GameObject target)
    {
        Target = target;
    }

    IEnumerator AbilityTimer(float time, string AnimCancelString)
    {
        yield return new WaitForSeconds(time - 0.1f);
        anim.SetBool(AnimCancelString, false);
    }

}