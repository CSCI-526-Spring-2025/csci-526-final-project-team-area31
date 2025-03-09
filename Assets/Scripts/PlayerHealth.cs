using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Health : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int health = 100;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI deathText;
    public GameObject restartButton;
    void Start()
    {
        healthText.color = Color.white;
        healthText.text = "Health: " + health.ToString();
        deathText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = "Health: " + health.ToString();
        if (health <= 0)
        {
            deathText.text = "YOU DIED! ";
            Debug.Log("Health is: " + health); // Remove the battery item after pickup

            if (restartButton != null)
            {
                restartButton.SetActive(true); // Show the restart button
            }
        }
    }

 
}