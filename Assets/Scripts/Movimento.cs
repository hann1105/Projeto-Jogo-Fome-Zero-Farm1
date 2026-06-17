using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimento : MonoBehaviour
{
    public float speed = 2f;
    public Animator animator;

    private Rigidbody2D rb;
    private Vector2 direcao;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        direcao = new Vector2(horizontal, vertical).normalized;

        AnimateMovement(direcao);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + direcao * speed * Time.fixedDeltaTime);
    }

    private void AnimateMovement(Vector2 direcao)
    {
        if (animator == null) return;

        bool isMoving = direcao.sqrMagnitude > 0;

        animator.SetBool("isMoving", isMoving);

        if (isMoving)
        {
            animator.SetFloat("horizontal", direcao.x);
            animator.SetFloat("vertical", direcao.y);
        }
    }
}