using UnityEngine;

public class ItemColetavel : MonoBehaviour
{
    public enum TipoDoItem { SinalEnergia, ModuloConexao }
    [Header("Configuração do Item")]
    public TipoDoItem tipoItem = TipoDoItem.SinalEnergia;
    public float quantidadeEnergia = 15f;

    [Header("Efeito Visual")]
    public float velocidadeRotacao = 100f;

    [Header("Configuração de Áudio")]
    [Tooltip("Arraste o arquivo de som (.mp3 ou .wav) aqui no Inspetor")]
    [SerializeField] private AudioClip somColeta;

    private bool jaFoiColetado = false; // Trava de segurança para o som rodar uma única vez

    private void Update()
    {
        // Faz a moeda vermelha girar no próprio eixo para dar efeito de jogo
        transform.Rotate(Vector3.up * velocidadeRotacao * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Só executa se for o Player E se a moeda ainda não tiver sido pega (evita eco/duplicação)
        if (other.CompareTag("Player") && !jaFoiColetado)
        {
            jaFoiColetado = true; // Ativa a trava imediatamente

            // Se houver um som configurado, ele toca uma única vez na posição da moeda
            if (somColeta != null)
            {
                AudioSource.PlayClipAtPoint(somColeta, transform.position);
            }

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