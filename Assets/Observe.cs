using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observe : MonoBehaviour
{
    [SerializeField] Transform target;

    [SerializeField] float dampening;
    [SerializeField] float focalSize;
    [SerializeField] Vector3 focalOffset;

    private void FixedUpdate()
    {
        if (target == null) { return; }
        float distance = Vector3.Distance(target.position, transform.position - focalOffset);
        if (distance < focalSize) { return; }
        transform.position = Vector3.Lerp(transform.position, target.position + focalOffset, 1f/dampening); 
    }
}
