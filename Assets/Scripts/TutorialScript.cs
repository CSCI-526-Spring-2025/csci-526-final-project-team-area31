using UnityEngine;
using System.Collections;

public class TutorialScript : MonoBehaviour
{
    public GameObject tutorialCanvas;

    void Start()
    {
        StartCoroutine(ShowTutorial());
    }

    IEnumerator ShowTutorial()
    {
        tutorialCanvas.SetActive(true); 
        yield return new WaitForSeconds(5f); 
        tutorialCanvas.SetActive(false); 
    }
}
