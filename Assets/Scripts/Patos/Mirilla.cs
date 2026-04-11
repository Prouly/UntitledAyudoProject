using UnityEngine;

public class Mirilla : MonoBehaviour
{
    [Header("Límites de movimiento")]
    public Transform limiteIzquierdo;
    public Transform limiteDerecho;
    public Transform limiteSuperior;
    public Transform limiteInferior;

    void Start() => Cursor.visible = false;

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        // Restringir la posición entre los colliders
        float xClamped = Mathf.Clamp(mousePos.x, limiteIzquierdo.position.x, limiteDerecho.position.x);
        float yClamped = Mathf.Clamp(mousePos.y, limiteInferior.position.y, limiteSuperior.position.y);

        transform.position = new Vector3(xClamped, yClamped, 0);
    }
}