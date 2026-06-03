using UnityEngine;

// ═══════════════════════════════════════════════════════
// JOGADOR — Movimento top-down e interação com o mundo
// Onde colocar: GameObject "Jogador" com Rigidbody2D
// ═══════════════════════════════════════════════════════
public class Jogador : MonoBehaviour
{
    [Header("=== MOVIMENTO ===")]
    public float velocidade = 3f;

    [Header("=== INTERAÇÃO ===")]
    public float raioInteracao = 0.8f;  // distância pra interagir
    public LayerMask layerCelulas;      // layer "Plantacao"
    public LayerMask layerNPCs;         // layer "NPC"

    [Header("=== CULTURA SELECIONADA ===")]
    // Qual planta o jogador vai plantar ao clicar
    public Cultura culturaSelecionada = null;

    public enum ModoAcao { Plantar, Regar, Colher, Nenhum }
    
    [Header("=== MODO DE AÇÃO ===")]
    public ModoAcao modoAtual = ModoAcao.Nenhum;
    
    // Componentes
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 movimento;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        LerInput();
        VerificarInteracao();
    }

    void FixedUpdate()
    {
        Mover();
    }

    // ── INPUT ────────────────────────────────────────────
    void LerInput()
    {
        // WASD ou setas do teclado
        movimento.x = Input.GetAxisRaw("Horizontal");
        movimento.y = Input.GetAxisRaw("Vertical");
        movimento.Normalize(); // evita diagonal mais rápida

        // Flip do sprite: olha pra esquerda ou direita
        if (movimento.x < 0) spriteRenderer.flipX = true;
        if (movimento.x > 0) spriteRenderer.flipX = false;

        // Animações
        if (animator != null)
        {
            animator.SetFloat("velX", Mathf.Abs(movimento.x));
            animator.SetFloat("velY", movimento.y);
            animator.SetBool("andando", movimento.magnitude > 0);
        }

        // Clique esquerdo = agir na célula mais próxima
        if (Input.GetMouseButtonDown(0))
            AgirNaCelulaProxima();
    }

    void Mover()
    {
        rb.MovePosition(rb.position + movimento * velocidade * Time.fixedDeltaTime);
    }

    // ── INTERAÇÃO ────────────────────────────────────────
    void AgirNaCelulaProxima()
    {
        // Busca células no raio de interação
        Collider2D hit = Physics2D.OverlapCircle(
            transform.position, raioInteracao, layerCelulas);

        if (hit == null) return;

        CelulaRoca celula = hit.GetComponent<CelulaRoca>();
        if (celula == null) return;

        switch (modoAtual)
        {
            case ModoAcao.Plantar:
                if (culturaSelecionada != null)
                    celula.Plantar(culturaSelecionada);
                break;

            case ModoAcao.Regar:
                celula.Irrigar();
                break;

            case ModoAcao.Colher:
                celula.Colher();
                break;
        }
    }

    void VerificarInteracao()
    {
        // Verifica NPCs próximos (porta da casa)
        Collider2D npc = Physics2D.OverlapCircle(
            transform.position, raioInteracao, layerNPCs);

        if (npc != null && Input.GetKeyDown(KeyCode.E))
        {
            NPC script = npc.GetComponent<NPC>();
            if (script != null) script.Interagir();
        }
    }

    // ── MÉTODOS CHAMADOS PELOS BOTÕES DA UI ──────────────
    public void SelecionarModoplantar(int indexCultura)
    {
        modoAtual = ModoAcao.Plantar;
        if (indexCultura < GradeDeRoca.Instance.culturasDisponiveis.Count)
            culturaSelecionada = GradeDeRoca.Instance.culturasDisponiveis[indexCultura];
    }

    public void SelecionarModoRegar()  { modoAtual = ModoAcao.Regar;  }
    public void SelecionarModoColher() { modoAtual = ModoAcao.Colher; }
    public void CancelarModo()         { modoAtual = ModoAcao.Nenhum; }

    // Gizmo visual do raio de interação no editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, raioInteracao);
    }
}
