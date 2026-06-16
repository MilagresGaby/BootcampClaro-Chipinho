using UnityEngine;

public class cameraIdle : MonoBehaviour
{
    private Vector3 startPos;
    private Quaternion startRot;

    public float altura = 0.1f;
    public float velocidade = 0.5f;
    public float rotacao = 1f;

    void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;
    }

    void Update()
    {
        transform.position = startPos +
            new Vector3(
                0,
                Mathf.Sin(Time.time * velocidade) * altura,
                0
            );

        transform.rotation = startRot *
            Quaternion.Euler(
                Mathf.Sin(Time.time * 0.3f) * rotacao,
                Mathf.Sin(Time.time * 0.2f) * rotacao,
                0
            );
    }
}