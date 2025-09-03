using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleTree : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f; // Speed of tree movement
    public float boundaryOffset = 20f; // How far from center the tree can move
    [Header("Apple Settings")]
    public GameObject applePrefab;
    [Header("Basket Settings")]
    public GameObject basketPrefab;
    
    private bool movingRight = true; // Track current direction
    private float appleDropTimer = 0f; // Timer for apple dropping
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Move the tree
        MoveTree();
        
        // Check if we should change direction (0.0009% chance)
        if (Random.Range(0f, 1f) < 0.0015f)
        {
            ChangeDirection();
        }
        
        // Check boundaries and change direction if needed
        CheckBoundaries();
        
        // Handle apple dropping every second
        DropApple();
        
        // Safety check: ensure tree is always moving
        if (Mathf.Abs(transform.position.x) > boundaryOffset + 1f)
        {
            Debug.LogWarning("Tree went beyond boundaries! Resetting position.");
            Vector3 resetPos = transform.position;
            resetPos.x = Mathf.Clamp(resetPos.x, -boundaryOffset, boundaryOffset);
            transform.position = resetPos;
        }
    }
    
    void MoveTree()
    {
        // Calculate movement direction
        Vector3 movement = Vector3.right * speed * Time.deltaTime;
        
        if (!movingRight)
        {
            movement = -movement; // Move left
        }
        
        // Apply movement
        transform.Translate(movement);
        
        // Debug: Log movement occasionally
        if (Random.Range(0f, 1f) < 0.000001f) // 0.0001% chance per frame
        {
           Debug.Log($"Tree moving: {(movingRight ? "Right" : "Left")} at position: {transform.position.x:F2}");
        }
    }
    
    void ChangeDirection()
    {
        movingRight = !movingRight;
    }
    
    void CheckBoundaries()
    {
        // If tree goes too far left, make it go right
        if (transform.position.x <= -boundaryOffset)
        {
            movingRight = true;
            // Ensure tree doesn't get stuck by moving it slightly inside boundary
            Vector3 newPos = transform.position;
            newPos.x = -boundaryOffset + 0.1f;
            transform.position = newPos;
        }
        // If tree goes too far right, make it go left
        else if (transform.position.x >= boundaryOffset)
        {
            movingRight = false;
            // Ensure tree doesn't get stuck by moving it slightly inside boundary
            Vector3 newPos = transform.position;
            newPos.x = boundaryOffset - 0.1f;
            transform.position = newPos;
        }
    }
    
    void DropApple()
    {
        // Increment timer
        appleDropTimer += Time.deltaTime;
        
        // Check if 0.5 seconds has passed
        if (appleDropTimer >= 0.4f)
        {
            // Reset timer
            appleDropTimer = 0f;
            
            // 94% chance to drop an apple (fixed the logic)
            if (Random.Range(0f, 1f) < 0.94f)
            {
                // Spawn apple at tree's position
                if (applePrefab != null)
                {
                    Vector3 applePosition = transform.position;
                    applePosition.y -= 1.5f; // Drop apple slightly below the tree
                    Instantiate(applePrefab, applePosition, Quaternion.identity);
                }
            }
        }
    }
}
