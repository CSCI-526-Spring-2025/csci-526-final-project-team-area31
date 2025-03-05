using UnityEngine;
using System.Collections;

public class TutorialScript : MonoBehaviour
{
    public GameObject tutorialCanvas;

    void Start()
    {
        //StartCoroutine(ShowTutorial());
    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player1")) // Make sure the player has the "Player" tag
        {
            Debug.Log("player is in tutorail so turn on the UI");
            tutorialCanvas.SetActive(true); // Show tutorial UI when entering the room
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player1"))
        {
            tutorialCanvas.SetActive(false); // Hide tutorial UI when leaving the room
        }
    }
    // IEnumerator ShowTutorial()
    // {
    //     tutorialCanvas.SetActive(true); 
    //     yield return new WaitForSeconds(5f); 
    //     tutorialCanvas.SetActive(false); 
    // }
}


// public class TutorialScript : MonoBehaviour
// {
//     public GameObject tutorialCanvas;
//     private bool isPlayerInTutorialRoom = false; // Track if the player is inside the room

//     void Start()
//     {
//         tutorialCanvas.SetActive(false); // Ensure it's hidden at the start
//     }

//     private void OnTriggerEnter2D(Collider2D other)
//     {

//         if (other.CompareTag("Player1")) // Make sure the player has the "Player" tag
//         {
//             Debug.Log("player is in tutorail so turn on the UI");
//             isPlayerInTutorialRoom = true;
//             tutorialCanvas.SetActive(true); // Show tutorial UI when entering the room
//         }
//     }

//     private void OnTriggerExit2D(Collider2D other)
//     {
//         if (other.CompareTag("Player1"))
//         {
//             isPlayerInTutorialRoom = false;
//             tutorialCanvas.SetActive(false); // Hide tutorial UI when leaving the room
//         }
//     }
// }

