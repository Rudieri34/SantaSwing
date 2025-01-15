using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Transform cameraTransform; // Reference to the camera
    public Vector2 parallaxMultiplier; // Multiplier for parallax effect (x, y)
    private Vector3 lastCameraPosition; // Previous camera position

    void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform; // Assign main camera if not set
        }
        lastCameraPosition = cameraTransform.position; // Initialize last camera position
    }

    void LateUpdate()
    {
        // Calculate the difference in camera position
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;

        // Move the background based on parallax multiplier
        transform.position += new Vector3(deltaMovement.x * parallaxMultiplier.x, deltaMovement.y * parallaxMultiplier.y);

        // Update last camera position
        lastCameraPosition = cameraTransform.position;
    }
}