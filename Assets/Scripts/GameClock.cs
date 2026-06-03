using UnityEngine;
using UnityEngine.UI;
using TMPro;

// ═══════════════════════════════════════════════════════
// GAMECLOCK — Controla o ciclo de dia, energia e eventos
// Onde colocar: mesmo GameObject do GameManager, ou um
//               vazio chamado "GameClock" na cena
// ═══════════════════════════════════════════════════════
public class GameClock : MonoBehaviour
{
    public static GameClock Instance;

    [Header("=== ENERGIA DO JOGADOR ===")]
    public int energiaMaxima = 10;      // ações por dia
    public int energiaAtual = 10;
    // Cada ação gasta 1 de energia:
    // plantar=1, regar=1, colher=1, preparar refeição=1

   
    // O dia tem 3 fases visuais
    public enum FaseDia { Manha, Tarde, Noite }
    
    [Header("=== FASE DO DIA ===")]
    public FaseDia faseAtual = FaseDia.Manha;

    [Header("=== UI ===")]
    public TextMeshProUGUI textoDia;
    public TextMeshProUGUI textoEnergia;
    public Slider sliderEnergia;
    public GameObject painelEventoNoite;    // painel que aparece à noite
    public TextMeshProUGUI textoEvento;

    [Header("=== EVENTOS ALEATÓRIOS ===")]
    // Chance de cada evento ruim por dia (0 a 1)
    [Range(0f, 1f)] public float chanceSeca = 0.15f;
    [Range(0f, 1f)] public float chancePraga = 0.10f;
    [Range(0f, 1f)] public float chanceChuvaForte = 0.10f;

    // Evento ativo hoje (nulo = dia normal)
    [HideInInspector] public string eventoHoje = "";

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        AtualizarUI();
        // Ouve o GameManager pra saber quando o dia muda
        GameManager.Instance.OnDiaMudou += AoMudarDia;
    }

    // ── GASTAR ENERGIA ───────────────────────────────────
    // Retorna false se não tiver energia suficiente
    public bool GastarEnergia(int quantidade = 1)
    {
        if (energiaAtual < quantidade)
        {
            Debug.Log("Sem energia! Descanse.");
            return false;
        }
        energiaAtual -= quantidade;
        AtualizarUI();

        // Se acabou a energia, força passagem de dia
        if (energiaAtual <= 0)
            PassarNoite();

        return true;
    }

    // ── BOTÃO "DORMIR" ───────────────────────────────────
    // Conecte esse método ao botão de dormir na UI
    public void BotaoDormir()
    {
        PassarNoite();
    }

    void PassarNoite()
    {
        faseAtual = FaseDia.Noite;

        // Sorteia evento aleatório
        SortearEvento();

        // Mostra painel de noite com resumo
        if (painelEventoNoite != null)
        {
            painelEventoNoite.SetActive(true);
            textoEvento.text = MontarTextoResumo();
        }
        // O botão "Próximo Dia" no painel chama ConfirmarNoite()
    }

    // Chamado pelo botão "Próximo Dia" no painel de noite
    public void ConfirmarNoite()
    {
        if (painelEventoNoite != null)
            painelEventoNoite.SetActive(false);

        // Restaura energia
        energiaAtual = energiaMaxima;
        faseAtual = FaseDia.Manha;

        // Aplica efeito do evento nas plantas
        AplicarEventoNasFazendas();

        // Avança o dia no GameManager
        GameManager.Instance.AvancarDia();
        AtualizarUI();
    }

    // ── EVENTOS ──────────────────────────────────────────
    void SortearEvento()
    {
        float sorteio = Random.value;

        if (sorteio < chanceSeca)
            eventoHoje = "seca";
        else if (sorteio < chanceSeca + chancePraga)
            eventoHoje = "praga";
        else if (sorteio < chanceSeca + chancePraga + chanceChuvaForte)
            eventoHoje = "chuva";
        else
            eventoHoje = "";
    }

    void AplicarEventoNasFazendas()
    {
        GradeDeRoca grade = FindObjectOfType<GradeDeRoca>();
        if (grade == null) return;

        switch (eventoHoje)
        {
            case "seca":
                grade.AplicarSecaEmTodas();    // plantas sem água perdem progresso
                break;
            case "praga":
                grade.AplicarPragaEmTodas();   // remove 1 planta aleatória
                break;
            case "chuva":
                grade.AplicarChuvaEmTodas();   // todas as plantas ganham água grátis
                break;
        }
    }

    string MontarTextoResumo()
    {
        string resumo = $"Dia {GameManager.Instance.diaAtual} encerrado.\n";
        resumo += $"Reputação: {GameManager.Instance.reputacao:F0}%\n\n";

        if (eventoHoje == "seca")
            resumo += "⚠️ SECA: Suas plantas perderam progresso sem irrigação!";
        else if (eventoHoje == "praga")
            resumo += "⚠️ PRAGA: Uma plantação foi destruída por insetos!";
        else if (eventoHoje == "chuva")
            resumo += "🌧️ CHUVA FORTE: Todas as plantas foram irrigadas!";
        else
            resumo += "🌙 Noite tranquila. Bom descanso!";

        return resumo;
    }

    void AoMudarDia()
    {
        AtualizarUI();
    }

    void AtualizarUI()
    {
        if (textoDia != null)
            textoDia.text = $"Dia {GameManager.Instance.diaAtual} / {GameManager.Instance.diaMaximo}";

        if (textoEnergia != null)
            textoEnergia.text = $"⚡ {energiaAtual} / {energiaMaxima}";

        if (sliderEnergia != null)
            sliderEnergia.value = (float)energiaAtual / energiaMaxima;
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnDiaMudou -= AoMudarDia;
    }
}
