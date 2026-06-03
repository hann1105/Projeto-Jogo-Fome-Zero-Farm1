using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

// ═══════════════════════════════════════════════════════
// NPC — Personagem que chega pedindo comida
// Onde colocar: cada prefab de NPC na cena da casa
// ═══════════════════════════════════════════════════════
public class NPC : MonoBehaviour
{
    [Header("=== IDENTIDADE ===")]
    public string nomeNPC = "Dona Maria";
    [TextArea(2, 4)]
    public string historiaPersonagem = "Mãe solo de 3 filhos, perdeu o emprego.";

    [Header("=== PEDIDO ===")]
    public List<PedidoItem> pedidos = new List<PedidoItem>();
    // Ex: 2x Arroz + 1x Feijão

    [Header("=== REPUTAÇÃO ===")]
    public float reputacaoGanhaAoAtender = 10f;
    public float reputacaoPerdidaAoIgnorar = 15f;

    [Header("=== UI DO NPC ===")]
    public GameObject painelDialogo;        // balão de fala
    public TextMeshProUGUI textoDialogo;
    public TextMeshProUGUI textoPedido;
    public Button botaoEntregar;
    public Button botaoFechar;

    [Header("=== ESTADO ===")]
    public bool foiAtendido = false;

    void Start()
    {
        if (painelDialogo != null) painelDialogo.SetActive(false);

        // Configura botões
        if (botaoEntregar != null) botaoEntregar.onClick.AddListener(TentarEntregar);
        if (botaoFechar != null)   botaoFechar.onClick.AddListener(FecharDialogo);
    }

    // ── INTERAÇÃO (chamada pelo Jogador) ─────────────────
    public void Interagir()
    {
        if (foiAtendido) return;

        if (painelDialogo != null)
        {
            painelDialogo.SetActive(true);
            textoDialogo.text = $"{nomeNPC}:\n\"{historiaPersonagem}\"";
            textoPedido.text = MontarTextoPedido();
        }
    }

    string MontarTextoPedido()
    {
        string texto = "Preciso de:\n";
        foreach (var pedido in pedidos)
            texto += $"• {pedido.quantidade}x {pedido.tipo}\n";
        return texto;
    }

    // ── TENTAR ENTREGAR ──────────────────────────────────
    public void TentarEntregar()
    {
        // Verifica se o jogador tem todos os itens
        foreach (var pedido in pedidos)
        {
            if (GameManager.Instance.ObterQuantidade(pedido.tipo) < pedido.quantidade)
            {
                textoDialogo.text = "Você não tem todos os itens ainda...";
                return;
            }
        }

        // Gasta os itens
        foreach (var pedido in pedidos)
            GameManager.Instance.GastarItem(pedido.tipo, pedido.quantidade);

        // Recompensa
        GameManager.Instance.AlterarReputacao(reputacaoGanhaAoAtender);
        GameManager.Instance.familisasAlimentadasTotal++;

        foiAtendido = true;
        textoDialogo.text = "Muito obrigada! Deus te abençoe! 🙏";

        // Fecha e some após 2 segundos
        Invoke(nameof(DesapareceNPC), 2f);
    }

    void FecharDialogo()
    {
        if (painelDialogo != null) painelDialogo.SetActive(false);
    }

    void DesapareceNPC()
    {
        FecharDialogo();
        gameObject.SetActive(false);
    }

    // Se o jogador ignorar o NPC até o fim do dia
    public void AoPassarDiaSemAtender()
    {
        if (!foiAtendido)
            GameManager.Instance.AlterarReputacao(-reputacaoPerdidaAoIgnorar);
    }
}

// Estrutura de um item pedido pelo NPC
[System.Serializable]
public class PedidoItem
{
    public TipoItem tipo;
    public int quantidade;
}
