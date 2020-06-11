using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Inventory inventory;

    private Rigidbody2D rb;
    private Vector2 movement;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Only update inputs if playing
        if (GameManager.Instance.Playing)
        {
            movement = Vector2.zero;

            // Horizontal
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                movement.x = -1;
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                movement.x = 1;
            }

            // Vertical
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                movement.y = 1;
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                movement.y = -1;
            }

            // Normalise the movement, multiply by speed and set it
            movement = movement.normalized * speed;
        }
    }

    // Called once per physics update
    private void FixedUpdate()
    {
        // Only add force if the game is playing
        if (GameManager.Instance.Playing)
        {
            rb.AddForce(movement);
        }
    }

    // Called when the player enters a trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Get a reference to the entity
        Entity entity = collision.GetComponent<Entity>();

        // Check if its an entity and collectable
        if (entity && entity.description.collectable)
        {
            // Add to inventory and delete it from scene
            inventory.Add(entity.description);
            entity.Remove();
        }
    }

    // Resets the player to its initial state
    public void Reset()
    {
        inventory.Clear();
    }
}
