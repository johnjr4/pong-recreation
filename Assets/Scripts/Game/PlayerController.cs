using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rb;
    public float speed;
    public float moveLimit;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        moveLimit = 3.4f;
    }

    public void Move(InputAction.CallbackContext context)
    {
        float direction = context.ReadValue<float>();
        if (context.performed)
        {
            rb.velocity = new Vector2(0, direction * speed);
        }
        if (context.canceled)
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        if (rb.position.y > moveLimit)
        {
            rb.position = new Vector2(rb.position.x, moveLimit);
        } else if (rb.position.y < -moveLimit)
        {
            rb.position = new Vector2(rb.position.x, -moveLimit);
        }
    }

    /* Outdated method of restricting paddle movment
     * 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.CompareTag("MainCamera"))
        {
            rb.velocity = Vector2.zero;
            rb.position += new Vector2(0f, rb.position.y < 0 ? .2f : -.2f);
        }
    }
    */
}
