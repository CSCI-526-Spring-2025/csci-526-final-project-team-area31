using UnityEngine;
using System.Collections;

public class TutorialTrigger : MonoBehaviour
{
    public GameObject trapTutorial;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player1"))
        {
            Debug.Log("Monster tirgger ");
            trapTutorial.SetActive(true); 
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player1"))
        {
           trapTutorial.SetActive(false); 
        }
    }
}
