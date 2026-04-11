using UnityEngine;
using System.Collections.Generic;

public class DuckHuntLogic : MonoBehaviour
{
    public GameObject patoPrefab;
    public Transform respawnPointsParent;
    public int patosParaGanar = 4;
    private int patosEliminados = 0;

    void Start()
    {
        SpawnDucks();
    }

    void SpawnDucks()
    {
        // Obtenemos todos los puntos de respawn
        List<Transform> puntos = new List<Transform>();
        foreach (Transform child in respawnPointsParent) puntos.Add(child);

        // Elegimos 4 posiciones aleatorias sin repetir
        for (int i = 0; i < 4; i++)
        {
            if (puntos.Count == 0) break;
            int randomIndex = Random.Range(0, puntos.Count);
            Instantiate(patoPrefab, puntos[randomIndex].position, Quaternion.identity);
            puntos.RemoveAt(randomIndex);
        }
    }

    public void RegistrarMuerte()
    {
        patosEliminados++;
        Debug.Log("Pato eliminado: " + patosEliminados);

        if (patosEliminados >= patosParaGanar)
        {
            // Seguridad: Comprobamos si la instancia existe antes de llamarla
            if (GameManager.instancia != null) 
            {
                GameManager.instancia.Ganar();
            }
            else 
            {
                Debug.LogWarning("No se encontró GameManager en la escena. ¡Ganaste, pero no puedo avisar al Manager!");
            }
        }
    }
}