using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GerenciadorColisao : MonoBehaviour
{
    [Header("Configurações de Vida")]
    public int vidasMaximas = 3; 
    private int vidasAtuais;

    [Header("Efeito de Piscar ao Dano")]
    private Renderer characterRenderer; 
    private Color originalColor;       
    private bool isInvincible = false;  

    // Propriedade para o gerador de fases saber se ela está no tranco do Hit
    public bool EstaEmRecuperacao { get; private set; } = false;

    private Animator anim;
    private Movement playerMovement;
    private CharacterController controller;

    private void Start()
    {
        vidasAtuais = vidasMaximas;
        anim = GetComponent<Animator>(); 
        playerMovement = GetComponent<Movement>();
        controller = GetComponent<CharacterController>();
        
        characterRenderer = GetComponentInChildren<Renderer>();
        if (characterRenderer != null)
        {
            originalColor = characterRenderer.material.color;
        }

        Debug.Log("❤️ Jogo Iniciado! Vidas Restantes: " + vidasAtuais);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isInvincible || vidasAtuais <= 0) return;

        // Detecta os obstáculos comuns ou as plataformas
        if (other.CompareTag("Obstaculo") || other.CompareTag("Plataforma"))
        {
            if (other.CompareTag("Plataforma"))
            {
                BoxCollider plataformaCollider = other.GetComponent<BoxCollider>();
                if (plataformaCollider != null)
                {
                    // Voltando para a sua lógica original que funcionava perfeitamente!
                    float topoDaPlataforma = other.transform.position.y + (plataformaCollider.size.y * other.transform.localScale.y / 2f);
                    float peDoJogador = transform.position.y;

                    if (peDoJogador >= topoDaPlataforma - 0.2f)
                    {
                        Debug.Log("🏃 Aya surfando com segurança no topo da plataforma!");
                        return; // Ignora o dano
                    }
                }
            }

            // Se bateu de frente (seja no obstáculo ou no filho dele), executa o dano
            ExecutarDano(other.gameObject);
        }
    }

    private void ExecutarDano(GameObject objetoColidido)
    {
        vidasAtuais--;
        Debug.LogWarning("💥 Bateu! Vidas Restantes: " + vidasAtuais);

        // REGRA DE OURO: Se o objeto que ela colidiu tiver um Pai (o bloco da plataforma), 
        // destrói o pai para sumir com tudo. Se não tiver, destrói o obstáculo comum.
        if (objetoColidido.transform.parent != null)
        {
            Destroy(objetoColidido.transform.parent.gameObject);
        }
        else
        {
            Destroy(objetoColidido);
        }

        if (vidasAtuais <= 0)
        {
            Debug.LogError("💀 Fim de Jogo!");
            StartCoroutine(RotinaMorteSuave());
        }
        else
        {
            if (anim != null) 
            {
                anim.ResetTrigger("Jump");
                anim.SetTrigger("Hit"); 
            }
            StartCoroutine(EfeitoDanoEPunicaoVelocidade());
        }
    }

    public void TomarDanoExterno(GameObject objeto)
    {
        if (isInvincible || vidasAtuais <= 0) return;
        ExecutarDano(objeto);
    }

    private IEnumerator EfeitoDanoEPunicaoVelocidade()
    {
        isInvincible = true;
        EstaEmRecuperacao = true; 

        float velocidadeOriginal = 17f;
        if (playerMovement != null)
        {
            velocidadeOriginal = playerMovement.currentSpeed;
            playerMovement.currentSpeed = 0f; 
        }

        if (characterRenderer != null) characterRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        if (characterRenderer != null) characterRenderer.material.color = originalColor;
        yield return new WaitForSeconds(0.15f);
        
        if (characterRenderer != null) characterRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        if (characterRenderer != null) characterRenderer.material.color = originalColor;

        if (anim != null && vidasAtuais > 0)
        {
            anim.CrossFade("Correndo", 0.2f); 
        }

        if (playerMovement != null && vidasAtuais > 0)
        {
            playerMovement.currentSpeed = velocidadeOriginal; 
        }
        
        EstaEmRecuperacao = false; 
        yield return new WaitForSeconds(0.4f); 
        isInvincible = false;
    }

    private IEnumerator RotinaMorteSuave()
    {
        if (anim != null) 
        {
            anim.ResetTrigger("Jump");
            anim.ResetTrigger("Hit");
            anim.Play("Dead"); // Força a animação de morte a tocar
            anim.SetTrigger("Dead");
        }

        if (playerMovement != null) playerMovement.enabled = false;

        float tempoQueda = 0f;
        Vector3 forcaQueda = Vector3.zero;

        while (controller != null && !controller.isGrounded && tempoQueda < 1.5f)
        {
            forcaQueda.y += -25f * Time.deltaTime; 
            controller.Move(forcaQueda * Time.deltaTime);
            tempoQueda += Time.deltaTime;
            yield return null;
        }

        if (controller != null) controller.enabled = false; // Liberta a física na morte
    }
}