using UnityEngine;
using System.Collections;

public class Rat : MonoBehaviour {

    public float moveSpeed;
    private bool movingRight;

	
    void Start () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "RatCollider")
        {

            ChangeDirection();
        }
    }

    void Update() 
    {
        transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
	}

    void ChangeDirection()
    {
        if (movingRight)
        {
            movingRight = false;
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            movingRight = true;
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }
}
