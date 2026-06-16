using UnityEngine;
using UnityMeshSimplifier; 

public class OtimizadorDeMesh : MonoBehaviour
{
    [Header("Configuração de Otimização")]
    [Range(0.01f, 1f)]
    [Tooltip("0.2 significa manter apenas 20% dos triângulos.")]
    public float nivelDeQualidade = 0.3f; 

    public void SimplificarMalha()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        
        if (meshFilter == null || meshFilter.sharedMesh == null)
        {
            Debug.LogError($"[Otimizador] O objeto '{gameObject.name}' não possui um MeshFilter ou malha válida!");
            return;
        }

        MeshSimplifier simplificador = new MeshSimplifier();
        simplificador.Initialize(meshFilter.sharedMesh);
        simplificador.SimplifyMesh(nivelDeQualidade);

        Mesh malhaOtimizada = simplificador.ToMesh();
        malhaOtimizada.name = meshFilter.sharedMesh.name + "_Otimizada";
        
        meshFilter.mesh = malhaOtimizada;

        Debug.Log($"<color=green>✓ {gameObject.name} otimizado!</color> Reduzido para {nivelDeQualidade * 100}% do peso.");
    }
}

// 🛠️ INTERFACE DO BOTÃO (CORRIGIDA PARA NÃO DAR MAIS ERRO)
#if UNITY_EDITOR
// 🔥 O segredo está aqui: CanEditMultipleObjects permite selecionar vários de uma vez!
[UnityEditor.CustomEditor(typeof(OtimizadorDeMesh)), UnityEditor.CanEditMultipleObjects]
public class OtimizadorDeMeshEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // Desenha o slider

        GUILayout.Space(15);
        
        if (GUILayout.Button("🔥 SIMPLIFICAR MALHA AGORA", GUILayout.Height(35)))
        {
            // Roda a otimização para cada um dos objetos selecionados!
            foreach (Object targetObject in targets)
            {
                OtimizadorDeMesh script = (OtimizadorDeMesh)targetObject;
                if (script != null)
                {
                    script.SimplificarMalha();
                }
            }
        }
        GUILayout.Space(5);
    }
}
#endif