using UnityEngine;
using System.Collections.Generic;

// ═══════════════════════════════════════════════════════
// GERENCIADORNPCS — Spawna NPCs a cada dia
// Onde colocar: GameObject "GerenciadorNPCs" na cena da casa
// ═══════════════════════════════════════════════════════
public class GerenciadorNPCs : MonoBehaviour
{
    public static GerenciadorNPCs Instance;

    [Header("=== CONFIGURAÇÃO ===")]
    public List<GameObject> prefabsNPCs;    // arraste os prefabs de NPC aqui
    public List<Transform> pontosSpawn;     // posições onde NPCs aparecem
    public int npcsPorDia = 2;              // aumenta com a progressão

    private List<NPC> npcsAtivos = new List<NPC>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        GameManager.Instance.OnDiaMudou += AoMudarDia;
        SpawnarNPCsDodia();
    }

    void AoMudarDia()
    {
        // Penaliza NPCs não atendidos do dia anterior
        foreach (var npc in npcsAtivos)
            if (npc != null) npc.AoPassarDiaSemAtender();

        // Limpa NPCs antigos
        foreach (var npc in npcsAtivos)
            if (npc != null && npc.gameObject != null)
                Destroy(npc.gameObject);
        npcsAtivos.Clear();

        // Progressão: mais NPCs por dia conforme avança
        npcsPorDia = 1 + (GameManager.Instance.diaAtual / 5);
        npcsPorDia = Mathf.Clamp(npcsPorDia, 1, pontosSpawn.Count);

        SpawnarNPCsDodia();
    }

    void SpawnarNPCsDodia()
    {
        if (prefabsNPCs.Count == 0 || pontosSpawn.Count == 0) return;

        // Sorteia quais NPCs aparecem hoje
        List<int> indicesUsados = new List<int>();

        for (int i = 0; i < npcsPorDia; i++)
        {
            int indexNPC = Random.Range(0, prefabsNPCs.Count);
            int indexSpawn = i % pontosSpawn.Count;

            GameObject obj = Instantiate(
                prefabsNPCs[indexNPC],
                pontosSpawn[indexSpawn].position,
                Quaternion.identity
            );

            NPC npc = obj.GetComponent<NPC>();
            if (npc != null) npcsAtivos.Add(npc);
        }
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnDiaMudou -= AoMudarDia;
    }
}
