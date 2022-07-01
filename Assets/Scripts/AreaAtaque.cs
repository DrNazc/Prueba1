using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaAtaque : MonoBehaviour
{
    public Vector2 knockbackForce;
    public Transform playerTransform;
    

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Enemigo"))
        {
            Debug.Log("Aplicar daño a enemigo");
            // TODO: Apply knockback
            Debug.Log("Apply knockback!");
            // Obtenemos el componente Rigidbody2D del objeto enemigo (collider)

            Rigidbody2D enemyRb = collision.GetComponent<Rigidbody2D>();
            // Obtener la direccion hacia la que mira el personaje
            float dir = playerTransform.localScale.x;
            // Ajustar el valor del knockback horizontal para que cuadre con la dir

            Vector2 knockbackToApply = new Vector2(knockbackForce.x * dir, knockbackForce.y);
            // Aplicar la fuerza en el rigidbody del enemigo para simular el knockback que se desea aplicar
            enemyRb.AddForce(knockbackToApply, ForceMode2D.Impulse);


        }
    }
}
