using UnityEngine;

public class BauDeDoacao : MonoBehaviour
{
    [Header("Interacao")]
    [SerializeField] private KeyCode teclaDoacao = KeyCode.E;
    [SerializeField] private float raioInteracao = 1.5f;

    [Header("Doacao")]
    [SerializeField] private int pontosPorVegetal = 10;
    [SerializeField] private string[] vegetaisAceitos =
    {
        "Cenoura",
        "Batata",
        "Repolho"
    };

    private Jogador jogador;

    private void Awake()
    {
        jogador = FindObjectOfType<Jogador>();
    }

    private void Update()
    {
        if (jogador == null)
        {
            jogador = FindObjectOfType<Jogador>();
        }

        if (jogador == null || jogador.inventario == null)
        {
            return;
        }

        if (Vector2.Distance(transform.position, jogador.transform.position) > raioInteracao)
        {
            return;
        }

        if (Input.GetKeyDown(teclaDoacao))
        {
            DoarVegetais(jogador);
        }
    }

    private void DoarVegetais(Jogador jogadorAtual)
    {
        int totalDoado = 0;

        foreach (string vegetal in vegetaisAceitos)
        {
            totalDoado += jogadorAtual.inventario.RemoveTodosPorNome(vegetal);
        }

        if (totalDoado <= 0)
        {
            return;
        }

        if (GerenciadorJogo.instance != null && GerenciadorJogo.instance.gerenciadorPontuacao != null)
        {
            GerenciadorJogo.instance.gerenciadorPontuacao.AdicionarPontos(totalDoado * pontosPorVegetal);
        }

        if (jogadorAtual.toolbar != null)
        {
            jogadorAtual.toolbar.Refresh();
        }

        Inventario_UI inventarioUI = FindObjectOfType<Inventario_UI>();
        if (inventarioUI != null)
        {
            inventarioUI.Refresh();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, raioInteracao);
    }
}
