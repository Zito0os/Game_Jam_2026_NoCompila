using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {

        //checa si la bala colision� con un enemigo
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Buscar el script AI_Enemy_Hombre en el objeto actual
            AI_Enemy_Hombre enemy = collision.gameObject.GetComponent<AI_Enemy_Hombre>();
            
            // Si no lo encuentra, buscar en el padre
            if (enemy == null)
            {
                enemy = collision.gameObject.GetComponentInParent<AI_Enemy_Hombre>();
            }

            // Si lo encuentra, hacer daño
            if (enemy != null)
            {
                enemy.LooseLife(20);
                Debug.Log("Bala impactó enemigo - Daño: 20");
            }
            else
            {
                Debug.LogError("No se encontró AI_Enemy_Hombre en " + collision.gameObject.name);
            }

            // Destruir la bala
            Destroy(gameObject);
        }


        if (collision.gameObject.CompareTag("Destruible"))
        {

           

            // Destroy the enemy
            Destroy(collision.gameObject);
            // Destroy the bullet
            //Destroy(gameObject);
        }



    }
}
