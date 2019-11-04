﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class SwimControl : MonoBehaviour
{
    public int swimForceMultiplier = 100;
    private SteamVR_Action_Pose poseAction;
    private Rigidbody rb;
    public bool logVelocity = false;
    public Crest.SimpleFloatingObject sfo;
    public GameObject head;
    private float handUpTime = 0;
    private float handDeltaThreshold = .5f;


    // Start is called before the first frame update
    void Start()
    {
        poseAction = SteamVR_Input.GetAction<SteamVR_Action_Pose>("Pose");
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        var combined_velocity = poseAction[SteamVR_Input_Sources.LeftHand].velocity.magnitude + poseAction[SteamVR_Input_Sources.RightHand].velocity.magnitude;
        if (logVelocity)
        {
            Debug.Log("vel: " + combined_velocity);
        }
        sfo._raiseObject = 2 - head.transform.position.y;
        rb.AddForce(Camera.main.transform.forward * combined_velocity * swimForceMultiplier);
        var leftDelta = poseAction[SteamVR_Input_Sources.LeftHand].localPosition.y - Camera.main.transform.localPosition.y;
        var rightDelta = poseAction[SteamVR_Input_Sources.RightHand].localPosition.y - Camera.main.transform.localPosition.y;
        Debug.Log(leftDelta + "," + rightDelta);
        if (leftDelta > handDeltaThreshold || rightDelta > handDeltaThreshold)
        {
            Debug.Log("Hand is up");
            if (Time.time - handUpTime > 10)
            {
                Debug.Log("hand was up for more than 10s");
            }
        } else
        {
            handUpTime = Time.time;
        }
    }
}
