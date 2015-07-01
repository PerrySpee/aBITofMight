using UnityEngine;
using System.Collections;

public class FireSpitter : BasicEnemy {

    public GameObject Target;



    public Vector3 TargetPos;

    private float AttackTimer;
    [SerializeField]private bool CanAttack = true;

    public GameObject Projectile;
    public Transform ProjectileSpawn;


    public override void Start()
    {
        base.Start();
        SetTarget(Players[0]);
    }


    void Update()
    {
        FindClosestTarget();
        AttackCooldownTimer();
        Attack();
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

   

    private void Attack()
    {

        if (Target != null)
        {
            ProjectileSpawn.LookAt(Target.transform);
            Fireball fireball = Projectile.GetComponent<Fireball>();

            fireball.Damage = Damage;

            if (Vector3.Distance(transform.position, Target.transform.position) <= AttackRange && CanAttack == true)
            {
                //Attack Animation
                //Damage player
                Instantiate(Projectile, ProjectileSpawn.position, ProjectileSpawn.transform.rotation);

                CanAttack = false;
                AttackTimer = AttackCooldown;
            }
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
}
