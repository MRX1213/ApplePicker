using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    [Header("References")]
    public Basket basketScript; // Reference to the Basket script
    public Text scoreText; // Text component to display the score
    
    [Header("Display Settings")]
    public string scorePrefix = "Score: "; // Text to show before the score
    public bool updateContinuously = true; // Whether to update every frame or only when score changes
    
    private int lastKnownScore = -1; // Track the last score to detect changes
    
    // Start is called before the first frame update
    void Start()
    {
        // If no basket reference is assigned, try to find it automatically
        if (basketScript == null)
        {
            basketScript = FindObjectOfType<Basket>();
            if (basketScript == null)
            {
                Debug.LogError("ScoreCounter: No Basket script found! Please assign one in the inspector.");
                return;
            }
            else
            {
                Debug.Log("ScoreCounter: Found Basket script automatically.");
            }
        }
        
        // If no text component is assigned, try to get it from this GameObject
        if (scoreText == null)
        {
            scoreText = GetComponent<Text>();
            if (scoreText == null)
            {
                Debug.LogError("ScoreCounter: No Text component found! Please assign one in the inspector.");
                return;
            }
            else
            {
                Debug.Log("ScoreCounter: Found Text component on this GameObject.");
            }
        }
        
        // Initial score update
        UpdateScoreDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        if (updateContinuously)
        {
            UpdateScoreDisplay();
        }
        else
        {
            // Only update when score changes
            if (basketScript != null)
            {
                int currentScore = basketScript.GetCurrentScore();
                if (currentScore != lastKnownScore)
                {
                    UpdateScoreDisplay();
                    lastKnownScore = currentScore;
                }
            }
        }
    }
    
    // Method to update the score display
    public void UpdateScoreDisplay()
    {
        if (basketScript != null && scoreText != null)
        {
            int currentScore = basketScript.GetCurrentScore();
            scoreText.text = scorePrefix + currentScore.ToString();
        }
    }
    
    // Method to manually refresh the score (can be called from other scripts)
    public void RefreshScore()
    {
        UpdateScoreDisplay();
    }
}
