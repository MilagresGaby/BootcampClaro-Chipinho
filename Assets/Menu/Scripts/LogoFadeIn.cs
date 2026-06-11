using UnityEngine;

public class LogoFadeIn : MonoBehaviour
{
    public CanvasGroup logo;
    public float atraso = 8f;
    void Start()
    {
        logo.alpha = 0;
        transform.localScale = Vector3.one * 0.8f;
    }

    void Update()
    {
        if (Time.time < atraso)
            return;

        if (logo.alpha < 1)
        {
            logo.alpha += Time.deltaTime;

            transform.localScale = Vector3.Lerp(
                transform.localScale,
                Vector3.one,
                Time.deltaTime * 3f
            );
        }
    }
}