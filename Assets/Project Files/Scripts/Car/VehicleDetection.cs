using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VehicleDetection : MonoBehaviour
{
    [SerializeField] BoxCollider m_Collider;

    [SerializeField] private int m_ScoreDecrease = 30;
    [SerializeField] private float holdThreshhold = 3f;
    [SerializeField] private float resetThreshhold = 4f;
    [SerializeField] private float outerRangeDistance = 15f;
    [SerializeField] private float innerRangeDistance = 10f;
    [SerializeField] private float carRPMThreshhold = 150f;
    [SerializeField] private float yOffset = 0.55f;
    [SerializeField] private LayerMask vehicleLayer;


    private bool inOuterRange = false;
    private bool inInnerRange = false;
    private float timer = 0f;
    public bool faulted = false;
    private CarControl carControl;
    public float carRPM;

    [SerializeField] private LineRenderer leftLine;
    [SerializeField] private LineRenderer rightLine;
    [SerializeField] private LineRenderer[] middleLine;

    Coroutine resetCr;
    RaycastHit m_HitOuter;
    RaycastHit m_HitInner;

    private void Awake()
    {
       // m_Collider = GetComponent<BoxCollider>();
        carControl = GetComponent<CarControl>();
    }

    private void Update()
    {
        carRPM = carControl.carSpeedConverted;
        if (carRPM > carRPMThreshhold)
        {
            //Check three sec when car is at certain speed
            CheckThreeSecRule();
        }
        else
        {
            inOuterRange = false;
            DisableBothLine();
        }
    }

    private void CheckThreeSecRule()
    {
        inOuterRange = Physics.BoxCast(new Vector3(transform.position.x, transform.position.y - yOffset, transform.position.z),
            m_Collider.size, transform.forward, out m_HitOuter, Quaternion.identity, outerRangeDistance, vehicleLayer);
        inInnerRange = Physics.BoxCast(new Vector3(transform.position.x, transform.position.y - yOffset, transform.position.z),
            m_Collider.size, transform.forward, out m_HitInner, Quaternion.identity, innerRangeDistance, vehicleLayer);

        if (inInnerRange)
        {
            //ThreeSecondRuleFaulted();
            EnableBothLine();
            RenderLines(leftLine, rightLine, m_HitInner, Color.red);
            if (!faulted)
            {
                timer += Time.deltaTime;
                if(timer > holdThreshhold)
                {
                    ThreeSecondRuleFaulted();
                }
            }
        }
        else
        {
            timer = 0f;
            if (inOuterRange)
            {
                //ThreeSecondRuleFaulted();
                EnableBothLine();
                RenderLines(leftLine, rightLine, m_HitOuter, Color.green);
            }
            else
            {
                //  timer = 0f;
                DisableBothLine();
            }
        }
    }

    private void RenderLines(LineRenderer leftLine, LineRenderer rightLine, RaycastHit rayHit, Color col)
    {
        //float zValue = Mathf.Abs(transform.position.z - rayHit.collider.gameObject.transform.position.z) + 2f;
        float zValue = Vector3.Distance(transform.position, rayHit.collider.transform.position);
        
        Vector3 pos = new Vector3(0f, 0f, zValue);
        leftLine.SetPosition(1, pos);
        rightLine.SetPosition(1, pos);

        leftLine.material.color = col;
        rightLine.material.color = col;
        for(int i =0; i < middleLine.Length; i++)
        {
            middleLine[i].material.color = col;
        }
    }

    private void EnableBothLine()
    {
        leftLine.gameObject.SetActive(true);
        rightLine.gameObject.SetActive(true);
    }

    private void DisableBothLine()
    {
        leftLine.gameObject.SetActive(false);
        rightLine.gameObject.SetActive(false);
        leftLine.SetPosition(1, Vector3.zero);
        rightLine.SetPosition(1, Vector3.zero);
    }

    private void ThreeSecondRuleFaulted()
    {
        faulted = true;
        timer = 0f;
        FindObjectOfType<ScoringSystem>().DecreaseScore(m_ScoreDecrease);
        FindObjectOfType<RoadRulePopUpUI>().RoadRulePopUp("Failed to maintain 3 Sec Rule!",Color.red);
        if(resetCr != null)
        {
            StopCoroutine(resetCr);
        }
        resetCr = StartCoroutine(ResetFaulted());
        // Decrease Points Here!!
    }

    private IEnumerator ResetFaulted()
    {
        yield return new WaitForSeconds(resetThreshhold);
        faulted = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("AICar"))
        {
            FindObjectOfType<RoadRulePopUpUI>().RoadRulePopUp("You hit a car!", Color.red);
            FindObjectOfType<ScoringSystem>().DecreaseScore(m_ScoreDecrease);
        }
    }

    private void OnDrawGizmos()
    {
       // Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y - yOffset, transform.position.z), transform.forward * outerRangeDistance);
       // Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y - yOffset, transform.position.z) + transform.forward * outerRangeDistance, m_Collider.size);
    }
}
