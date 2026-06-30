using UnityEngine;

public class Jogador : MonoBehaviour
{
    public Inventario inventario;
    public Toolbar_UI toolbar;

    private void Awake()
    {
        inventario = GetComponent<Inventario>();
        toolbar = FindObjectOfType<Toolbar_UI>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            InteragirComPlantacao();
        }
    }

    private void InteragirComPlantacao()
    {
        if (GerenciadorJogo.instance == null ||
            GerenciadorJogo.instance.gerenciadorDeBlocos == null ||
            GerenciadorJogo.instance.gerenciadorDePlantacao == null)
        {
            return;
        }

        Vector3Int position = GerenciadorJogo.instance.gerenciadorDeBlocos.WorldToCell(transform.position);

        if (!GerenciadorJogo.instance.gerenciadorDeBlocos.isInteractable(position))
        {
            return;
        }

        if (GerenciadorJogo.instance.gerenciadorDePlantacao.TryHarvest(position, inventario))
        {
            AdicionarPontos(5);
            AtualizarToolbar();
            return;
        }

        TentarPlantar(position);
    }

    private void TentarPlantar(Vector3Int position)
    {
        int selectedIndex = toolbar != null ? toolbar.SelectedIndex : 0;
        Inventario.Slot selectedSlot = inventario.GetSlot(selectedIndex);

        if (selectedSlot == null || selectedSlot.itemData == null || selectedSlot.itemData.cropData == null)
        {
            return;
        }

        bool planted = GerenciadorJogo.instance.gerenciadorDePlantacao.TryPlant(
            position,
            selectedSlot.itemData.cropData,
            selectedSlot.icon);

        if (planted)
        {
            inventario.Remove(selectedIndex);
            AdicionarPontos(5);
            AtualizarToolbar();
        }
    }

    private void AdicionarPontos(int quantidade)
    {
        if (GerenciadorJogo.instance != null && GerenciadorJogo.instance.gerenciadorPontuacao != null)
        {
            GerenciadorJogo.instance.gerenciadorPontuacao.AdicionarPontos(quantidade);
        }
    }

    private void AtualizarToolbar()
    {
        if (toolbar != null)
        {
            toolbar.Refresh();
        }
    }

    public void DroparItem(Item item)
    {
        Vector2 localOrigem = transform.position;
        Vector2 deslocamento = Random.insideUnitCircle * 1.25f;

        Item itemDropado = Instantiate(item, localOrigem + deslocamento, Quaternion.identity);
        itemDropado.rb2d.AddForce(deslocamento * 2f, ForceMode2D.Impulse);
    }
}
