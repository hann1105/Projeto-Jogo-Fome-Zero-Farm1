using UnityEngine;
using System.Collections.Generic;

public class GerenciadorItem : MonoBehaviour
{
    public Coletavel[] itensColetaveis;

    private Dictionary<TipoColetavel, Coletavel> dicionarioItens = 
        new Dictionary<TipoColetavel, Coletavel>();

    private void Awake()
    {
        foreach(Coletavel item in itensColetaveis)
        {
          AdicionarItem(item);  
        }
    }

    private void AdicionarItem(Coletavel item)
    {
        // Alterado de Constainskey para ContainsKey
        if (!dicionarioItens.ContainsKey(item.tipo))
        {
            dicionarioItens.Add(item.tipo, item);
        }
    }

    public Coletavel ObterItemPorTipo(TipoColetavel tipo)
    {
        if (dicionarioItens.ContainsKey(tipo))
        {
            return dicionarioItens[tipo];
        }
        return null;
    }
}


