using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesCounter : MonoBehaviour
{
    [Header("References")]
    public Basket basketScript; // Reference to the Basket script
    public Text livesText; // Text component to display the lives
    
    [Header("Display Settings")]
    public string livesPrefix = "Lives: "; // Text to show before the lives count
    public bool updateContinuously = true; // Whether to update every frame or only when lives change
    
    private int lastKnownLives = -1; // Track the last lives count to detect changes
    
    // Start is called before the first frame update
    void Start()
    {
        // If no basket reference is assigned, try to find it automatically
        if (basketScript == null)
        {
            basketScript = FindObjectOfType<Basket>();
            if (basketScript == null)
            {
                Debug.LogError("LivesCounter: No Basket script found! Please assign one in the inspector.");
                return;
            }
            else
            {
                Debug.Log("LivesCounter: Found Basket script automatically.");
            }
        }
        
        // If no text component is assigned, try to get it from this GameObject
        if (livesText == null)
        {
            livesText = GetComponent<Text>();
            if (livesText == null)
            {
                Debug.LogError("LivesCounter: No Text component found! Please assign one in the inspector.");
                return;
            }
            else
            {
                Debug.Log("LivesCounter: Found Text component on this GameObject.");
            }
        }
        
        // Initial lives update
        UpdateLivesDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        if (updateContinuously)
        {
            UpdateLivesDisplay();
        }
        else
        {
            // Only update when lives change
            if (basketScript != null)
            {
                int currentLives = basketScript.GetCurrentLives();
                if (currentLives != lastKnownLives)
                {
                    UpdateLivesDisplay();
                    lastKnownLives = currentLives;
                }
            }
        }
    }
    
    // Method to update the lives display
    public void UpdateLivesDisplay()
    {
        if (basketScript != null && livesText != null)
        {
            int currentLives = basketScript.GetCurrentLives();
            livesText.text = livesPrefix + currentLives.ToString();
        }
    }
    
    // Method to manually refresh the lives (can be called from other scripts)
    public void RefreshLives()
    {
        UpdateLivesDisplay();
    }
}
