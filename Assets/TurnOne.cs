using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOne : MonoBehaviour
{
    public bool isLeftIndicated;
    public bool isRightIndicated;

    public bool isRecordingTurning;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateIndicatorInfo(GameObject indicator)
    {
        RightLeftIndicators indi = indicator.GetComponent<RightLeftIndicators>();
        isLeftIndicated = indi.leftOn;
        isRightIndicated = indi.rightOn;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            UpdateIndicatorInfo(other.gameObject);
            isRecordingTurning = true;
        }
    }

}
