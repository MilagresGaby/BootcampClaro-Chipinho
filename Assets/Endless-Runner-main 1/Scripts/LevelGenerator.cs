using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject[] listaDeTiles; // Array com suas variações de obstáculos
    public GameObject StartTile;      // O tile inicial sem obstáculos

    // CORRIGIDO: Agora o gerador começa a criar o asfalto 20 metros atrás do Player (Z: 0)
    private float nextSpawnZ = -20f; 
    private float tileLength = 50f; 

    // Lista para guardar os blocos que estão ativos na cena e podermos destruí-los depois
    private List<GameObject> activeTiles = new List<GameObject>();
    private int maxTilesOnScreen = 5; // Quantidade máxima de blocos permitidos na tela por vez

    private void Start()
    {
        // CORRIGIDO: Removida a linha errada. 
        // Agora criamos os 3 primeiros blocos seguros usando o nextSpawnZ recuado
        SpawnTile(StartTile); // Bloco 1: nasce em Z: -20
        SpawnTile(StartTile); // Bloco 2: nasce em Z: 30
        SpawnTile(StartTile); // Bloco 3: nasce em Z: 80
    }

    private void Update()
    {
        // Pega a velocidade do jogador para mover o gerador junto com ele
        Movement playerMovement = FindAnyObjectByType<Movement>();
        float speed = (playerMovement != null) ? playerMovement.currentSpeed : 4f;

        gameObject.transform.position += new Vector3(0, 0, speed * Time.deltaTime);

        // Verifica se é hora de criar um bloco novo
        if (transform.position.z >= nextSpawnZ - (tileLength * 3))
        {
            // Garante que você colocou algum tile na lista antes de sortear
            if (listaDeTiles.Length > 0)
            {
                int indiceAleatorio = Random.Range(0, listaDeTiles.Length);
                SpawnTile(listaDeTiles[indiceAleatorio]);
            }
            else
            {
                // Caso a lista esteja vazia por esquecimento no Unity, gera o inicial para não quebrar o jogo
                SpawnTile(StartTile);
            }
        }
    }

    private void SpawnTile(GameObject tilePrefab)
    {
        // Instancia o bloco e guarda a referência dele na variável 'go'
        GameObject go = Instantiate(tilePrefab, new Vector3(0, 0, nextSpawnZ), Quaternion.identity);
        
        // Adiciona o bloco recém-criado na nossa lista de controle
        activeTiles.Add(go);
        
        nextSpawnZ += tileLength;

        // SISTEMA DE LIMPEZA: Se passar do limite permitido na tela, destrói o mais antigo
        if (activeTiles.Count > maxTilesOnScreen)
        {
            DeleteOldestTile();
        }
    }

    private void DeleteOldestTile()
    {
        // Remove o primeiro item da lista (o mais antigo de todos)
        Destroy(activeTiles[0]);
        // Remove a referência dele da lista para o próximo da fila assumir o posto
        activeTiles.RemoveAt(0);
    }
}