using UnityEngine;
using System.Collections;

public class EfeitoRaioConexao : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public float duracaoRaio = 0.3f; // Tempo que o raio fica visível na tela

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    public void DispararRaio(Vector3 posicaoInicial, Vector3 posicaoFinal)
    {
        StartCoroutine(AnimarRaio(posicaoInicial, posicaoFinal));
    }

    private IEnumerator AnimarRaio(Vector3 inicio, Vector3 fim)
    {
        lineRenderer.enabled = true;
        
        float tempo = 0;
        while (tempo < duracaoRaio)
        {
            tempo += Time.deltaTime;
            
            // Atualiza a posição do raio caso o Player continue a mover-se
            // Se o script estiver no Player, 'inicio' pode ser transform.position
            lineRenderer.SetPosition(0, inicio); 
            lineRenderer.SetPosition(1, fim);
            
            yield return null;
        }

        lineRenderer.enabled = false;
    }
}