using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    public float amplitude = 0.5f; // How high the ship moves up and down
    public float frequency = 1f;  // How fast the ship moves up and down

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
