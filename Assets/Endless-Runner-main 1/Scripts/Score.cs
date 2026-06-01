using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // IMPORTANTE: Nova biblioteca para TextMeshPro

public class Score : MonoBehaviour
{
    private int ScoreInt;
    public TextMeshProUGUI ScoreText; // Alterado de Text para TextMeshProUGUI

    public void ScorePlusOne()
    {
        ScoreInt++;
    }

    private void Update()
    {
        // Verifica se o texto foi arrastado para evitar novos erros de Null
        if (ScoreText != null)
        {
            ScoreText.text = ScoreInt.ToString();
        }
    }
}