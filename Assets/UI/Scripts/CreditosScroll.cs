using UnityEngine;

public class CreditosScroll : MonoBehaviour
{
    [Header("Objeto que vai subir")]
    public RectTransform creditosContainer;

    [Header("Posições")]
    public float posicaoInicialY = -900f;
    public float posicaoFinalY = 0f;

    [Header("Velocidade")]
    public float velocidade = 80f;

    private bool terminou = false;

    private void OnEnable()
    {
        ReiniciarCreditos();
    }

    private void Start()
    {
        ReiniciarCreditos();
    }

    private void Update()
    {
        if (creditosContainer == null || terminou)
        {
            return;
        }

        Vector2 posicaoAtual = creditosContainer.anchoredPosition;

        posicaoAtual.y += velocidade * Time.deltaTime;

        if (posicaoAtual.y >= posicaoFinalY)
        {
            posicaoAtual.y = posicaoFinalY;
            terminou = true;
        }

        creditosContainer.anchoredPosition = posicaoAtual;
    }

    public void ReiniciarCreditos()
    {
        if (creditosContainer == null)
        {
            return;
        }

        creditosContainer.anchoredPosition = new Vector2(
            creditosContainer.anchoredPosition.x,
            posicaoInicialY
        );

        terminou = false;
    }
}