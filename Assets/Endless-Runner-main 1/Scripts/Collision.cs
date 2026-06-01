using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    [Header("Configurações de Vida")]
    public int vidasMaximas = 3; 
    private int vidasAtuais;

    [Header("Efeito de Piscar ao Dano")]
    private Renderer characterRenderer; 
    private Color originalColor;       
    private bool isInvincible = false;  

    private Animator anim;
    private Movement playerMovement;

    private void Start()
    {
        vidasAtuais = vidasMaximas;
        // Pega o Animator no próprio pai
        anim = GetComponent<Animator>(); 
        playerMovement = GetComponent<Movement>();
        
        // Pega o Renderer no filho (Visual_Container) para mudar a cor para vermelho no dano
        characterRenderer = GetComponentInChildren<Renderer>();
        if (characterRenderer != null)
        {
            originalColor = characterRenderer.material.color;
        }

        Debug.Log("❤️ Jogo Iniciado! Vidas Restantes: " + vidasAtuais);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isInvincible) return;

        // Certifique-se de que seus obstáculos tenham a Tag "Obstaculo"
        if (other.CompareTag("Obstaculo"))
        {
            vidasAtuais--;
            Debug.LogWarning("💥 Bateu em um obstáculo! Vidas Restantes: " + vidasAtuais);

            // CORRIGIDO: MUDADO PARA "Hit"
            if (anim != null) anim.SetTrigger("Hit"); 

            if (vidasAtuais <= 0)
            {
                Debug.LogError("💀 Fim de Jogo! O Player perdeu todas as vidas.");
                
                // CORRIGIDO: MUDADO PARA "Dead"
                if (anim != null) anim.SetTrigger("Dead");
                
                // Trava o movimento do jogador ao morrer
                if (playerMovement != null) playerMovement.enabled = false; 
            }
            else
            {
                // Inicia o efeito de piscar vermelho se ele ainda tiver vidas
                StartCoroutine(EfeitoDanoPiscar(anim));
            }
        }
    }

    private IEnumerator EfeitoDanoPiscar(Animator anim)
    {
        isInvincible = true;

        // Pisca 1
        if (characterRenderer != null) characterRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        if (characterRenderer != null) characterRenderer.material.color = originalColor;
        yield return new WaitForSeconds(0.15f);
        
        // Pisca 2
        if (characterRenderer != null) characterRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        if (characterRenderer != null) characterRenderer.material.color = originalColor;

        yield return new WaitForSeconds(0.35f); 

        // Força o Animator a voltar para o estado Correndo depois do susto do dano
        if (anim != null && vidasAtuais > 0)
        {
            anim.Play("Correndo", 0, 0f); 
        }

        yield return new WaitForSeconds(0.4f);
        isInvincible = false;
    }
}