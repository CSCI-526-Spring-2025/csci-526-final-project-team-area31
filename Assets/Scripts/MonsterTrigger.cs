using UnityEngine;

public class MonsterTrigger : MonoBehaviour
{
    public GameObject monsterTutorial;
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("monster tirgger ");
        monsterTutorial.SetActive(true); 
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player1"))
        {
           monsterTutorial.SetActive(false); 
        }
    }
}
