using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public void SelectLevel(int levelIndex)
    {
        GameManager.respawnTargetLevelIndex = levelIndex;
        SceneManager.LoadScene("MazeScene_new"); 
    }
}
