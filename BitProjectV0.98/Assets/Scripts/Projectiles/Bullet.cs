using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public int Damage;
	// Use this for initialization
	void Start () {
        Invoke("Destroy", 5);
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(10 * Time.deltaTime, 0, 0);
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

    void Destroy()
    {
        Destroy(gameObject);
    }
}
