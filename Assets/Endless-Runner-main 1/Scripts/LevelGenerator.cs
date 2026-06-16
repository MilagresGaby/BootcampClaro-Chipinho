using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject[] listaDeTiles; 
    public GameObject StartTile;      

    // 🔥 TRAVADO EM ZERO: Para nascer exatamente embaixo do Player no Z:0
    private float nextSpawnZ = 0f; 
    private List<GameObject> activeTiles = new List<GameObject>();
    private int maxTilesOnScreen = 8; 

    private void Start()
    {
        // Cria os 3 primeiros blocos começando do zero absoluto embaixo do pé dela!
        SpawnTile(StartTile); // Bloco 1: Z = 0
        SpawnTile(StartTile); // Bloco 2: Z = 50
        SpawnTile(StartTile); // Bloco 3: Z = 100
    }

    private void Update()
{
    Movement playerMovement = FindAnyObjectByType<Movement>();
    GerenciadorColisao colisao = FindAnyObjectByType<GerenciadorColisao>();

    if (playerMovement == null || !playerMovement.enabled)
    {
        return; 
    }

    // 🛡️ TRAVA DO TRANCO: Se ela bateu e está se recuperando, 
    // paramos o gerador completamente até ela voltar a correr, evitando o teletransporte!
    if (colisao != null && colisao.EstaEmRecuperacao)
    {
        return; 
    }

    // SINCRONIZAÇÃO TOTAL: Só roda se ela estiver correndo normalmente
    float speed = playerMovement.currentSpeed;
    gameObject.transform.position += new Vector3(0, 0, speed * Time.deltaTime);

    // Mantemos a distância de segurança em 120f para dar tempo do bloco nascer
    if (transform.position.z >= nextSpawnZ - 120f)
    {
        if (listaDeTiles.Length > 0)
        {
            int indiceAleatorio = Random.Range(0, listaDeTiles.Length);
            SpawnTile(listaDeTiles[indiceAleatorio]);
        }
        else
        {
            SpawnTile(StartTile);
        }
    }
}

    private void SpawnTile(GameObject tilePrefab)
    {
        // 1. Instancia o bloco na posição atual do nextSpawnZ
        GameObject go = Instantiate(tilePrefab, new Vector3(0, 0, nextSpawnZ), Quaternion.identity);
        activeTiles.Add(go);
        
        // 2. VALOR PADRÃO DE SEGURANÇA (Caso o bloco não tenha o objeto "Rua")
        float comprimentoDesseTile = 50f; 

        // 3. PROCURA A RUA DINAMICAMENTE
        // Ele vai vasculhar os filhos do bloco procurando o objeto com o nome exato "Rua"
        Transform objetoRua = ProcurarFilhoPorNome(go.transform, "Rua");

        if (objetoRua != null)
        {
            MeshRenderer meshRenderer = objetoRua.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                // 🔥 A MÁGICA: Pega o tamanho real do modelo 3D no eixo Z!
                comprimentoDesseTile = meshRenderer.bounds.size.z;
            }
        }

        // 4. Soma o tamanho real medido para o próximo bloco nascer no lugar perfeito
        nextSpawnZ += comprimentoDesseTile;

        if (activeTiles.Count > maxTilesOnScreen)
        {
            DeleteOldestTile();
        }
    }

    private void DeleteOldestTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }

    // Função auxiliar para encontrar a "Rua" mesmo se ela estiver escondida dentro de outros grupos
    private Transform ProcurarFilhoPorNome(Transform pai, string nomeProcurado)
    {
        if (pai.name == nomeProcurado) return pai;

        for (int i = 0; i < pai.childCount; i++)
        {
            Transform resultado = ProcurarFilhoPorNome(pai.GetChild(i), nomeProcurado);
            if (resultado != null) return resultado;
        }
        return null;
    }
}