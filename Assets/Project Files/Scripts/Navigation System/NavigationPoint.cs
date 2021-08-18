using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class NavigationPoint : MonoBehaviour
{
    public bool isReached = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isReached = true;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        float radius = .5f;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
