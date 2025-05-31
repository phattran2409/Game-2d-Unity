using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 offset = new Vector3(0f , 0f ,-10f); // Offset from the target position
    public Transform target; // The target to follow 
    [SerializeField] private float smoothTime = 0.3f; // Time to smooth the camera movement 
    // Start is called once before the first execution of Update after the MonoBehaviour is created 
    private Vector3 velocity = Vector3.zero;

    private float initialY;
    private float initialZ;

    private void Start()
    {
        initialY = transform.position.y;
        initialZ = transform.position.z;

    }

    //private void Update()
    //{
    //    Vector3 targetPosition = target.position + offset; // Calculate the target position based on the target's position and the offset
    //    //transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);   
    //    transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, 0.3f); // Smoothly move the camera to the target position
    //}

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPosition = new Vector3(target.position.x, initialY, initialZ);
        Vector3 smoothedPosition = Vector3.SmoothDamp( transform.position, targetPosition, ref velocity ,smoothTime );

        transform.position = smoothedPosition;
    }
}
