using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour
{

    public int Damage;
    public float speed;
    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, 5);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, 0, speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.name)
        {
            case "Knight":

                Knight knight = other.GetComponent<Knight>();
                knight.AdjustHealth(Damage);
                break;

            case "Caveman":

                Caveman caveman = other.GetComponent<Caveman>();
                caveman.AdjustHealth(Damage);
                break;

            case "Magician":

                Magician magician = other.GetComponent<Magician>();
                magician.AdjustHealth(Damage);
                break;

            case "Gunslinger":

                Gunslinger gunslinger = other.GetComponent<Gunslinger>();
                gunslinger.AdjustHealth(Damage);
                break;


        }

        Destroy(gameObject);
    }
}
