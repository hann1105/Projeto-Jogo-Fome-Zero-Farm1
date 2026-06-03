using UnityEngine;
using UnityEngine.UI;
using TMPro;

// ═══════════════════════════════════════════════════════
// UIMANAGER — Atualiza toda a interface do jogo
// Onde colocar: GameObject "UIManager" na cena
// ═══════════════════════════════════════════════════════
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("=== HUD PRINCIPAL ===")]
    public TextMeshProUGUI textoDia;
    public TextMeshProUGUI textoEnergia;
    public Slider sliderReputacao;
    public TextMeshProUGUI textoReputacao;
    public TextMeshProUGUI textoFamilias;

    [Header("=== INVENTÁRIO ===")]
    public TextMeshProUGUI textoArroz;
    public TextMeshProUGUI textoFeijao;
    public TextMeshProUGUI textoMilho;
    public TextMeshProUGUI textoTomate;
    public TextMeshProUGUI textoAlface;
    public TextMeshProUGUI textoBatata;

    [Header("=== PAINÉIS ===")]
    public GameObject painelInventario;
    public GameObject painelAcoes;      // botões: Plantar, Regar, Colher
    public GameObject painelGameOver;
    public GameObject painelVitoria;
    public TextMeshProUGUI textoFimDeJogo;

    [Header("=== BOTÕES DE AÇÃO ===")]
    // Cada botão muda o modo do jogador
    public Button botaoModoRegar;
    public Button botaoModoColher;
    public Button botaoDormir;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Inscreve nos eventos do GameManager
        GameManager.Instance.OnDiaMudou       += AtualizarTudo;
        GameManager.Instance.OnReputacaoMudou  += AtualizarReputacao;
        GameManager.Instance.OnInventarioMudou += AtualizarInventario;

        // Conecta botões
        if (botaoModoRegar  != null) botaoModoRegar .onClick.AddListener(
            () => FindObjectOfType<Jogador>().SelecionarModoRegar());
        if (botaoModoColher != null) botaoModoColher.onClick.AddListener(
            () => FindObjectOfType<Jogador>().SelecionarModoColher());
        if (botaoDormir     != null) botaoDormir    .onClick.AddListener(
            () => GameClock.Instance.BotaoDormir());

        // Esconde painéis de fim
        if (painelGameOver != null) painelGameOver.SetActive(false);
        if (painelVitoria  != null) painelVitoria .SetActive(false);

        AtualizarTudo();
    }

    // ── ATUALIZAR TUDO ───────────────────────────────────
    public void AtualizarTudo()
    {
        AtualizarHUD();
        AtualizarReputacao();
        AtualizarInventario();
        VerificarFimDeJogo();
    }

    void AtualizarHUD()
    {
        if (textoDia != null)
            textoDia.text = $"Dia {GameManager.Instance.diaAtual}";

        if (textoFamilias != null)
            textoFamilias.text =
                $"🏘️ {GameManager.Instance.familisasAlimentadasTotal} famílias";
    }

    void AtualizarReputacao()
    {
        float rep = GameManager.Instance.reputacao;

        if (sliderReputacao != null)
            sliderReputacao.value = rep / 100f;

        if (textoReputacao != null)
            textoReputacao.text = $"Reputação: {rep:F0}%";
    }

    void AtualizarInventario()
    {
        if (textoArroz  != null) textoArroz .text = $"🌾 {GameManager.Instance.qtdArroz}";
        if (textoFeijao != null) textoFeijao.text = $"🫘 {GameManager.Instance.qtdFeijao}";
        if (textoMilho  != null) textoMilho .text = $"🌽 {GameManager.Instance.qtdMilho}";
        if (textoTomate != null) textoTomate.text = $"🍅 {GameManager.Instance.qtdTomate}";
        if (textoAlface != null) textoAlface.text = $"🥬 {GameManager.Instance.qtdAlface}";
        if (textoBatata != null) textoBatata.text = $"🥔 {GameManager.Instance.qtdBatata}";
    }

    void VerificarFimDeJogo()
    {
        if (!GameManager.Instance.jogoAcabou) return;

        if (GameManager.Instance.vitoria)
        {
            if (painelVitoria != null) painelVitoria.SetActive(true);
            if (textoFimDeJogo != null)
                textoFimDeJogo.text =
                    $"🌟 PARABÉNS!\n\n" +
                    $"Você alimentou {GameManager.Instance.familisasAlimentadasTotal} famílias!\n\n" +
                    $"No mundo real, 733 milhões de pessoas passam fome.\n" +
                    $"No Brasil, 19 milhões vivem em insegurança alimentar grave.\n\n" +
                    $"Pequenas ações fazem grande diferença. 🌱";
        }
        else
        {
            if (painelGameOver != null) painelGameOver.SetActive(true);
            if (textoFimDeJogo != null)
                textoFimDeJogo.text =
                    "A comunidade perdeu a confiança...\n\nTente novamente!";
        }
    }

    // ── BOTÃO REINICIAR ──────────────────────────────────
    public void BotaoReiniciar()
    {
        GameManager.Instance.ReiniciarJogo();
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnDiaMudou       -= AtualizarTudo;
            GameManager.Instance.OnReputacaoMudou  -= AtualizarReputacao;
            GameManager.Instance.OnInventarioMudou -= AtualizarInventario;
        }
    }
}
