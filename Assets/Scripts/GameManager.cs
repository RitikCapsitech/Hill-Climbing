using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Player")]
    public CarController carController;

    [Header("Environment & Coins")]
    public EnvironmentSpawner environmentSpawner;
    
    [Header("UI Panels")]
    public GameObject StartPanel;
    public GameObject GameWonPanel;
    public GameObject RetryPanel;

    [Header("Buttons")]
    public GameObject PauseButton;
    public GameObject ResumeButton;

    [Header("UI")]
    public TextMeshProUGUI checkpointText;
    public TextMeshProUGUI scoreText;

    private int score = 0;
    private int currentCheckpoint = 0;
    public static bool gameStarted = false;
    private bool isPaused = false;

    private Vector3 lastSafePosition;
    private Quaternion lastSafeRotation;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UpdateScoreUI();
    }

    public void UpdateLastSafePoint(Vector3 pos, Quaternion rot)
    {
        lastSafePosition = pos;
        lastSafeRotation = rot;
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        gameStarted = true;

        StartPanel.SetActive(false);

        environmentSpawner.spawnEnvironment();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        gameStarted = true;

        if (GameWonPanel != null && GameWonPanel.activeSelf)
            GameWonPanel.SetActive(false);
        if (RetryPanel != null && RetryPanel.activeSelf)
            RetryPanel.SetActive(false);

        carController.ResetCar();

        score = 0;
        UpdateScoreUI();

        currentCheckpoint = 0;
        checkpointText?.gameObject.SetActive(false);

        foreach (GameObject c in GameObject.FindGameObjectsWithTag("Coin"))
            Destroy(c);

        CoinSpawner coinSpawner = FindObjectOfType<CoinSpawner>(); coinSpawner.RespawnCoins();

        environmentSpawner.ClearEnvironment();
        environmentSpawner.spawnEnvironment();
    }

    public void RetryFromLastPosition()
    {
        Time.timeScale = 1f;
        gameStarted = true;

        RetryPanel.SetActive(false);

       
        carController.RetryFromLastSafe(lastSafePosition, lastSafeRotation);
    }

    public void GameOver()
    {
        gameStarted = false;
        RetryPanel.SetActive(true);
    }

    public void GameWon()
    {
        GameWonPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void PauseResumeGame()
    {
        if (!isPaused)
        {
            Time.timeScale = 0f;
            PauseButton.SetActive(true);
            ResumeButton.SetActive(false);
            isPaused = true;
        }
        else
        {
            Time.timeScale = 1f;
            ResumeButton.SetActive(true);
            PauseButton.SetActive(false);
            isPaused = false;
        }
    }

    public void CheckpointCrossed(int num)
    {
        if (num == currentCheckpoint + 1)
        {
            currentCheckpoint = num;
            StartCoroutine(ShowCheckpointMessage(num));
        }
    }

    private IEnumerator ShowCheckpointMessage(int n)
    {
        checkpointText.text = "Checkpoint " + n + " Crossed!";
        checkpointText.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);
        checkpointText.gameObject.SetActive(false);
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = score.ToString();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); 
#endif
    }
}