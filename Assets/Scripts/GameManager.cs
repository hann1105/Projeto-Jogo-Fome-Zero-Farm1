using UnityEngine;
using UnityEngine.SceneManagement;

// ═══════════════════════════════════════════════════════
// GAMEMANAGER — Cérebro central do jogo
// Onde colocar: GameObject vazio chamado "GameManager"
// Esse objeto NUNCA é destruído ao trocar de cena
// ═══════════════════════════════════════════════════════
public class GameManager : MonoBehaviour
{
    // Singleton: qualquer script acessa via GameManager.Instance
    public static GameManager Instance;

    [Header("=== PROGRESSO DO JOGADOR ===")]
    public int diaAtual = 1;
    public int diaMaximo = 21;         // jogo dura 21 dias
    public float reputacao = 50f;      // começa em 50%, vai de 0 a 100
    public int familisasAlimentadasTotal = 0;

    [Header("=== INVENTÁRIO ===")]
    public int qtdArroz = 0;
    public int qtdFeijao = 0;
    public int qtdMilho = 0;
    public int qtdTomate = 0;
    public int qtdAlface = 0;
    public int qtdBatata = 0;

    [Header("=== ESTADO DO JOGO ===")]
    public bool jogoAcabou = false;
    public bool vitoria = false;

    // ── Eventos que outros scripts podem ouvir ──
    // Ex: UIManager.cs se inscreve e atualiza a tela automaticamente
    public System.Action OnDiaMudou;
    public System.Action OnReputacaoMudou;
    public System.Action OnInventarioMudou;

    void Awake()
    {
        // Garante que só existe UM GameManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // sobrevive à troca de cena
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ── REPUTAÇÃO ───────────────────────────────────────
    public void AlterarReputacao(float quantidade)
    {
        reputacao = Mathf.Clamp(reputacao + quantidade, 0f, 100f);
        OnReputacaoMudou?.Invoke(); // avisa a UI

        if (reputacao <= 0f) TerminarJogo(false);
    }

    // ── INVENTÁRIO ──────────────────────────────────────
    public void AdicionarItem(TipoItem tipo, int quantidade)
    {
        switch (tipo)
        {
            case TipoItem.Arroz:   qtdArroz   += quantidade; break;
            case TipoItem.Feijao:  qtdFeijao  += quantidade; break;
            case TipoItem.Milho:   qtdMilho   += quantidade; break;
            case TipoItem.Tomate:  qtdTomate  += quantidade; break;
            case TipoItem.Alface:  qtdAlface  += quantidade; break;
            case TipoItem.Batata:  qtdBatata  += quantidade; break;
        }
        OnInventarioMudou?.Invoke();
    }

    public bool GastarItem(TipoItem tipo, int quantidade)
    {
        // Retorna false se não tiver o suficiente
        switch (tipo)
        {
            case TipoItem.Arroz:
                if (qtdArroz < quantidade) return false;
                qtdArroz -= quantidade; break;
            case TipoItem.Feijao:
                if (qtdFeijao < quantidade) return false;
                qtdFeijao -= quantidade; break;
            case TipoItem.Milho:
                if (qtdMilho < quantidade) return false;
                qtdMilho -= quantidade; break;
            case TipoItem.Tomate:
                if (qtdTomate < quantidade) return false;
                qtdTomate -= quantidade; break;
            case TipoItem.Alface:
                if (qtdAlface < quantidade) return false;
                qtdAlface -= quantidade; break;
            case TipoItem.Batata:
                if (qtdBatata < quantidade) return false;
                qtdBatata -= quantidade; break;
        }
        OnInventarioMudou?.Invoke();
        return true;
    }

    public int ObterQuantidade(TipoItem tipo)
    {
        switch (tipo)
        {
            case TipoItem.Arroz:  return qtdArroz;
            case TipoItem.Feijao: return qtdFeijao;
            case TipoItem.Milho:  return qtdMilho;
            case TipoItem.Tomate: return qtdTomate;
            case TipoItem.Alface: return qtdAlface;
            case TipoItem.Batata: return qtdBatata;
            default: return 0;
        }
    }

    // ── AVANÇO DE DIA ────────────────────────────────────
    public void AvancarDia()
    {
        if (jogoAcabou) return;

        diaAtual++;
        OnDiaMudou?.Invoke();

        if (diaAtual > diaMaximo)
            TerminarJogo(reputacao >= 80f);
    }

    // ── FIM DE JOGO ──────────────────────────────────────
    void TerminarJogo(bool ganhou)
    {
        jogoAcabou = true;
        vitoria = ganhou;
        // Carrega a cena de fim de jogo após 1 segundo
        Invoke(nameof(CarregarCenaFim), 1f);
    }

    void CarregarCenaFim()
    {
        SceneManager.LoadScene("CenaFim");
    }

    public void ReiniciarJogo()
    {
        // Reseta tudo e volta ao início
        diaAtual = 1;
        reputacao = 50f;
        familisasAlimentadasTotal = 0;
        qtdArroz = qtdFeijao = qtdMilho = 0;
        qtdTomate = qtdAlface = qtdBatata = 0;
        jogoAcabou = false;
        vitoria = false;
        SceneManager.LoadScene("CenaFazenda");
    }
}

// Enum global — todos os scripts usam esse mesmo tipo
public enum TipoItem { Arroz, Feijao, Milho, Tomate, Alface, Batata }
