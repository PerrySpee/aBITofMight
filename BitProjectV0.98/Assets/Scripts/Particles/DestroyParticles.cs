using UnityEngine;
using System.Collections;

public class DestroyParticles : MonoBehaviour
{
    private float particleDuration;

    // Use this for initialization
    void Start()
    {
        particleDuration = GetComponent<ParticleSystem>().duration;
        Destroy(gameObject, particleDuration);
    }
}
