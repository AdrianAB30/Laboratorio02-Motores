using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Controller : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float speed;
    private Rigidbody2D myrbd;
    private Vector3 target;
    private bool facingRight = true; 

    private void Awake()
    {
        myrbd = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        target = pointA.position;
    }
    private void FixedUpdate()
    {
        Vector2 direction = ((Vector2)target - myrbd.position).normalized;
        myrbd.velocity = direction * speed;

        if (Vector2.Distance(myrbd.position, pointA.position) < 0.1f)
        {
            target = pointB.position;
            if (!facingRight)
            {
                FlipSprite();
            }
            facingRight = true;
        }
        else if (Vector2.Distance(myrbd.position, pointB.position) < 0.1f)
        {
            target = pointA.position;
            if (facingRight)
            {
                FlipSprite();
            }
            facingRight = false;
        }
    }
    private void FlipSprite()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1; 
        transform.localScale = theScale;
    }
}
