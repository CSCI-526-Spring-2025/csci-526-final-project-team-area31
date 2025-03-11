using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Health : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float health = 100;
    public float MaxHealth = 100;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI deathText;
    public GameObject restartButton;
    public Image healthBar;
    public float Width, Height;

    [SerializeField]
    private RectTransform health_;
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
        UpdateHealthUI();
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

    void UpdateHealthUI()
    {
        //Debug.Log("health" + health);
        //Debug.Log("MaxHealth" + MaxHealth);
        //Debug.Log("Width" + Width);
        float newWidth = (health / MaxHealth) * Width;
        //Debug.Log("newWidth" + newWidth);
        health_.sizeDelta = new Vector2(newWidth, Height);
    }


}