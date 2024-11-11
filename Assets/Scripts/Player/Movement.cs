using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;

    [SerializeField] private GameObject _feet;

    [SerializeField] private LayerMask _layerMask;

    private Vector3 trajectory;
    private Rigidbody2D rb;
    private SpriteRenderer render;
    private float gravity;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        render = rb.GetComponent<SpriteRenderer>();
        gravity = rb.gravityScale;
    }

    private void FixedUpdate()
    {
        trajectory.x = Input.GetAxis("Horizontal");
        trajectory *= Time.deltaTime * _speed;
        if (trajectory.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (trajectory.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        transform.position += trajectory;
    }

    private void Update()
    {
        bool ifGrounded = Physics2D.OverlapBox(_feet.transform.position, Vector2.one, 0, _layerMask);
        if (ifGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (!ifGrounded)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                rb.gravityScale = gravity * 0.9f;
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                rb.gravityScale = gravity * 2.5f;
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                rb.gravityScale = gravity * 1f;
            }
        }
        else
        {
            rb.gravityScale = gravity * 1f;
        }
    }

    void Jump()
    {
        rb.AddForce(Vector2.up * _jumpForce);
    }
}
