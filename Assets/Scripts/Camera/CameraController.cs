using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using static UnityEditor.Experimental.GraphView.GraphView;

[RequireComponent(typeof(CinemachineTargetGroup))]

public class CameraController : MonoBehaviour
{
    private CinemachineVirtualCamera vcam;
    private CinemachineTargetGroup cinemachineTargetGroup;

    Transform PlayerTransform;

    void Awake()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        cinemachineTargetGroup = GetComponent<CinemachineTargetGroup>();
    }

    void Start()
    {
        PlayerTransform = GameObject.FindWithTag("Player").transform;
        vcam.Follow = PlayerTransform;
    }
}

