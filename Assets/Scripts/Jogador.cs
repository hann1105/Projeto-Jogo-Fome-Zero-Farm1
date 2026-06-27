using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jogador : MonoBehaviour
{
    public Inventario inventario;

    private void Awake()
    {
        inventario = GetComponent<Inventario>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3Int position = new Vector3Int((int)transform.position.x, (int)transform.position.y, 0);
            if (GerenciadorJogo.instance.gerenciadorDeBlocos.isInteractable(position))
            {
                Debug.Log("O jogador está em um bloco interativo!");
            } 

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