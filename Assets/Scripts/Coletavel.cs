using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coletavel : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D colisao)
    {

        Jogador jogador = colisao.GetComponent<Jogador>();
        if (jogador)
        {

            jogador.numMacas++;
            Destroy(this.gameObject);        

        }
    }
    
}
