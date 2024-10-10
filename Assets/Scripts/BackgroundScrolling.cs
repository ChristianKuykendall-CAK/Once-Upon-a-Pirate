using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScrolling : MonoBehaviour
{
    public float scrollSpeed = 0.5f;  // Speed of the background scrolling
    public float backgroundWidth = 10f; // Width of the background sprite (set this based on your sprite's size)

    private Vector3 startPosition;

    void Start()
    {
        // Save the initial position of the background
        startPosition = transform.position;
    }

    void Update()
    {
        // Continuously scroll the background based on time and scroll speed
        float x = Time.time * scrollSpeed;

        // Set the new position, keeping the y and z the same as the start position
        transform.position = new Vector3(startPosition.x + x, startPosition.y, startPosition.z);
    }
}
