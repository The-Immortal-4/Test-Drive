using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldCoin : MonoBehaviour
{
    [SerializeField] private int m_Score = 50;
    [SerializeField] private float m_RotateSpeed = 5f;
    [SerializeField] private AnimationCurve myCurve;
    [SerializeField] private AudioClip collectSound;


    private void FixedUpdate()
    {
        Rotate();
    }

    private void Rotate()
    {
        transform.Rotate(0, m_RotateSpeed, 0, Space.World);
        transform.position = new Vector3(transform.position.x,
            myCurve.Evaluate((Time.time % myCurve.length)), transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<ScoringSystem>().PlayCoinSound(collectSound);
            FindObjectOfType<ScoringSystem>().AddScore(m_Score);
            Destroy(gameObject, 0.2f);
        }
    }
}
