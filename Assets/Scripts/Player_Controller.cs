using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    [Header("Player Data")]
    private Rigidbody2D myRBD;
    private SpriteRenderer mySprite;
    private Vector2 direction;
    public bool isColliding = false;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private int playerLife;

    [Header("Raycast Properties")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance = 5f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Jump Controllers")]
    private bool isJumping;
    [SerializeField] private bool hasDoubleJump;
    [SerializeField] private bool canJump;

    [Header("Manager")]
    [SerializeField] private GameManager gameManager;

    private void Awake()
    {
        myRBD = GetComponent<Rigidbody2D>();
        mySprite = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        gameManager.UpdateLifePlayer(playerLife);

    }
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        direction = new Vector2(horizontal, 0).normalized;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
        }
    }
    private void FixedUpdate()
    {
        myRBD.velocity = new Vector2(direction.x * speed, myRBD.velocity.y);

        CheckRaycast();

        if (isJumping)
        {
            if (canJump)
            {
                myRBD.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                isJumping = false;
            }
            else if (hasDoubleJump)
            {
                myRBD.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                hasDoubleJump = false;
                isJumping = false;
            }
        }
    }
    private void CheckRaycast()
    {
        Debug.DrawRay(transform.position, Vector2.down * groundCheckDistance, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);

        if (hit.collider != null)
        {
            canJump = true;
            hasDoubleJump = true;
        }
        else
        {
            canJump = false;
        }
    }
    public void TakeDamage(int damage)
    {
        playerLife -= damage;

        if (playerLife < 0)
        {
            playerLife = 0;
        }

        gameManager.UpdateLifePlayer(playerLife);

        if (playerLife == 0)
        {
            Debug.Log("Moriste");
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Obstacle"))
        {
            TakeDamage(1);
            isColliding = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Obstacle"))
        {
            TakeDamage(1);
            isColliding = false;
        }
    }
}