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
    
    [Header("UI References")]
    public Text scoreText; // UI text to display score
    public Text livesText; // UI text to display lives
    public GameObject gameOverPanel; // Panel to show when game ends
    public Text finalScoreText; // Text to show final score
    
    private int currentScore = 0;
    private int currentLives;
    private bool gameActive = true;
    
    // Start is called before the first frame update
    void Start()
    {
        currentLives = startingLives;
        UpdateUI();
        
        // Hide game over panel initially
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        
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
        
        // Check if we caught an apple
        if (other.CompareTag("Apple"))
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
        
        // Add points
        currentScore += pointsPerApple;
        
        // Destroy the apple
        Destroy(apple);
        
        // Update UI
        UpdateUI();
        
        // Optional: Play sound effect here
        // AudioSource.PlayClipAtPoint(catchSound, transform.position);
    }
    
    public void MissApple()
    {
        if (!gameActive) return;
        
        // Lose a life
        currentLives--;
        
        // Update UI
        UpdateUI();
        
        // Check if game is over
        if (currentLives <= 0)
        {
            GameOver();
        }
        
        // Optional: Play sound effect here
        // AudioSource.PlayClipAtPoint(missSound, transform.position);
    }
    
    void UpdateUI()
    {
        // Update score text
        if (scoreText != null)
            scoreText.text = "Score: " + currentScore.ToString();
        
        // Update lives text
        if (livesText != null)
            livesText.text = "Lives: " + currentLives.ToString();
    }
    
    void GameOver()
    {
        gameActive = false;
        
        // Show game over panel
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        
        // Show final score
        if (finalScoreText != null)
        {
            finalScoreText.text = "Game Over! Final Score: " + currentScore.ToString();
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
        
        // Update UI
        UpdateUI();
        
        // Optional: Destroy all remaining apples
        GameObject[] apples = GameObject.FindGameObjectsWithTag("Apple");
        foreach (GameObject apple in apples)
        {
            Destroy(apple);
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
}