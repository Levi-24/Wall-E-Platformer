using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 7;
    [SerializeField] private float jumpHeight = 150;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform sideCheck;
    [SerializeField] private LayerMask groundLayer;

    private Animator animator;
    private Rigidbody2D rigidbody2d;
    private bool isFacingRight;
    private bool isGrounded;
    private bool isTouchingSide;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        isFacingRight = true;
        isGrounded = false;
        isTouchingSide = false;
    }

    private void Update()
    {
        if (isGrounded && Input.GetAxis("Jump") > 0)
        {
            isGrounded = false;
            animator.SetBool("IsGrounded", false);
            rigidbody2d.AddForce(new Vector2(0, jumpHeight));
        }
    }

    private void FixedUpdate()
    {
        float move = Input.GetAxis("Horizontal");
        animator.SetFloat("Speed", Mathf.Abs(move));
        rigidbody2d.velocity = new(
            move * maxSpeed,
            rigidbody2d.velocity.y
        );

        if ((move > 0 && !isFacingRight) || (move < 0 && isFacingRight)) Flip();

        // Check for ground and side collisions separately
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, .15f, groundLayer);
        isTouchingSide = Physics2D.OverlapCircle(sideCheck.position, .15f, groundLayer);

        // Only update animator state if not touching the side
        if (!isTouchingSide)
        {
            animator.SetBool("IsGrounded", isGrounded);
            animator.SetFloat("VerticalSpeed", rigidbody2d.velocity.y);
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new(
            transform.localScale.x * -1,
            transform.localScale.y,
            transform.localScale.z);
    }
}