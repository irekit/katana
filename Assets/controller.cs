using UnityEngine;
using UnityEngine.InputSystem;
public class controller : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        init_pos = transform.position;
    }
    int dir = 0;
    Rigidbody2D rb;
    const float forc = 150;
    const float max_accel = 5;
    const float jumpForce = 20;
    float grt = 0;
    //initial player position
    Vector3 init_pos;
    bool grounded = false;
    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed)
        {
            dir = 1;
        }
        else if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed)
        {
            dir = -1;
        }
        else
        {
            dir = 0;
        }
        if ((Keyboard.current.rightArrowKey.isPressed && Keyboard.current.leftArrowKey.isPressed) || (Keyboard.current.aKey.isPressed && Keyboard.current.dKey.isPressed))
        {
            dir = 0;
        }
        Vector3 downed = transform.position;
        downed.y -= 1.01f;
        RaycastHit2D groun = Physics2D.Raycast(downed, Vector2.down, 0.1f);
        if (groun.collider != null)
        {
            grounded = true;
            grt = 0.13f;
        }
        else
        {
            //grt = ground timer
            grt -= Time.deltaTime;
            if (grt <= 0)
            {
                grounded = false;
            }
        }
        //jumping
        if (grounded && Keyboard.current.upArrowKey.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame || Keyboard.current.wKey.wasPressedThisFrame)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x * 0.9f, jumpForce);
        }
    }
    void FixedUpdate()
    {
        //movement logic
        if (Mathf.Abs(rb.linearVelocity.x) <= max_accel)
        {
            rb.AddForce(Vector2.right * dir * forc);

        }
        rb.linearVelocity = new Vector2(rb.linearVelocity.x * 0.9f, Mathf.Clamp(rb.linearVelocity.y, -15, 5000));
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Respawn"))
        {
            Respawn();
        }
    }
    void Respawn()
    {
        transform.position = init_pos;
        rb.linearVelocity = Vector2.zero;
    }
}
