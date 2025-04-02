using System.Collections;
using UnityEngine;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    public TextMeshProUGUI speechText; // assign this in the Inspector
    public float typingSpeed = 0.05f;

    public IEnumerator TypeText(string message)
    {
        speechText.text = "";

        for (int i = 0; i <= message.Length; i++)
        {
            speechText.text = message.Substring(0, i);
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
