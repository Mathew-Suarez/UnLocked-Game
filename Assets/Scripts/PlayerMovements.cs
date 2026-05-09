using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    public float speed = 5;
    public int facingDirection = 1;

    public Rigidbody2D rb;
    public Animator anim;

    public Player_Combat player_Combat;

    private void Update()
    {
        if (Input.GetButtonDown("Slash"))
        {
            player_Combat.Attack();
        }
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal"); // Using GetAxisRaw for "snappier" ICT movement
        float vertical = Input.GetAxisRaw("Vertical");

        // 1. Create a direction vector
        Vector2 moveDirection = new Vector2(horizontal, vertical);

        // 2. NORMALIZE IT
        // This makes sure the diagonal length is 1, not 1.41
        if (moveDirection.magnitude > 1)
        {
            moveDirection.Normalize();
        }

        if (horizontal > 0 && transform.localScale.x < 0 ||
            horizontal < 0 && transform.localScale.x > 0)
        {
            Flip();
        }

        anim.SetFloat("horizontal", Mathf.Abs(horizontal));
        anim.SetFloat("vertical", Mathf.Abs(vertical));

        // 3. Apply the normalized direction to the velocity
        rb.velocity = moveDirection * speed;
    }

    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }
}