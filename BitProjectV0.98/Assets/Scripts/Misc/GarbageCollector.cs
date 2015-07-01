using UnityEngine;
using System.Collections;

public class GarbageCollector : MonoBehaviour {



    void OnCollisionEnter(Collision other)
    {
        Destroy(other.gameObject);
    }
}
