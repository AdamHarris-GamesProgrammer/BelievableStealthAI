using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawDebugLine : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward * 15.0f, Color.yellow);
    }
}
