/**
 * Proyecto: Smoothie Criminal
 * Autor: Álvaro Muñoz Adán
 * Descripción: Gestiona la lógica del minijuego de trileros (vasos y bolita).
 * Última modificación: 10/04/2026
 */

using UnityEngine;
using System.Collections;

public class ShellGameLogic : MonoBehaviour
{
    #region Variables de Configuración
    [Header("Referencias de Objetos")]
    [SerializeField] private Transform[] vasos; 
    [SerializeField] private GameObject bolita; 

    [Header("Ajustes de Tiempos y Velocidad")]
    [SerializeField] private float alturaLevantado = 2f;
    [SerializeField] private float tiempoMezcla = 10f;
    [SerializeField] private float tiempoDecision = 5f;
    
    [Tooltip("A mayor valor, más lento será el intercambio de los vasos.")]
    [SerializeField] private float duracionIntercambio = 0.5f; 
    #endregion

    #region Variables de Estado
    private int indiceGanador;
    private bool puedeElegir = false;
    private bool juegoTerminado = false;
    #endregion

    #region Métodos de Unity
    void Start()
    {
        // Iniciamos el flujo del minijuego
        StartCoroutine(SecuenciaJuego());
    }
    #endregion

    #region Corrutinas de Juego
    /// <summary>
    /// Ejecuta la secuencia: Mostrar bolita, mezclar vasos y esperar selección.
    /// </summary>
    IEnumerator SecuenciaJuego()
    {
        // 1. Configuración inicial de la bolita
        indiceGanador = Random.Range(0, vasos.Length);
        bolita.transform.position = vasos[indiceGanador].position;
        bolita.transform.SetParent(null); 

        // 2. Fase de Revelación Inicial
        Vector3 posOriginal = vasos[indiceGanador].position;
        yield return MoverVaso(vasos[indiceGanador], posOriginal + new Vector3(0, alturaLevantado, 0), 0.5f);
        yield return new WaitForSeconds(1f);
        yield return MoverVaso(vasos[indiceGanador], posOriginal, 0.5f);

        // Emparentamos para que la bolita se mueva con el vaso durante la mezcla
        bolita.transform.SetParent(vasos[indiceGanador]);

        // 3. Fase de Mezcla
        float tiempoPasado = 0;
        while (tiempoPasado < tiempoMezcla)
        {
            int a = Random.Range(0, vasos.Length);
            int b = Random.Range(0, vasos.Length);
            while (a == b) b = Random.Range(0, vasos.Length);

            yield return IntercambiarVasos(a, b);
            tiempoPasado += duracionIntercambio; 
        }

        // 4. Fase de Selección
        puedeElegir = true;
        Debug.Log("SISTEMA: Elige un vaso...");
        yield return new WaitForSeconds(tiempoDecision);

        if (!juegoTerminado)
        {
            Debug.Log("RESULTADO: Tiempo agotado.");
            FinalizarJuego(false);
        }
    }

    /// <summary>
    /// Intercambia la posición de dos vasos asegurando que la posición final sea exacta.
    /// </summary>
    IEnumerator IntercambiarVasos(int idxA, int idxB)
    {
        Vector3 posA = vasos[idxA].position;
        Vector3 posB = vasos[idxB].position;
        float t = 0;

        while (t < 1f)
        {
            // El incremento de t depende del framerate y la duración configurada
            t += Time.deltaTime / duracionIntercambio;
            float curvaSeno = Mathf.Sin(t * Mathf.PI) * 0.5f;
            
            vasos[idxA].position = Vector3.Lerp(posA, posB, t) + new Vector3(0, curvaSeno, 0);
            vasos[idxB].position = Vector3.Lerp(posB, posA, t) + new Vector3(0, -curvaSeno, 0);
            yield return null;
        }

        // Snap final: Forzamos la posición exacta para evitar desalineación por velocidad alta
        vasos[idxA].position = posB;
        vasos[idxB].position = posA;
    }

    /// <summary>
    /// Mueve un vaso a un destino específico y asegura el centrado al finalizar.
    /// </summary>
    IEnumerator MoverVaso(Transform vaso, Vector3 destino, float tiempo)
    {
        Vector3 inicio = vaso.position;
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime / tiempo;
            vaso.position = Vector3.Lerp(inicio, destino, t);
            yield return null;
        }

        // Corregimos cualquier desfase decimal al terminar el movimiento
        vaso.position = destino;
    }
    #endregion

    #region Interacción y Finalización
    /// <summary>
    /// Gestiona el click del jugador sobre un vaso.
    /// </summary>
    /// <param name="vasoClickeado">Transform del vaso seleccionado.</param>
    public void IntentarSeleccion(Transform vasoClickeado)
    {
        if (!puedeElegir || juegoTerminado) return;

        juegoTerminado = true;
        bool esCorrecto = (vasoClickeado == vasos[indiceGanador]);

        if (esCorrecto)
        {
            // Liberamos la bolita para que se quede en el suelo al subir el vaso
            bolita.transform.SetParent(null); 
            Debug.Log("RESULTADO: ¡Has acertado!");
        }
        else
        {
            Debug.Log("RESULTADO: Vaso vacío.");
        }

        StartCoroutine(MoverVaso(vasoClickeado, vasoClickeado.position + new Vector3(0, alturaLevantado, 0), 0.3f));
        FinalizarJuego(esCorrecto);
    }

    void FinalizarJuego(bool ganado)
    {
        if (GameManager.instancia != null)
        {
            if (ganado) GameManager.instancia.Ganar();
            else GameManager.instancia.Perder();
        }
    }
    #endregion
}