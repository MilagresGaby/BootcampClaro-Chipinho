using UnityEngine;

public class ItemColetavel : MonoBehaviour
{
    public enum TipoDoItem { SinalEnergia, ModuloConexao }
    [Header("Configuração do Item")]
    public TipoDoItem tipoItem = TipoDoItem.SinalEnergia;
    public float quantidadeEnergia = 15f;

    [Header("Efeito Visual")]
    public float velocidadeRotacao = 100f;

    private void Update()
    {
        // Faz a moeda vermelha girar no próprio eixo para dar efeito de jogo
        transform.Rotate(Vector3.up * velocidadeRotacao * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verifica se quem bateu na moeda foi o Player
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
            {
                if (tipoItem == TipoDoItem.SinalEnergia)
                {
                    GameManager.Instance.AdicionarEnergia(quantidadeEnergia);
                }
                else if (tipoItem == TipoDoItem.ModuloConexao)
                {
                    GameManager.Instance.AdicionarModulo();
                }
            }

            // Destrói a moeda para sumir da pista ao ser coletada
            Destroy(gameObject);
        }
    }
}