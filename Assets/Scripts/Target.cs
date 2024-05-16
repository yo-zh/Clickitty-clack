using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Target : MonoBehaviour
{
    private Rigidbody targetRb;
    private float minSpeed = 10;
    private float maxSpeed = 20;
    private float maxTorque = 10;
    private float xSpawnRange = 5;
    private float ySpawnPosition = -1;
    void Start()
    {
        targetRb = GetComponent<Rigidbody>();
        targetRb.AddForce(RandomUpwardForce(), ForceMode.Impulse);
        targetRb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);
        transform.position = RandomPosition();
    }

    private void OnMouseDown()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) // MB check for collision in case targets collide w/ each other at spawn, because it looks bad
    {
        Destroy(gameObject);
    }

    Vector3 RandomUpwardForce()
    {
        return Vector3.up * Random.Range(minSpeed, maxSpeed);
    }

    float RandomTorque()
    {
        return Random.Range(-maxTorque, maxTorque);
    }

    Vector3 RandomPosition()
    {
        return new Vector3(Random.Range(-xSpawnRange, xSpawnRange), ySpawnPosition);
    }
}
