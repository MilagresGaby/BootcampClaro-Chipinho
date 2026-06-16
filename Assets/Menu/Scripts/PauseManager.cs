using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject painelPause;

    private bool pausado = false;

    // Chame este método pelo OnClick() do botão no Inspector
    public void AlternarPause()
    {
        pausado = !pausado;

        painelPause.SetActive(pausado);

        Time.timeScale = pausado ? 0f : 1f;
    }

    // Garante que o jogo volta ao normal ao sair da cena
    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}