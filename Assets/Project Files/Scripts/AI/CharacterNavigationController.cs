using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterNavigationController : MonoBehaviour
{
    public float movementSpeed = 1f;
    public float rotationSpeed = 120f;
    public float stopDistance = 2.5f;
    public Vector3 destination;
    public bool reachedDestination = false;

    [SerializeField] private Renderer []models;
    [SerializeField] private Material transparentMaterial;

    private Animator _animator;
    private Rigidbody rb;
    private CapsuleCollider col;

    private Vector3 velocity;
    private Vector3 lastPosition;

    //Stopping
    [SerializeField] private float idleTime = 3f;
    [SerializeField] private int idleProbability = 20;
    [SerializeField] private float idleCheckFrequency = 2f;
    private float idleCheckTime = 0f;
    private bool isIdle = false;
    private bool startMoving = false;

    public LayerMask pathwayLayerMask;
    public float pathwayDetectThreshold = 2f;
    public float jumpForce = 5f;

    private bool fading = false;
    [SerializeField] private float m_DestroyTime = 5f;
    [SerializeField] private float m_ImpulseForceMin = 300f;
    [SerializeField] private float m_ImpulseForceMax = 500f;
    [SerializeField] private float fadeSpeed = 0.5f;
    private bool isDead = false;
    private float savedMovementSpeed;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();

        SetRigidbodyState(true);
        SetColliderState(false);
        savedMovementSpeed = movementSpeed;
        UpdateZombieAnimation();
    }

    private void Update()
    {
        if (transform.position != destination  && !isIdle && startMoving && !isDead)
        {
            Vector3 destinationDirection = destination - transform.position;
            destinationDirection.y = 0;

            float destinationDistance = destinationDirection.magnitude;

            if (destinationDistance >= stopDistance)
            {
                reachedDestination = false;
                Quaternion targetRotation = Quaternion.LookRotation(destinationDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
            }
            else
            {
                reachedDestination = true;
            }

            velocity = (transform.position - lastPosition) / Time.deltaTime;
            velocity.y = 0;
            var velocityMagnitude = velocity.magnitude;
            velocity = velocity.normalized;
            float fwdDotProduct = Vector3.Dot(transform.forward, velocity);
            float rightDotProduct = Vector3.Dot(transform.right, velocity);

            if (velocity.x > 0.1f || velocity.x < 0.1f)
            {
                _animator.SetBool("Moving", true);
            }
            else
            {
                _animator.SetBool("Moving", false);
            }

            _animator.SetFloat("Horizontal", rightDotProduct);
            _animator.SetFloat("Vertical", fwdDotProduct);
        }

        //Limiting how many times it checks to idle
        if (!isDead)
        {
            if (idleCheckTime < idleCheckFrequency)
            {
                idleCheckTime += Time.deltaTime;
            }
            else
            {
                CheckIdle();
                idleCheckTime = 0f;
            }


            DetectPathWay();
            //Detecting Pathways
        }
    }

    private void DetectPathWay()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pathwayDetectThreshold, pathwayLayerMask))
        {
            if (hit.collider.gameObject.CompareTag("Pathway"))
            {
                ImpulseJump(Vector3.up);
                ImpulseJump(Vector3.forward);
            }
        }
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out hit, pathwayDetectThreshold, pathwayLayerMask))
        {
            if (hit.collider.gameObject.CompareTag("Pathway"))
            {
                ImpulseJump(Vector3.up);
                ImpulseJump(Vector3.left);
            }
        }
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hit, pathwayDetectThreshold, pathwayLayerMask))
        {
            if (hit.collider.gameObject.CompareTag("Pathway"))
            {
                ImpulseJump(Vector3.up);
                ImpulseJump(Vector3.right);
            }
        }
    }

    private void ImpulseJump(Vector3 direction)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.TransformDirection(direction) * jumpForce, ForceMode.Impulse);
    }

    public void SetDestination(Vector3 destination)
    {
        this.destination = destination;
        reachedDestination = false;
        StartCoroutine(StartMovingDelay());
    }

    IEnumerator StartMovingDelay()
    {
        yield return new WaitForSeconds(0.5f);
        startMoving = true;
    }

    private void CheckIdle()
    {
        int ran = Random.Range(0, 100);
        if(ran <= idleProbability)
        {
            Idle();
        }
    }

    private void Idle()
    {
        if (!isIdle)
        {
            isIdle = true;
            _animator.SetBool("Moving", false);
            StartCoroutine(ResetIdle());
        }
    }

    public void Die()
    {
        if (!isDead)
        {
            isDead = true;
            isIdle = true;
            _animator.SetBool("Moving", false);
            fading = true;

            //Ragdoll
            GetComponent<Animator>().enabled = false;
            SetRigidbodyState(false);
            SetColliderState(true);

            StartCoroutine(FadeModel());       
        }
        else
        {
            ImpulseAI();
        }
    }

    IEnumerator ResetIdle()
    {
        yield return new WaitForSeconds(idleTime);
        isIdle = false;
    }

    IEnumerator FadeModel()
    {
        if (models != null)
        {
            float fadeDelay = m_DestroyTime;
            foreach (Renderer ren in models)
            {
                ren.material = transparentMaterial;
            }

            yield return new WaitForSeconds(fadeDelay);
            while (fading)
            {
                yield return new WaitForEndOfFrame();
                if (models[models.Length-1].material.color.a > 0)
                {
                    foreach (Renderer ren in models)
                    {
                        Color col = ren.material.color;
                        col.a -= fadeSpeed * Time.deltaTime;
                        ren.material.color = col;
                    }
                }
                else
                {
                    fading = false;
                    Destroy(this.gameObject);
                    break;
                }
            }
        }
        else
        {
            Destroy(this.gameObject, m_DestroyTime);
        }
    }

    private void ImpulseAI()
    {
        //Random Force
        float force = Random.Range(m_ImpulseForceMin, m_ImpulseForceMax);

        Collider[] colliders = Physics.OverlapSphere(transform.position, 50f);

        foreach(Collider col in colliders)
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.AddExplosionForce(force, transform.position, 50f);
            }
        }
    }

    //Ragdol effect
    private void SetRigidbodyState(bool state)
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach(Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = state;
        }

        GetComponent<Rigidbody>().isKinematic = !state;
    }

    private void SetColliderState(bool state)
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();

        foreach (Collider col in colliders)
        {
            col.enabled = state;
        }

        GetComponent<Collider>().enabled = !state;
    }

    private void UpdateZombieAnimation()
    {
        StartCoroutine(ZombieAnimationRoutine());
    }

    private IEnumerator ZombieAnimationRoutine()
    {
        yield return new WaitForSeconds(0.1f);
        SaveManager saveManager = FindObjectOfType<SaveManager>();
        if (saveManager.zombiesActivated)
        {
            _animator.SetBool("isZombie", true);
            float randomSpeed = Random.Range(0.6f, 0.8f); 
            movementSpeed = randomSpeed;
        }
        else
        {
            _animator.SetBool("isZombie", false);
            movementSpeed = savedMovementSpeed;
        }
    }
}
