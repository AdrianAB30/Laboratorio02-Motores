using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Unity.UI;
using UnityEngine.SceneManagement;

public class Player_Controller : MonoBehaviour
{
    [Header("Player Data")]
    private Rigidbody2D myRBD;
    private SpriteRenderer mySprite;
    private Vector2 direction;
    public bool isColliding = false;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private int playerLife = 10;

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
            gameManager.EndLevel(false);
            this.gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            isColliding = true;
            Color enemyColor = collision.gameObject.GetComponent<SpriteRenderer>().color;

            if (mySprite.color == Color.red && enemyColor == Color.red ||
                mySprite.color == Color.blue && enemyColor == Color.blue ||
                mySprite.color == Color.yellow && enemyColor == Color.yellow)
            {
                Debug.Log("No recibes daño, son el mismo color");
            }
            else
            {
                TakeDamage(1);
            }
        }
        else if (collision.gameObject.CompareTag("Meta"))
        {   
            SceneManager.LoadScene("Game 2");
        }
        else if (collision.gameObject.CompareTag("Limite"))
        {
            TakeDamage(10);
            this.gameObject.SetActive(false);
            gameManager.EndLevel(false);
            Debug.Log("Moriste de Caida");
        }
        else if (collision.gameObject.CompareTag("Final"))
        {
            gameManager.EndLevel(true);
            Debug.Log("Ganaste");
        }       
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            isColliding = false;
        }
    }
}