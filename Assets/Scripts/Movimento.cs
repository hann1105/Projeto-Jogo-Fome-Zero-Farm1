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
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

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
        rb.linearVelocity = direcao * speed;
    }

    private void OnDisable()
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
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
