using UnityEngine;

// ═══════════════════════════════════════════════════════
// CULTURA — ScriptableObject de cada tipo de planta
// Onde criar: clique direito na pasta Project →
//             Create → FomeZero → Cultura
// Crie um arquivo por planta: Arroz, Feijao, Milho...
// ═══════════════════════════════════════════════════════
[CreateAssetMenu(fileName = "NovaCultura", menuName = "FomeZero/Cultura")]
public class Cultura : ScriptableObject
{
    [Header("=== IDENTIFICAÇÃO ===")]
    public string nomeCultura = "Arroz";
    public TipoItem tipoItem;           // qual item gera ao colher

    [Header("=== TEMPO DE CRESCIMENTO ===")]
    public int diasParaCrescer = 3;     // dias até ficar pronta pra colher
    // Fases visuais: semente → broto → adulta → pronta
    // (4 sprites, um por fase)

    [Header("=== SPRITES POR FASE ===")]
    // Arraste os sprites do asset aqui no Inspector
    public Sprite spriteSemente;        // fase 0 — recém plantada
    public Sprite spriteBroto;          // fase 1 — crescendo
    public Sprite spriteAdulta;         // fase 2 — quase pronta
    public Sprite spritePronta;         // fase 3 — pronta pra colher

    [Header("=== COLHEITA ===")]
    public int quantidadeColhida = 2;   // itens gerados ao colher
    public int custoEnergia = 1;        // energia gasta pra plantar

    [Header("=== SUSTENTABILIDADE ===")]
    // Quanto esgota o solo (0=nada, 1=muito)
    [Range(0f, 1f)] public float desgasteSolo = 0.3f;
    // Precisa de quanta água por dia (0=pouco, 1=muito)
    [Range(0f, 1f)] public float necessidadeAgua = 0.5f;

    // Retorna o sprite correto baseado no progresso (0 a 1)
    public Sprite ObterSpritePorProgresso(float progresso)
    {
        if (progresso < 0.25f) return spriteSemente;
        if (progresso < 0.5f)  return spriteBroto;
        if (progresso < 0.9f)  return spriteAdulta;
        return spritePronta;
    }
}
