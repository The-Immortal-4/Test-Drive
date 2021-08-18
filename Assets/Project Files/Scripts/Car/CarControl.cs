using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarControl : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;
    private float currentSteerAngle;
    private float currentBreakForce;
    private bool isBreaking = false;

    [SerializeField] private float maxMotorTorque;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteerAngle;

    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;

    float motorTorque;                 // Current motor Torque
    float carSpeed;                    // Speed in m/s
    public float carSpeedConverted;           // Speed in Km/s
    float engineAudioPitch = 1f;

    private AudioSource engineAudioSource;
    private Rigidbody rb;
    private Speedometer speedometer;
    public float centerOfMassOffset = -0.9f;

    [SerializeField] private Material LightMat;
    [SerializeField] private float LightingBrightness = 0.3f;
    private bool isLighting = false;
    private GameState gameState;


    private void Awake()
    {
        engineAudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, centerOfMassOffset, 0);

        gameState = FindObjectOfType<GameState>();
        speedometer = GameObject.FindGameObjectWithTag("Speedometer").GetComponent<Speedometer>();
       // engineAudioSource.Play();
        speedometer.speedMax = maxSpeed;
    }

    private void Update()
    {
        carSpeed = rb.velocity.magnitude;                  // Car speed in m/s               

        carSpeedConverted = Mathf.Round(carSpeed * 3.6f);  // Car speed in Km/s
        speedometer.speed = carSpeedConverted;

        GetInput();
        // Calculate the engine sound
        engineSound();
    }

    private void FixedUpdate()
    {     
        HandleMotor();
        HandleBreaking();
        HandleSteering();
        UpdateWheels();       
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        isBreaking = Input.GetKey(KeyCode.Space);
    }

    public float GetRPM()
    {
        return carSpeedConverted;
    }

    private void HandleMotor()
    {
        // Check if car speed has exceeded from maxSpeed
        if (carSpeedConverted < maxSpeed)
        {
            motorTorque = maxMotorTorque * verticalInput;
        }
        else
        {
            motorTorque = 0;
        }

        frontLeftWheelCollider.motorTorque = motorTorque;
        frontRightWheelCollider.motorTorque = motorTorque;
    }

    private void HandleBreaking()
    {
        if (isBreaking)
        {
            motorTorque = 0;
            currentBreakForce = breakForce;
            LightOn();
        }
        else
        {
            currentBreakForce = 0;
            LightOFF();
        }
        ApplyBreaking();
    }

    private void ApplyBreaking()
    {
        frontLeftWheelCollider.brakeTorque = currentBreakForce;
        frontRightWheelCollider.brakeTorque = currentBreakForce;
        rearLeftWheelCollider.brakeTorque = currentBreakForce;
        rearRightWheelCollider.brakeTorque = currentBreakForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot * Quaternion.Euler(180, 90, 0);
        wheelTransform.position = pos;
    }

    // Calculate the engine sound based on the car speed by changing the audio pitch
    private void engineSound()
    {
        if (gameState.currentState == GameState.States.IsPause || gameState.currentState == GameState.States.GameWon)
        {
            if (engineAudioSource.isPlaying)
            {
                engineAudioSource.volume = 0.0f;
                engineAudioSource.Stop();
                engineAudioSource.Pause();
            }
        }
        else
        {
            if (!engineAudioSource.isPlaying)
            {
                engineAudioSource.Play();
            }
            float y = 0.4f;
            float z = 0.1f;

            engineAudioSource.volume = 0.08f;

            if (verticalInput == 0 && carSpeedConverted > 30)
            {
                engineAudioSource.volume = 0.05f;

                if (engineAudioPitch >= 0.35f)
                {
                    engineAudioPitch -= Time.deltaTime * 0.1f;
                }
            }
            else if (carSpeedConverted <= 5)
            {
                engineAudioPitch = 0.15f;
            }
            else if (verticalInput != 0 && carSpeedConverted > 5 && carSpeedConverted <= 45)
            {

                float x = ((carSpeedConverted - 5) / 40) * y;
                engineAudioPitch = z + x;
            }
            else if (verticalInput != 0 && carSpeedConverted > 45 && carSpeedConverted <= 85)
            {
                float x = ((carSpeedConverted - 45) / 40) * y;
                engineAudioPitch = z + x + 0.2f;
            }
            else if (verticalInput != 0 && carSpeedConverted > 85 && carSpeedConverted <= 115)
            {
                float x = ((carSpeedConverted - 85) / 30) * y;
                engineAudioPitch = z + x + 0.3f;
            }
            else if (verticalInput != 0 && carSpeedConverted > 115 && carSpeedConverted <= 145)
            {
                float x = ((carSpeedConverted - 115) / 30) * y;
                engineAudioPitch = z + x + 0.4f;
            }
            else if (verticalInput != 0 && carSpeedConverted > 145 && carSpeedConverted <= 165)
            {
                float x = ((carSpeedConverted - 125) / 20) * y;
                engineAudioPitch = z + x + 0.6f;
            }
            else if (verticalInput != 0 && carSpeedConverted > 165)
            {
                engineAudioPitch = 1.5f;
            }

            engineAudioSource.pitch = engineAudioPitch;
        }
    }

    private void LightOn()
    {
        if (!isLighting)
        {
            isLighting = true;
            Color col = Color.red;
            col.a = LightingBrightness;
            LightMat.color = col;
        }
    }

    private void LightOFF()
    {
        if (isLighting)
        {
            isLighting = false;
            Color col = Color.black;
            col.a = 1f;
            LightMat.color = col;
        }
    }
}
