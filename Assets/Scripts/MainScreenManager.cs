using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneManager : MonoBehaviour
{
    public void LoadGameScene()
    {
        SceneManager.LoadScene("MazeScene_new"); 
    }
}