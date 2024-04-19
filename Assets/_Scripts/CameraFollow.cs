using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.3f;
    public Vector3 offset;

    private Vector3 velocity = Vector3.zero;
    private bool isRotating = false;
    private float rotationSpeed = 5f;

    void Update()
    {
        if (target != null)
        {
            Vector3 targetPos = target.position + offset;

            // Rotate the camera based on the player's rotation
            if (isRotating)
            {
                float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
                transform.RotateAround(target.position, Vector3.up, mouseX);
            }

            // Smoothly move the camera to the target position
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
        }
    }

    public void StartRotation()
    {
        isRotating = true;
    }

    public void StopRotation()
    {
        isRotating = false;
    }
}