using UnityEngine;
using System.Collections;

public class RangedEnemy : BasicEnemy
{

    public GameObject Target;



    public Vector3 TargetPos;
    public float IdleWalkDistance;

    private float AttackTimer;
    private bool CanAttack = true;

    public GameObject Projectile;
    public Transform ProjectileSpawn;

    private float DestinationTimer = 3;

    public override void Start()
    {
        base.Start();
        SetTarget(Players[0]);
        SetIdleDestination();

        for (int i = 0; i < level; i++)
        {
            Damage -= 4;
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

                anim.SetBool("Walking", false);
            }
            else if (distance > VisionRange)
            {
                Idle();

            }
            else if (distance <= AttackRange)
            {
                if (Target.transform.position.x > transform.position.x)
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    facing = 1;

                }
                else
                {
                    transform.eulerAngles = new Vector3(0, 180, 0);
                    facing = -1;
                }
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
            facing = 1;
        }
        else
        {

            transform.Translate(MoveSpeed * Time.deltaTime, 0, 0);

            transform.eulerAngles = new Vector3(0, 180, 0);
            facing = -1;
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
        else if (DestinationTimer <= 0)
        {
            SetIdleDestination();
        }
        DestinationTimer -= Time.deltaTime;

    }

    private void Attack()
    {
        Bullet bullet = Projectile.GetComponent<Bullet>();

        bullet.Damage = Damage;
        if (Vector3.Distance(transform.position, Target.transform.position) <= AttackRange && CanAttack == true)
        {
            anim.SetBool("Attacking", true);
            StartCoroutine(AbilityTimer(1f, "Attacking"));
            anim.SetBool("Walking", false);
            //Damage player
            if (facing == 1)
            {
                ProjectileSpawn.transform.eulerAngles = new Vector3(ProjectileSpawn.transform.rotation.x, 360, ProjectileSpawn.transform.rotation.z);
            }
            else if(facing == -1)
            {
                ProjectileSpawn.transform.eulerAngles = new Vector3(ProjectileSpawn.transform.rotation.x,180,ProjectileSpawn.transform.rotation.z);
            }
            StartCoroutine(WaitAndFire(0.3f, Projectile));
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

    IEnumerator WaitAndFire(float time, GameObject projectile)
    {
        yield return new WaitForSeconds(time);

        Instantiate(projectile, ProjectileSpawn.position, ProjectileSpawn.transform.rotation);
    }
}