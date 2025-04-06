using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;
using UnityEngine.Networking;

public class GameEventManager : MonoBehaviour
{
    [SerializeField]
    private string firebaseURL = "https://area31-9329c-default-rtdb.firebaseio.com/";

    private string sessionID;

    private bool isPaused = false;
    public GameObject pauseMenuUI; 
    public GameObject tutorialUI;
    void Start()
    {
        // Ensure each browser session has a single, persistent session ID
        if (!PlayerPrefs.HasKey("SessionID"))
        {
            string newSessionId = System.Guid.NewGuid().ToString();
            PlayerPrefs.SetString("SessionID", newSessionId);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame(){
        pauseMenuUI.SetActive(true);
        //tutorialUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        //tutorialUI.SetActive(false);
        Time.timeScale = 1f; 
        isPaused = false;
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        if (GameManager.Instance != null)
            {
                GameManager.respawnTargetLevelIndex = GameManager.Instance.GetCurrentLevelIndex();
                Debug.Log($"Storing target level index for post-reload respawn: {GameManager.respawnTargetLevelIndex}");
            }
            else
            {
                GameManager.respawnTargetLevelIndex = -1; // Set to invalid if no GameManager found
                Debug.LogWarning("GameManager instance not found before reload. Cannot store target respawn index.");
            }
        StartCoroutine(SendRestartEvent());

        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 

        
    }

    IEnumerator SendRestartEvent()
    {
        string sessionId = PlayerPrefs.GetString("SessionID");

        // Increment restart count in Firebase under this session ID
        string firebaseURL = "https://area31-9329c-default-rtdb.firebaseio.com/gameRestarts/" + sessionId + ".json";

        string jsonData = "{\"timestamp\":\"" + System.DateTime.UtcNow.ToString("o") + "\"}";

        using (UnityWebRequest uwr = new UnityWebRequest(firebaseURL, "POST"))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
            uwr.uploadHandler = new UploadHandlerRaw(jsonToSend);
            uwr.downloadHandler = new DownloadHandlerBuffer();
            uwr.SetRequestHeader("Content-Type", "application/json");

            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
                Debug.LogError("Firebase Error: " + uwr.error);
            else
                Debug.Log("Restart Event Logged Successfully.");
        }

        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}