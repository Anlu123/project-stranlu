using UnityEngine;

public class DestruirEnColision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Verificar si el objeto colisionado tiene una etiqueta específica
        if (collision.gameObject.CompareTag("Policia"))
        {
            // Destruir este objeto
            Destroy(gameObject);
        }
    }
}
