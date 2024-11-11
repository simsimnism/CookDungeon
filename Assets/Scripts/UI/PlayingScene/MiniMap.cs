using Cinemachine;
using UnityEngine;

[DisallowMultipleComponent]
public class Minimap : MonoBehaviour
{
    #region Tooltip
    [Tooltip("Populate with the child MinimapPlayer gameobject")]
    #endregion Tooltip

    // 플레이어 아이콘 설정
    // [SerializeField] private GameObject miniMapPlayer;

    private Transform playerTransform;

    private void Start()
    {
        // playerTransform = Managers.GM.GetPlayer().transform;

        // 플레이어를 시네머신 대상으로 채웁니다.
        CinemachineVirtualCamera cinemachineVirtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        cinemachineVirtualCamera.Follow = playerTransform;

        // 미니맵에 플레이어 아이콘 띄우기
    }

    private void Update()
    {
        // 미니맵 플레이어가 플레이어를 따라가도록 이동
        //if (playerTransform != null && miniMapPlayer != null)
        //{
        //    miniMapPlayer.transform.position = playerTransform.position;
        //}
    }

    #region Validation

#if UNITY_EDITOR

    private void OnValidate()
    {
        // HelperUtilities.ValidateCheckNullValue(this, nameof(miniMapPlayer), miniMapPlayer);
    }

#endif

    #endregion Validation

}