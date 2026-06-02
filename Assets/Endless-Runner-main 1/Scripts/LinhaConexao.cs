using UnityEngine;

public class LinhaConexao : MonoBehaviour
{
    [Header("Casas desta mesma altura")]
    public CasaConectavel casaEsquerda;
    public CasaConectavel casaDireita;

    private bool jaAtivou = false;

    private void OnTriggerEnter(Collider other)
    {
        // Quando o jogador cruzar esta linha vertical da rua
        if (other.CompareTag("Player") && !jaAtivou)
        {
            jaAtivou = true; // Garante que só roda uma vez por pista

            // Tenta conectar a casa da esquerda se ela existir
            if (casaEsquerda != null)
            {
                casaEsquerda.ConectarPelaLinha();
            }

            // Tenta conectar a casa da direita se ela existir
            if (casaDireita != null)
            {
                casaDireita.ConectarPelaLinha();
            }
        }
    }
}