using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    [Header("Apple Settings")]
    public float fallSpeed = 5f; // How fast the apple falls
    public float destroyY = -10f; // Y position where apple gets destroyed (off screen)
    
    private Basket basket; // Reference to the basket to notify when apple is missed
    
    void Start()
    {
        // Find the basket in the scene
        basket = FindObjectOfType<Basket>();
        
        // Automatically set the Apple tag
        gameObject.tag = "Apple";
        
        // Debug: Check if tag was set correctly
        Debug.Log("Apple spawned with tag: " + gameObject.tag);
        Debug.Log("Apple position: " + transform.position);
        
        // Check apple components
        Collider appleCollider = GetComponent<Collider>();
        if (appleCollider != null)
        {
            //Debug.Log("Apple has collider: " + appleCollider.GetType().Name);
        }
        else
        {
           // Debug.LogError("APPLE ERROR: No collider found on apple!");
        }
        
        Rigidbody appleRigidbody = GetComponent<Rigidbody>();
        if (appleRigidbody != null)
        {
            //Debug.Log("Apple has rigidbody");
        }
        else
        {
            //Debug.Log("Apple has no rigidbody (this might be needed for collision detection)");
        }
        
        if (basket != null)
        {
            //Debug.Log("Basket found successfully");
        }
        else
        {
            //Debug.Log("ERROR: No basket found in scene!");
        }
    }

    void Update()
    {
        // Make apple fall
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
        
        // Check if apple fell off screen
        if (transform.position.y < destroyY)
        {
            // Notify basket that apple was missed
            if (basket != null)
            {
                basket.MissApple();
            }
            
            // Destroy the apple
            Destroy(gameObject);
        }
    }
    
    // Try both trigger and collision methods
    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Apple OnTriggerEnter with: " + other.name + " (tag: " + other.tag + ")");
        
        if (other.CompareTag("Basket") || other.GetComponent<Basket>() != null)
        {
            Debug.Log("Apple hit basket via trigger!");
            if (basket != null)
            {
                basket.CatchApple(gameObject);
            }
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Apple OnCollisionEnter with: " + collision.gameObject.name + " (tag: " + collision.gameObject.tag + ")");
        
        if (collision.gameObject.CompareTag("Basket") || collision.gameObject.GetComponent<Basket>() != null)
        {
            Debug.Log("Apple hit basket via collision!");
            if (basket != null)
            {
                basket.CatchApple(gameObject);
            }
        }
    }
}
