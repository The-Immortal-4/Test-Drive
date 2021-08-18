using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speedometer : MonoBehaviour
{
    private const float MAX_SPEED_ANGLE = -20;
    private const float ZERO_SPEED_ANGLE = 220;

    [SerializeField] private Transform needleTransform;
    //private Transform Speed;

    public float speedMax = 200f;
    public float speed;

    private void Awake()
    {
        speed = 0f;
    }

    private void Update()
    {
        speed += 30f * Time.deltaTime;
        if (speed > speedMax) speed = speedMax;

        needleTransform.eulerAngles = new Vector3(0, 0, GetSpeedRotation());

    }

    private float GetSpeedRotation()
    {
        float totalAngleSize = ZERO_SPEED_ANGLE - MAX_SPEED_ANGLE;

        float speedNormalized = speed / speedMax;

        return ZERO_SPEED_ANGLE - speedNormalized * totalAngleSize;

    }
}
