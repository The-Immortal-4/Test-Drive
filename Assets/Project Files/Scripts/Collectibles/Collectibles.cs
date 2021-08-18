using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectibles : MonoBehaviour
{
    public float rotateSpeed;
    public AnimationCurve myCurve;
    public AudioSource collectSound;

    
    void Update()
    {
        transform.Rotate(0, rotateSpeed, 0, Space.World);
        transform.position = new Vector3(transform.position.x, myCurve.Evaluate((Time.time % myCurve.length)), transform.position.z);
    }

    void OnTriggerEnter(Collider other)
    {
        
        collectSound.Play();
        FindObjectOfType<ScoringSystem>().AddScore(50);
        Destroy(gameObject);
    }


}
