using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawDebugSphere : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.05f);
    }
}
