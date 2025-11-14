using UnityEngine;

// [RequireComponent]: 실수로 CharacterController 컴포넌트를 안 넣는 것을 방지합니다.
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("설정")]
    // [SerializeField]: private 변수지만 유니티 에디터 창에서 조절할 수 있게 해줍니다.
    [Tooltip("플레이어의 이동 속도입니다.")]
    [SerializeField] private float moveSpeed = 5.0f;

    // 이동을 담당할 핵심 부품
    private CharacterController _controller;

    void Start()
    {
        // 내 몸에 붙어있는 CharacterController를 찾아서 기억해둡니다.
        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // 1. 키보드 입력 받기 (WASD / 화살표)
        float h = Input.GetAxis("Horizontal"); // 좌우 입력 (-1 ~ 1)
        float v = Input.GetAxis("Vertical");   // 위아래 입력 (-1 ~ 1)

        // 2. 이동 방향 만들기 (y가 0인 이유는 점프가 아니라 바닥 이동이라서)
        Vector3 moveDir = new Vector3(h, 0, v);

        // 3. 실제 이동시키기
        // SimpleMove는 중력을 알아서 처리해줘서 코드가 간단해집니다.
        _controller.SimpleMove(moveDir * moveSpeed);
    }
}