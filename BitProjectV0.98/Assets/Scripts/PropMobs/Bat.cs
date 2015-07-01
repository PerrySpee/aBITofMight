using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]

public class Bat : MonoBehaviour {

    

    public GameObject[] destinations;

    private Vector3 destination;

    private bool moving;

    public AudioClip sound;
    private AudioSource audioSource;

    private float soundTimer;
    private float speed;

   
	void Start () 
    {
        destinations = GameObject.FindGameObjectsWithTag("Destination");
        audioSource = GetComponent<AudioSource>();
        GetNewDestination();
        soundTimer = Random.Range(10f, 30f); 
        speed = Random.Range(20f, 60f);
	}
	

	void Update () 
    {
        if (moving == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
            if(Vector3.Distance(transform.position,destination) < 2)
            {
                moving = false;
                GetNewDestination();
            }
        }
        if (soundTimer > 0)
        {
            soundTimer -= Time.deltaTime;
        }
        else if (soundTimer <= 0)
        {
            audioSource.PlayOneShot(sound);
            soundTimer = Random.Range(10f, 30f);
        }
	}

    private void GetNewDestination()
    {
        int newDestination = Random.Range(0, destinations.Length);
        if (newDestination >= destinations.Length -1)
        {
            newDestination = destinations.Length - 1;
        }

        destination = destinations[newDestination].transform.position;

        speed = Random.Range(20f, 60f);
        moving = true;
    }
}
