using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterCollisionHandler : MonoBehaviour
{
    public GameObject gameOverPanel;  // Drag from the Canvas in Inspector

    void Start()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);  // Hide at start
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player1"))
        {
            TriggerGameOver();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player1"))
        {
            TriggerGameOver();
        }
    }

    void TriggerGameOver()
    {
        Time.timeScale = 0f;  // First, freeze everything
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);  // Then, show Game Over popup
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;  // Unfreeze time when restarting
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // Reload the current scene
    }
}
