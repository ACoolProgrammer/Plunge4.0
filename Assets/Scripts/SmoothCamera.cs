using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SmoothCamera : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] float damping;

    public Transform target;

    private Vector3 vel = Vector3.zero;
    
private void FixedUpdate()
{
    Vector3 targetPosition = target.position + offset;
    targetPosition.z = transform.position.z;

    transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref vel, damping);
}
}