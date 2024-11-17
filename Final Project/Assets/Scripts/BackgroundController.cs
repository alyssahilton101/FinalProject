using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public float scrollSpeed = 5f; // Speed of the background movement
    public float backgroundWidth; // Width of a single background object
    public GameObject[] backgrounds; // Assign all background objects here

    private Vector3[] originalPositions; // Array to store the original positions

    void Start()
    {
        // Initialize the array to match the number of backgrounds
        originalPositions = new Vector3[backgrounds.Length];

        // Save the original positions of the backgrounds
        for (int i = 0; i < backgrounds.Length; i++)
        {
            originalPositions[i] = backgrounds[i].transform.position;
        }
    }

    void Update()
    {
        foreach (GameObject bg in backgrounds)
        {
            backgroundWidth = bg.GetComponent<SpriteRenderer>().bounds.size.x;

            // Move the background to the left
            bg.GetComponent<Transform>().position += Vector3.left * scrollSpeed * Time.deltaTime;

            // If the background is out of view, move it to the end
            if (bg.GetComponent<Transform>().position.x <= -backgroundWidth)
            {
                bg.GetComponent<Transform>().position += Vector3.right * backgroundWidth * backgrounds.Length;
            }
        }
    }

    // Method to reset backgrounds to their original positions
    public void ResetBackgrounds()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            backgrounds[i].transform.position = originalPositions[i];
        }
    }
}
