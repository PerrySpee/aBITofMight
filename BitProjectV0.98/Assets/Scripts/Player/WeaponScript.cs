using UnityEngine;
using System.Collections;

public class WeaponScript : MonoBehaviour
{
    private GameObject knightParent;
    public float damage;
    private float force = 100;
    public AudioClip hitEnemy;

    private AudioSource audioSource;

    void Start()
    {
        knightParent = transform.root.gameObject;
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Enemy"))
        {
            switch (other.gameObject.name)
            {
                case "MeleeEnemy(Clone)":

                    MeleeEnemy meleeEnemy = other.GetComponent<MeleeEnemy>();
                    meleeEnemy.AdjustHealth(damage, force, knightParent);

                    break;

                case "StatueEnemy":

                    StatueEnemy statueEnemy = other.GetComponent<StatueEnemy>();
                    statueEnemy.AdjustHealth(damage, force, knightParent);
                    break;

                case "RangedEnemy(Clone)":

                    RangedEnemy rangedEnemy = other.GetComponent<RangedEnemy>();
                    rangedEnemy.AdjustHealth(damage, force, knightParent);
                    break;

                case "TrollEnemy(Clone)":

                    TrollEnemy trollEnemy = other.GetComponent<TrollEnemy>();
                    trollEnemy.AdjustHealth(damage, force, knightParent);
                    break;
                case "BossTroll(Clone)":

                    TrollEnemy bossTroll = other.GetComponent<TrollEnemy>();
                    bossTroll.AdjustHealth(damage, force, knightParent);
                    break;
            }

            if (hitEnemy != null)
            {
                audioSource.PlayOneShot(hitEnemy);
            }
        }
    }
}
