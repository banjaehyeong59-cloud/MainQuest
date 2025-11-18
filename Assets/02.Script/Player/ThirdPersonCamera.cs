using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float distance = 5.0f;
    [SerializeField] private float mouseSensitivity = 5f;

    [SerializeField] private float pitchMin = -30f;
    [SerializeField] private float pitchMax = 70f;

    // 현재 카메라 회전 각도
    private float yaw = 0f;   // 좌,우
    private float pitch = 0f; // 상,하

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // 마우스 입력으로 yaw, pitch 값 누적
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;

        // pitch(상하) 각도 제한
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        Vector3 desiredPosition = target.position - (rotation * Vector3.forward * distance);

        transform.position = desiredPosition;
        transform.LookAt(target.position);
    }
}