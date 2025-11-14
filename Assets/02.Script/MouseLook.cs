using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Header("설정")]
    [Tooltip("마우스 감도입니다.")]
    [SerializeField] private float mouseSensitivity = 500f;

    [Tooltip("좌우 회전을 위해 돌릴 플레이어 몸통입니다.")]
    [SerializeField] private Transform playerBody;

    private float xRotation = 0f; // 위아래 회전값 누적 변수

    void Start()
    {
        // 게임 시작 시 마우스 커서를 화면 중앙에 고정하고 숨깁니다. (FPS 필수)
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // 1. 마우스 입력 받기
        // Time.deltaTime을 곱해서 프레임 저하가 있어도 회전 속도가 일정하게 보정합니다.
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // 2. 위아래(X축) 회전 계산
        // 마우스를 위로 올리면 시선은 위를 봐야 하므로, Rotation X값은 감소해야 합니다. (직관과 반대)
        xRotation -= mouseY;

        // 3. 고개가 뒤로 꺾이지 않게 제한 (Clamp)
        // -90도(바닥) ~ 90도(하늘) 사이로 가둡니다.
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // 4. 카메라 회전 (위아래)
        // 쿼터니언(Quaternion)은 3D 회전을 담당하는 수학 개념인데, 일단 공식처럼 외우셔도 됩니다.
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // 5. 몸통 회전 (좌우)
        // 카메라는 위아래만 돌리고, 좌우 회전은 몸통 자체를 돌립니다.
        playerBody.Rotate(Vector3.up * mouseX);
    }
}