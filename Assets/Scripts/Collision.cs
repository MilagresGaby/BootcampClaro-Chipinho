using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    private SkinnedMeshRenderer characterRenderer;
    private Color originalColor;
    
    [Header("Configurações de Vida")]
    public int vidasMaximas = 3; 
    private int vidasAtuais;
    private bool isInvincible = false; 

    private void Start()
    {
        vidasAtuais = vidasMaximas;
        characterRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        
        if (characterRenderer != null)
        {
            originalColor = characterRenderer.material.color;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle") && !isInvincible)
        {
            vidasAtuais--; 
            Debug.Log("O Player tomou dano! Vidas restantes: " + vidasAtuais);

            Animator anim = GetComponentInChildren<Animator>();

            if (vidasAtuais > 0)
            {
                if (anim != null)
                {
                    anim.SetTrigger("Hit"); 
                }
                StartCoroutine(EfeitoDanoPiscar(anim));
            }
            else
            {
                Debug.Log("Game Over! O Player morreu.");
                isInvincible = true; 

                if (anim != null)
                {
                    anim.ResetTrigger("Hit");
                    anim.SetTrigger("Dead"); 
                }

                Movement mov = GetComponent<Movement>();
                if (mov != null) mov.enabled = false;

                LevelGenerator gen = FindAnyObjectByType<LevelGenerator>();
                if (gen != null) gen.enabled = false;
            }
        }
    }

    private IEnumerator EfeitoDanoPiscar(Animator anim)
    {
        isInvincible = true;

        if (characterRenderer != null) characterRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        if (characterRenderer != null) characterRenderer.material.color = originalColor;
        yield return new WaitForSeconds(0.15f);
        if (characterRenderer != null) characterRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        if (characterRenderer != null) characterRenderer.material.color = originalColor;

        yield return new WaitForSeconds(0.35f); 

        if (anim != null && vidasAtuais > 0)
        {
            anim.Play("Correndo", 0, 0f); 
        }

        yield return new WaitForSeconds(0.4f);
        isInvincible = false;
    }
}