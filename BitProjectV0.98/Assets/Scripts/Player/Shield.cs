using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {

    private GameObject knightParent;
    public float damage = -10;
    private float force = 8;

    void Start()
    {
        knightParent = transform.root.gameObject;
    }

    void OnTriggerStay(Collider other)
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
            }
        }
    }
}
