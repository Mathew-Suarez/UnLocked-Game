using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;
    public int facingDirection = 1;

    [Header("Components")]
    public Rigidbody2D rb;
    public Animator anim;
    public Player_Combat player_Combat;   // (if you need it for other scripts)

    [Header("Mobile Input")]
    public MobileJoystick moveJoystick;

    private void Awake()
    {
        // Auto‑assign Rigidbody2D if not set
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        // Auto‑assign Animator if not set
        if (anim == null)
            anim = GetComponent<Animator>();

        // Auto‑find the joystick
        if (moveJoystick == null)
        {
            // Try the static reference first (fastest)
            moveJoystick = MobileJoystick.instance;

            // If still null, search the whole scene
            if (moveJoystick == null)
            {
                moveJoystick = FindObjectOfType<MobileJoystick>();
                if (moveJoystick == null)
                    Debug.LogError($"[{gameObject.name}] No MobileJoystick found! Movement disabled.");
                if (moveJoystick == null)
                    Debug.LogError("❌ Joystick is NULL! Movement impossible.");
                else
                    Debug.Log("✅ Joystick found: " + moveJoystick.gameObject.name);
            }
        }
    }

    void FixedUpdate()
    {
        // If no joystick, do nothing (prevents NullReferenceException)
        if (moveJoystick == null)
            return;

        float horizontal = moveJoystick.Horizontal;
        float vertical = moveJoystick.Vertical;

        Vector2 moveDirection = new Vector2(horizontal, vertical);

        if (moveDirection.magnitude > 1f)
            moveDirection.Normalize();

        if (horizontal > 0f && transform.localScale.x < 0f ||
            horizontal < 0f && transform.localScale.x > 0f)
        {
            Flip();
        }

        if (anim != null)
        {
            anim.SetFloat("horizontal", Mathf.Abs(horizontal));
            anim.SetFloat("vertical", Mathf.Abs(vertical));
        }

        if (rb != null)
            rb.velocity = moveDirection * speed;
    }

    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }
}