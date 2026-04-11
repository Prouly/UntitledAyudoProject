/**
 * Proyecto: Smoothie Criminal
 * Autor: Álvaro Muñoz Adán
 * Descripción: Detecta la interacción del ratón con los vasos individuales.
 * Última modificación: 10/04/2026
 */

using UnityEngine;

public class VasoClick : MonoBehaviour
{
    #region Variables Privadas
    private ShellGameLogic logic;
    #endregion

    #region Métodos de Unity
    void Start()
    {
        // Localizamos el controlador del minijuego en la escena
        logic = FindObjectOfType<ShellGameLogic>();
    }

    /// <summary>
    /// Evento de Unity disparado al hacer click sobre el Collider del vaso.
    /// </summary>
    private void OnMouseDown()
    {
        if (logic != null)
        {
            logic.IntentarSeleccion(this.transform);
        }
    }
    #endregion
}