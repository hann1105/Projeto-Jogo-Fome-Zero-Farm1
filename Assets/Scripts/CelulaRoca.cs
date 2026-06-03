using UnityEngine;

// ═══════════════════════════════════════════════════════
// CELURAROCA — Uma célula individual do grid de plantio
// Onde colocar: prefab "CelulaRoca" (cada quadradinho da fazenda)
// ═══════════════════════════════════════════════════════
public class CelulaRoca : MonoBehaviour
{
    [Header("=== ESTADO DA CÉLULA ===")]
    public bool temPlanta = false;
    public bool estaIrrigada = false;
    public bool prontaParaColher = false;
    public Cultura culturaAtual = null;

    [Header("=== PROGRESSO ===")]
    public int diasPlantada = 0;        // quantos dias desde que foi plantada
    [Range(0f, 1f)]
    public float progressoCrescimento = 0f; // 0=semente, 1=pronta

    [Header("=== FERTILIDADE DO SOLO ===")]
    [Range(0f, 1f)]
    public float fertilidade = 1f;     // cai com uso repetido

    [Header("=== REFERÊNCIAS ===")]
    public SpriteRenderer spriteRenderer;   // mostra a planta
    public SpriteRenderer spriteSolo;       // mostra o estado do solo
    public Sprite soloVazio;
    public Sprite soloIrrigado;
    public Sprite soloSeco;

    private int coordX, coordY;

    public void Inicializar(int x, int y)
    {
        coordX = x;
        coordY = y;
        AtualizarVisual();
    }

    // ── PLANTAR ──────────────────────────────────────────
    // Chamado pelo Jogador ao clicar na célula com cultura selecionada
    public bool Plantar(Cultura cultura)
    {
        if (temPlanta)
        {
            Debug.Log("Já tem planta aqui!");
            return false;
        }

        // Verifica energia
        if (!GameClock.Instance.GastarEnergia(cultura.custoEnergia))
            return false;

        culturaAtual = cultura;
        temPlanta = true;
        diasPlantada = 0;
        progressoCrescimento = 0f;
        prontaParaColher = false;

        // Desgasta o solo
        fertilidade = Mathf.Max(0.1f, fertilidade - cultura.desgasteSolo);

        AtualizarVisual();
        return true;
    }

    // ── REGAR ────────────────────────────────────────────
    public void Irrigar(bool gratis = false)
    {
        if (!temPlanta) return;

        if (!gratis && !GameClock.Instance.GastarEnergia(1))
            return;

        estaIrrigada = true;
        AtualizarVisual();
    }

    // ── COLHER ───────────────────────────────────────────
    public bool Colher()
    {
        if (!prontaParaColher) return false;
        if (!GameClock.Instance.GastarEnergia(1)) return false;

        // Gera os itens no inventário
        int quantidade = Mathf.RoundToInt(
            culturaAtual.quantidadeColhida * fertilidade
        );
        GameManager.Instance.AdicionarItem(culturaAtual.tipoItem, quantidade);

        Debug.Log($"Colheu {quantidade}x {culturaAtual.nomeCultura}!");

        // Limpa a célula
        culturaAtual = null;
        temPlanta = false;
        estaIrrigada = false;
        prontaParaColher = false;
        diasPlantada = 0;
        progressoCrescimento = 0f;

        AtualizarVisual();
        return true;
    }

    // ── AVANÇO DE DIA ────────────────────────────────────
    public void AvancarDia()
    {
        if (!temPlanta) return;

        // Só cresce se estiver irrigada (ou chuva automática)
        if (estaIrrigada)
        {
            diasPlantada++;
            progressoCrescimento = (float)diasPlantada / culturaAtual.diasParaCrescer;
            progressoCrescimento = Mathf.Clamp01(progressoCrescimento);

            if (progressoCrescimento >= 1f)
                prontaParaColher = true;
        }

        // Reset da irrigação a cada dia (precisa regar de novo)
        estaIrrigada = false;
        AtualizarVisual();
    }

    // ── EVENTOS CLIMÁTICOS ───────────────────────────────
    public void AplicarSeca()
    {
        if (!temPlanta) return;
        // Perde progresso se não estava irrigada
        if (!estaIrrigada)
        {
            progressoCrescimento = Mathf.Max(0f, progressoCrescimento - 0.2f);
            diasPlantada = Mathf.Max(0, diasPlantada - 1);
        }
        AtualizarVisual();
    }

    public void DestruirPlanta()
    {
        culturaAtual = null;
        temPlanta = false;
        estaIrrigada = false;
        prontaParaColher = false;
        progressoCrescimento = 0f;
        AtualizarVisual();
    }

    // ── VISUAL ───────────────────────────────────────────
    void AtualizarVisual()
    {
        // Solo
        if (spriteSolo != null)
        {
            if (estaIrrigada)
                spriteSolo.sprite = soloIrrigado;
            else if (temPlanta)
                spriteSolo.sprite = soloSeco;
            else
                spriteSolo.sprite = soloVazio;
        }

        // Planta
        if (spriteRenderer != null)
        {
            if (temPlanta && culturaAtual != null)
            {
                spriteRenderer.sprite =
                    culturaAtual.ObterSpritePorProgresso(progressoCrescimento);
                spriteRenderer.enabled = true;
            }
            else
            {
                spriteRenderer.enabled = false;
            }
        }
    }
}
