using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInteractions : MonoBehaviour
{
    public Transform startPosition;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("GunAmmo"))
        {
            //accedemos al gamemanager , le añadimos la municion de la caja , del script ammobox
            GameManager.Instance.gunammo += other.gameObject.GetComponent<AmmoBox>().ammo;
            
            Destroy(other.gameObject);
        }


        if (other.gameObject.CompareTag("HealthObject"))
        {
            //accedemos al gamemanager , le añadimos la municion de la caja , del script ammobox
            GameManager.Instance.AddHealth(other.gameObject.GetComponent<HealthObject>().health);

            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("DeathFloor"))
        {
            //perder vida
            GameManager.Instance.LoseHealth(50);
            //respawnear a nuestro player

            GetComponent<CharacterController>().enabled = false; // Desactivar el CharacterController temporalmente
            gameObject.transform.position = startPosition.position;
            GetComponent<CharacterController>().enabled = true; // Desactivar el CharacterController temporalmente

        }


    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            //perder vida
            GameManager.Instance.LoseHealth(5);
            
        }

    }


}
