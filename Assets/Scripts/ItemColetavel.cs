using UnityEngine;

public class ItemColetavel : MonoBehaviour
{
    // Permite escolher no Unity se este item é um raio de energia ou outro coletável
    public enum TipoColetavel { SinalEnergia, Outro }
    public TipoColetavel tipoItem;

    private void Update()
    {
        // Faz o item girar no ar
        transform.Rotate(0, 50 * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        // CORRIGIDO: Removido o 'Hierarchy' que causava o erro CS1061
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
            {
                if (tipoItem == TipoColetavel.SinalEnergia)
                {
                    // Alimenta a barra de energia do painel do grupo
                    GameManager.Instance.AdicionarEnergia(15f); 
                }
                else
                {
                    GameManager.Instance.AdicionarModulo();
                }
            }

            // Destrói o item coletado
            Destroy(gameObject);
        }
    }
}