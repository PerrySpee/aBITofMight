using UnityEngine;
using System.Collections;

public class FlameBlaster : MonoBehaviour
{

    public GameObject flames;
    Collider flameCollider;


    // Use this for initialization
    void Start()
    {
        flameCollider = GetComponent<BoxCollider>();
        flames.GetComponent<ParticleSystem>().emissionRate = 0;
        TurnOff();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void TurnOn()
    {
        Invoke("TurnOff", 2);
        flameCollider.enabled = true;
        flames.GetComponent<ParticleSystem>().emissionRate = 45;
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
    }

    void TurnOff()
    {
        Invoke("TurnOn", Random.Range(2, 5));
        flameCollider.enabled = false;
        flames.GetComponent<ParticleSystem>().emissionRate = 0;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Knight>().AdjustHealthPercentage(0.3f);
        }
    }

}
