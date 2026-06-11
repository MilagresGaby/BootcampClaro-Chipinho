using UnityEngine;
using System.Collections;

public class IdleVariationController : MonoBehaviour
{
    private Animator animator;

    public float minTime = 8f;
    public float maxTime = 15f;

    void Start()
    {
        animator = GetComponent<Animator>();

        StartCoroutine(PlayVariations());
    }

    IEnumerator PlayVariations()
    {
        while (true)
        {
            float waitTime = Random.Range(minTime, maxTime);

            yield return new WaitForSeconds(waitTime);

            int randomAnim = Random.Range(0, 2);

            if (randomAnim == 0)
                animator.SetTrigger("IdleVariation");
            else
                animator.SetTrigger("IdleLook");
        }
    }
}