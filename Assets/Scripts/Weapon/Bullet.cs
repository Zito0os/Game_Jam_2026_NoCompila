using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {

        //checa si la bala colisionó con un enemigo
        if (collision.gameObject.CompareTag("Enemy"))
        {

            collision.gameObject.GetComponent<AI>().LooseLife(20);

            // Destroy the enemy
            //Destroy(collision.gameObject);
            // Destroy the bullet
            //Destroy(gameObject);
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
