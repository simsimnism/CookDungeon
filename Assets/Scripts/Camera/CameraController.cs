using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

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
    // 카메라의 2D 위치를 반환합니다.
    public Vector3 GetCameraPosition()
    {
        return new Vector3(vcam.transform.position.x, vcam.transform.position.y, 0); // Z축은 0으로 설정
    }
}

