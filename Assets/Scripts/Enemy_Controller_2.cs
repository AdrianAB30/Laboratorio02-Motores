using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Controller_2 : MonoBehaviour
{
    public float velocidad = 2f;
    public float alturaMaxima = 4f;
    public float alturaMinima = 0f;

    private bool subiendo = true;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (subiendo)
        {
            rb.velocity = new Vector2(0, velocidad);

            if (transform.position.y >= alturaMaxima)
            {
                subiendo = false;
            }
        }
        else
        {
            rb.velocity = new Vector2(0, -velocidad);

            if (transform.position.y <= alturaMinima)
            {
                subiendo = true;
            }
        }
    }
}
