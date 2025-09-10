using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalScoreCount : MonoBehaviour
{
    [Header("References")]
    public Basket basketScript; // Reference to the Basket script
    public Text finalScoreText; // Text component to display the final score
    
    [Header("Display Settings")]
    public string finalScorePrefix = "Game Over! Final Score: "; // Text to show before the final score
    public bool hideDuringGameplay = true; // Whether to hide the text during gameplay
    
    private bool gameWasActive = true; // Track if game was active last frame
    
    // Start is called before the first frame update
    void Start()
    {
        // If no basket reference is assigned, try to find it automatically
        if (basketScript == null)
        {
            basketScript = FindObjectOfType<Basket>();
            if (basketScript == null)
            {
                Debug.LogError("FinalScoreCount: No Basket script found! Please assign one in the inspector.");
                return;
            }
            else
            {
                Debug.Log("FinalScoreCount: Found Basket script automatically.");
            }
        }
        
        // If no text component is assigned, try to get it from this GameObject
        if (finalScoreText == null)
        {
            finalScoreText = GetComponent<Text>();
            if (finalScoreText == null)
            {
                Debug.LogError("FinalScoreCount: No Text component found! Please assign one in the inspector.");
                return;
            }
            else
            {
                Debug.Log("FinalScoreCount: Found Text component on this GameObject.");
            }
        }
        
        // Hide the final score text initially if we should hide during gameplay
        // if (hideDuringGameplay && finalScoreText != null)
        // {
        //     finalScoreText.gameObject.SetActive(false);
        // }
    }

    // Update is called once per frame
    void Update()
    {
        if (basketScript != null)
        {
            // bool gameIsActive = basketScript.IsGameActive();
            
            // // Check if game just ended
            // if (gameWasActive && !gameIsActive)
            // {
            //     ShowFinalScore();
            // }
            // // Check if game just started (restarted)
            // else if (!gameWasActive && gameIsActive)
            // {
            //     HideFinalScore();
            // }
            
            // gameWasActive = gameIsActive;

            ShowFinalScore();
        }
    }
    
    // Method to show the final score
    public void ShowFinalScore()
    {
        if (basketScript != null && finalScoreText != null)
        {
            int finalScore = basketScript.GetCurrentScore();
            finalScoreText.text = finalScorePrefix + finalScore.ToString();
            finalScoreText.gameObject.SetActive(true);
        }
    }
    
    // Method to hide the final score
    public void HideFinalScore()
    {
        if (finalScoreText != null)
        {
            finalScoreText.gameObject.SetActive(false);
        }
    }
    
    // Method to manually refresh the final score (can be called from other scripts)
    public void RefreshFinalScore()
    {
        if (basketScript != null && !basketScript.IsGameActive())
        {
            ShowFinalScore();
        }
    }
}
