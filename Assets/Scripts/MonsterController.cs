using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public Animator animator;
    public float jumpSpeed;

    bool jumping = false;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Set initial values
        animator.SetBool("Jumping", false);
        animator.SetFloat("YVelocity", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!jumping && Input.GetKeyDown(KeyCode.J))
        {
            jumping = true;
            animator.SetBool("Jumping", true);
            rb.velocity = new Vector3(0, jumpSpeed, 0);
        }
    }

    // Called once per physics update
    private void FixedUpdate()
    {
        animator.SetFloat("YVelocity", rb.velocity.y);

        // Stop jumping if on ground
        if (jumping && rb.velocity.y == 0)
        {
            jumping = false;
            animator.SetBool("Jumping", false);
        }
    }
}
