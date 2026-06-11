using UnityEngine;

public class SkyboxRotate : MonoBehaviour
{
    public float speed = 1f;

    void Update()
    {
        RenderSettings.skybox.SetFloat(
            "_Rotation",
            Time.time * speed
        );
    }
}