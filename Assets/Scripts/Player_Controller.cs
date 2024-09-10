using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.InputSystem;

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
    [SerializeField] private int points;

    [Header("Raycast Properties")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance = 5f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Jump Controllers")]
    private bool isJumping;
    private bool hasDoubleJump;
    private bool canJump;

    [Header("Manager")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private UIManager uiManager;

    public static event Action<int> OnPlayerLifeUpdate;
    public static event Action<int> OnChangedScore;
    public static event Action OnPlayerWin;
    public static event Action OnPlayerLoose;

    private void Awake()
    {
        myRBD = GetComponent<Rigidbody2D>();
        mySprite = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        uiManager.UpdateLifePlayer(playerLife);
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
                hasDoubleJump = true;
            }
            else if (hasDoubleJump)
            {
                myRBD.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                hasDoubleJump = true;
                isJumping = false;
            }
        }
    }
    public void InputReadDirection(InputAction.CallbackContext context)
    {
        direction.x = context.ReadValue<float>();
    }
    public void InputReadJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (canJump)
            {
                isJumping = true;
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

        OnPlayerLifeUpdate?.Invoke(playerLife);

        if (playerLife == 0)
        {
            Debug.Log("Moriste");
            OnPlayerLoose?.Invoke();
            if (gameObject != null)
            {
                gameObject.SetActive(false);
            }
        }
    }
    public void HealPlayer(int healAmount)
    {
        playerLife += healAmount;
        if (playerLife > 10)
        {
            playerLife = 10;
        }
        OnPlayerLifeUpdate?.Invoke(playerLife);
    }
    public void ChangeScore(int score)
    {
        points += score;
        OnChangedScore?.Invoke(points);
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
        else if (collision.gameObject.CompareTag("Corazon"))
        {
            HealPlayer(2);
            Destroy(collision.gameObject);
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
            OnPlayerWin?.Invoke();
            Debug.Log("Ganaste");
        }
        else if (collision.gameObject.CompareTag("Coin"))
        {
            ChangeScore(3);
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            isColliding = false;
        }
    }
    private void OnDisable()
    {
        OnPlayerLifeUpdate = null;
        OnChangedScore = null;
        OnPlayerWin = null;
        OnPlayerLoose = null;
    }
}