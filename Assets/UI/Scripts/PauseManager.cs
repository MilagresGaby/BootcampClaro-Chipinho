using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("Painel de Pause")]
    public GameObject painelPause;

    [Header("Nome da cena do menu principal")]
    public string nomeCenaMenuPrincipal = "Main";

    private bool jogoPausado = false;

    private void Start()
    {
        Time.timeScale = 1f;

        if (painelPause != null)
        {
            painelPause.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            AlternarPause();
        }
    }

    public void AlternarPause()
    {
        if (jogoPausado)
        {
            ContinuarJogo();
        }
        else
        {
            PausarJogo();
        }
    }

    public void PausarJogo()
    {
        if (painelPause != null)
        {
            painelPause.SetActive(true);
        }

        Time.timeScale = 0f;
        jogoPausado = true;

        Debug.Log("Jogo pausado");
    }

    public void ContinuarJogo()
    {
        if (painelPause != null)
        {
            painelPause.SetActive(false);
        }

        Time.timeScale = 1f;
        jogoPausado = false;

        Debug.Log("Jogo continuado");
    }

    public void IrParaMenuPrincipal()
    {
        Time.timeScale = 1f;

        Debug.Log("Indo para o menu principal");

        SceneManager.LoadScene(nomeCenaMenuPrincipal);
    }

    public void SairDoJogo()
    {
        Time.timeScale = 1f;

        Debug.Log("Saindo do jogo");

        Application.Quit();
    }
}