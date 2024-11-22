using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    //Used ChatGPT for help generating this script

    public float amplitude = 0.5f;
    public float frequency = 1f; 

    private Vector3 startPosition;

    void Start()
    {
        // Store the initial position of the ship
        startPosition = transform.position;
    }

    void Update()
    {
        // Calculate the new Y position using a sine wave
        float newY = startPosition.y + Mathf.Sin(Time.time * frequency) * amplitude;

        // Apply the new position while keeping X and Z the same
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
