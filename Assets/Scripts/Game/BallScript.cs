using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{

    private Rigidbody2D rb;
    private DropShadow dropShadow;
    private GameManager gameManager;
    public float spinMultiplier, returnMultiplier;
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip wallHit, paddleHit;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        dropShadow = GetComponent<DropShadow>();
        audioSource = GetComponent<AudioSource>();
        gameManager = GameObject.FindGameObjectsWithTag("GameController")[0].GetComponent<GameManager>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.CompareTag("Player"))
        {
            rb.velocity = new Vector2(-rb.velocity.x * returnMultiplier, rb.velocity.y + (other.GetComponent<Rigidbody2D>().velocity.y * spinMultiplier));
            audioSource.PlayOneShot(paddleHit);
        } else if (other.CompareTag("MainCamera"))
        {
            rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y);
            audioSource.PlayOneShot(wallHit);

        } else if (other.CompareTag("Goals"))
        {
            gameManager.endRound(gameObject);
        }
    }
}
