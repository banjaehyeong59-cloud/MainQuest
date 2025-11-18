using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private AudioClip collectSound;

    [Header("Magnet Effect (Optional)")]
    [SerializeField] private float magnetSpeed = 10f; // 아이템이 날아오는 속도
    private bool isAttracted = false;      // 현재 끌려가는 중인지?
    private Transform playerTransform;

    public void StartAttraction(Transform target)
    {
        isAttracted = true;
        playerTransform = target;
    }

    void Update()
    {
        if (isAttracted && playerTransform != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, magnetSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }

    private void Collect()
    {
        if (collectSound != null)
        {
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
        }

        GameManager.Instance.CollectItem();

        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        // 아이템이 다시 창고로 돌아갈 때, 변수들 초기화
        isAttracted = false;
        playerTransform = null;
    }
}