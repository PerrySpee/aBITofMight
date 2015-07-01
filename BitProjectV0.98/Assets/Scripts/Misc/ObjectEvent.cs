using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectEvent : MonoBehaviour {


    [SerializeField]
    private Vector3 rotateWhere, moveWhere = new Vector3(0,0,0);

    [SerializeField]
    private float rotateSpeed, moveSpeed = 1;


    public GameObject objectToMove;
    private bool moveObj = false;
    private bool rotateObj = false;
    private Vector3 endPos;
    private Vector3 endRot;

    void Start()
    {
        endPos = objectToMove.transform.position + moveWhere;
        endRot = objectToMove.transform.eulerAngles + rotateWhere;
	}
    IEnumerator WaitAndDestroySelf()
    {
        yield return new WaitForSeconds(3f);

        Destroy(gameObject);
    }
    void OnTriggerEnter(Collider hit)
    {
        if (hit.CompareTag("Player") && moveObj == false && rotateObj == false)
        {
            moveObj = true;
            rotateObj = true;
            StartCoroutine(WaitAndDestroySelf());
        }
    }

	void Update () {
        if (moveObj)
        {
            objectToMove.transform.position = Vector3.Lerp(objectToMove.transform.position, endPos, moveSpeed * Time.deltaTime);
            if (objectToMove.transform.position == endPos)
            {
                moveObj = false;
            }
        } 
        if (rotateObj)
        {
            objectToMove.transform.eulerAngles = Vector3.Lerp(objectToMove.transform.eulerAngles, endRot, rotateSpeed * Time.deltaTime);
            if (objectToMove.transform.eulerAngles == endRot)
            {
                rotateObj = false;
            }
        }
	}
}