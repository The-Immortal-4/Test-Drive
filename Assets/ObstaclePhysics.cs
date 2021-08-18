using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePhysics : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("PhysicsObstacle"))
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                float randomForce = Random.Range(5.0f, 7.0f);
                rb.AddForce(randomForce * transform.forward, ForceMode.Impulse);
            }
        }
    }
}
