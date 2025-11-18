using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [SerializeField] private Transform playerBody;
    [SerializeField] private float mouseSensitivity = 2f; // 마우스 감도

    private float pitch = 0f; // 카메라의 상하 회전 각도

    void Start()
    {
        // 게임 시작 시 마우스 커서를 잠그고 숨김
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // 플레이어 몸체 좌우 회전 (Y축 기준)
        playerBody.Rotate(Vector3.up * mouseX);

        // 카메라 상하 회전 (X축 기준)
        pitch -= mouseY;
        // 상하 회전 각도를 80도 ~ -80도로 제한
        pitch = Mathf.Clamp(pitch, -80f, 80f);

        // 카메라의 localRotation을 변경 (상하 회전만)
        transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }
}