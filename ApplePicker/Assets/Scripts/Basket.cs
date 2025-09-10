using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // For UI elements

public class Basket : MonoBehaviour
{
    [Header("Basket Settings")]
    public float mouseSensitivity = 1f; // How responsive the basket is to mouse movement
    
    [Header("Game Settings")]
    public int startingLives = 3; // Starting number of baskets/lives
    public int pointsPerApple = 10; // Points earned per apple caught
    public int pointsPerGoldenApple = 100; // Points earned per golden apple caught (10x regular)
    
    [Header("UI References")]
    public GameObject gameOverPanel; // Panel to show when game ends
    
    [Header("Button References")]
    public Button restartButton; // Button to restart the game
    public Button exitButton; // Button to exit the game
    
    private static int currentScore = 0;
    private int currentLives;
    private bool gameActive = true;
    
    // Start is called before the first frame update
    void Start()
    {
        currentLives = startingLives;
        
        // Hide game over panel initially
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        
        // Set up button listeners
        SetupButtons();
        
        // Debug: Check basket setup
        CheckBasketSetup();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameActive)
        {
            MoveBasketWithMouse();
        }
    }
    
    void MoveBasketWithMouse()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        
        // Move basket horizontally based on mouse movement
        Vector3 newPosition = transform.position;
        newPosition.x += mouseX;
        
        // Apply movement
        transform.position = newPosition;
    }
    
    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Trigger entered by: " + other.name + " with tag: " + other.tag);
        
        if (!gameActive) return;
        
        // Check if we caught an apple or golden apple
        if (other.CompareTag("Apple") || other.CompareTag("GoldenApple"))
        {
            //Debug.Log("Apple caught! Tag matches: " + other.tag);
            CatchApple(other.gameObject);
        }
        else
        {
            //Debug.Log("Object entered trigger but tag doesn't match 'Apple'. Tag is: " + other.tag);
        }
    }
    
    public void CatchApple(GameObject apple)
    {
        //Debug.Log("CatchApple called! Destroying apple: " + apple.name);
        
        // Check if it's a golden apple and add appropriate points
        if (apple.CompareTag("GoldenApple"))
        {
            currentScore += pointsPerGoldenApple;
            Debug.Log("Golden apple caught! +" + pointsPerGoldenApple + " points!");
        }
        else
        {
            currentScore += pointsPerApple;
        }
        
        // Destroy the apple
        Destroy(apple);
        
        // Optional: Play sound effect here
        // AudioSource.PlayClipAtPoint(catchSound, transform.position);
    }
    
    public void MissApple()
    {
        if (!gameActive) return;
        
        // Lose a life
        currentLives--;
        
        // Check if game is over
        if (currentLives <= 0)
        {
            GameOver();
        }
        
        // Optional: Play sound effect here
        // AudioSource.PlayClipAtPoint(missSound, transform.position);
    }
    
    void GameOver()
    {
        gameActive = false;
        
        // Show game over panel
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        
        // Optional: Pause the game or show restart button
        Time.timeScale = 0f;
    }
    
    // Public method to restart the game (can be called from UI button)
    public void RestartGame()
    {
        // Reset game state
        currentScore = 0;
        currentLives = startingLives;
        gameActive = true;
        
        // Hide game over panel
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        
        // Resume time
        Time.timeScale = 1f;
        
        // Optional: Destroy all remaining apples and golden apples
        GameObject[] apples = GameObject.FindGameObjectsWithTag("Apple");
        foreach (GameObject apple in apples)
        {
            Destroy(apple);
        }
        
        GameObject[] goldenApples = GameObject.FindGameObjectsWithTag("GoldenApple");
        foreach (GameObject goldenApple in goldenApples)
        {
            Destroy(goldenApple);
        }
    }
    
    void CheckBasketSetup()
    {
        // Check if basket has a collider
        Collider basketCollider = GetComponent<Collider>();
        if (basketCollider == null)
        {
            Debug.LogError("BASKET ERROR: No Collider component found on basket!");
        }
        else
        {
            Debug.Log("Basket collider found: " + basketCollider.GetType().Name);
            if (basketCollider.isTrigger)
            {
                Debug.Log("Basket collider is set as TRIGGER ✓");
            }
            else
            {
                Debug.LogError("BASKET ERROR: Collider is NOT set as trigger! Check 'Is Trigger' checkbox");
            }
        }
        
        // Check basket position
        //Debug.Log("Basket position: " + transform.position);
        //Debug.Log("Basket scale: " + transform.localScale);
    }
    
    void SetupButtons()
    {
        // Set up restart button
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }
        
        // Set up exit button
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(ExitGame);
        }
    }
    
    // Method to exit the game
    public void ExitGame()
    {
        Debug.Log("Exiting game...");
        
        #if UNITY_EDITOR
            // If running in Unity Editor, stop play mode
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // If running as a built game, quit the application
            Application.Quit();
        #endif
    }
    
    // Public method to get the current score (for other scripts to access)
    public int GetCurrentScore()
    {
        return currentScore;
    }
    
    // Public method to get the current lives (for other scripts to access)
    public int GetCurrentLives()
    {
        return currentLives;
    }
    
    // Public method to check if the game is currently active
    public bool IsGameActive()
    {
        return gameActive;
    }
}