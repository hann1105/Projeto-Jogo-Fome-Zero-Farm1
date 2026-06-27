using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Item))]
public class Coletavel : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D colisao)
    {
        Jogador jogador = colisao.GetComponent<Jogador>();

        if (jogador)
        {
            Item item = GetComponent<Item>();

            if (item != null)
            {
                jogador.inventario.Add(item);
                Destroy(this.gameObject);
            }
            
        }
    }
}

