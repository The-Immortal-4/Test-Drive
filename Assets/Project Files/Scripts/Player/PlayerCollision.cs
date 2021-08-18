using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    void OnCollisionEnter (Collision collisionInfo)
    {
       if (collisionInfo.collider.name == "Obstacle")
        {
            Debug.Log("You hit a obstacle");
        }
    }
}
