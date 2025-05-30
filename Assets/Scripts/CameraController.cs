using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 offset = new Vector3(0f , 0f ,-10f); // Offset from the target position
    public Transform target; // The target to follow 
    // Start is called once before the first execution of Update after the MonoBehaviour is created 
    private Vector3 velocity = Vector3.zero;
    private void Update()
    {
        Vector3 targetPosition = target.position + offset; // Calculate the target position based on the target's position and the offset
        //transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);   
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, 0.3f); // Smoothly move the camera to the target position
    }
}
