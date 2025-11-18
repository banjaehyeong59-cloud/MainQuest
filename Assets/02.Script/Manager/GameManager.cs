using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game State")]
    [SerializeField] private int totalItemCount = 10;
    private int currentItemCount = 0;

    [Header("Timer")]
    [SerializeField] private TextMeshProUGUI currentTimeText;
    [SerializeField] private TextMeshProUGUI bestTimeText;

    private float elapsedTime = 0f;
    private float bestTime = float.MaxValue;
    private bool isGameActive = false;
    private const string BestTimeKey = "BestTime";

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject clearPanel;

    [Header("Object Pooling")]
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private int poolSize = 20;
    private List<GameObject> itemPool;

    [Header("Spawning")]
    [SerializeField] private Transform[] spawnPoints;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
        InitializeObjectPool();
    }

    void Start()
    {
        currentItemCount = 0;
        clearPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        elapsedTime = 0f;
        isGameActive = true;
        LoadBestTime();

        SpawnInitialItems();
    }

    void Update()
    {
        // 게임이 활성화 상태일 때만 시간 증가
        if (isGameActive)
        {
            elapsedTime += Time.deltaTime;
            UpdateCurrentTimeUI();
        }
    }

    // 풀링 및 스폰 함수
    void InitializeObjectPool()
    {
        itemPool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject item = Instantiate(itemPrefab, transform);
            item.SetActive(false);
            itemPool.Add(item);
        }
    }
    public GameObject GetPooledItem()
    {
        for (int i = 0; i < itemPool.Count; i++)
        {
            if (!itemPool[i].activeInHierarchy) return itemPool[i];
        }
        return null;
    }
    void SpawnInitialItems()
    {
        List<Transform> availableSpawnPoints = new List<Transform>(spawnPoints);
        if (availableSpawnPoints.Count < totalItemCount)
        {
            Debug.LogError("스폰 포인트가 목표 아이템 개수보다 적습니다!");
            totalItemCount = availableSpawnPoints.Count;
        }
        for (int i = 0; i < totalItemCount; i++)
        {
            GameObject item = GetPooledItem();
            if (item != null)
            {
                int randomIndex = Random.Range(0, availableSpawnPoints.Count);
                Transform spawnPoint = availableSpawnPoints[randomIndex];
                availableSpawnPoints.RemoveAt(randomIndex); // 중복 방지
                item.transform.position = spawnPoint.position;
                item.SetActive(true);
            }
        }
        UpdateScoreUI();
    }

    // 아이템 수집
    public void CollectItem()
    {
        if (!isGameActive) return;

        currentItemCount++;
        UpdateScoreUI();

        if (currentItemCount >= totalItemCount)
        {
            ShowClearPanel();
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Items: " + currentItemCount + " / " + totalItemCount;
        }
    }

    private void ShowClearPanel()
    {
        // 타이머 정지
        isGameActive = false;

        clearPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // 최고 기록 확인 및 저장
        CheckAndSaveBestTime();
    }



    private void UpdateCurrentTimeUI()
    {
        if (currentTimeText != null)
        {
            // 초 를 "0.00" 형식의 문자열로 변환
            currentTimeText.text = "시간: " + elapsedTime.ToString("F2");
        }
    }

    private void LoadBestTime()
    {
        // 저장된 최고 기록을 불러오고, 없으면 무한대를 사용
        bestTime = PlayerPrefs.GetFloat(BestTimeKey, float.MaxValue);
        UpdateBestTimeUI();
    }

    private void UpdateBestTimeUI()
    {
        if (bestTimeText != null)
        {
            // 저장된 기록이 없으면 무한
            if (bestTime == float.MaxValue)
            {
                bestTimeText.text = "최고 기록: --.--";
            }
            else
            {
                // 저장된 기록을 "0.00" 형식으로 표시
                bestTimeText.text = "최고 기록: " + bestTime.ToString("F2");
            }
        }
    }

    // 게임 클리어 시 시간 비교 및 저장
    private void CheckAndSaveBestTime()
    {
        if (elapsedTime < bestTime)
        {
            bestTime = elapsedTime; // 최고 기록 갱신
            PlayerPrefs.SetFloat(BestTimeKey, bestTime);
            PlayerPrefs.Save();
            UpdateBestTimeUI(); //
        }
    }

    public void RestartGame()
    {
        isGameActive = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}