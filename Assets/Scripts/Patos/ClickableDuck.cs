using UnityEngine;

public class ClickableDuck : MonoBehaviour
{
    public float velocidad = 5f;
    private Vector2 direccion;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Dirección inicial: diagonal aleatoria hacia arriba
        float dirX = Random.Range(0, 2) == 0 ? -1 : 1;
        direccion = new Vector2(dirX, 1).normalized;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = direccion * velocidad;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Rebotar: invertimos la dirección según el impacto
        Vector2 normal = collision.contacts[0].normal;
        direccion = Vector2.Reflect(direccion, normal).normalized;
        
        // Evitar que se queden moviéndose solo en vertical/horizontal puro
        direccion.x += Random.Range(-0.1f, 0.1f);
        direccion = direccion.normalized;
    }

    private void OnMouseDown()
    {
        FindObjectOfType<DuckHuntLogic>().RegistrarMuerte();
        Debug.Log("Pato abatido");
        Destroy(gameObject); // O gameObject.SetActive(false);
    }
}