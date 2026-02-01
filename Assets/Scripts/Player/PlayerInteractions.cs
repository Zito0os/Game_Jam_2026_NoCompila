using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInteractions : MonoBehaviour
{
    public Transform startPosition;
    private List<Collider> itemsInTrigger = new List<Collider>();

    void Update()
    {
        //Detectar presión de E para recoger items
        if (Input.GetKeyDown(KeyCode.E))
        {
            for (int i = itemsInTrigger.Count - 1; i >= 0; i--)
            {
                Collider other = itemsInTrigger[i];

                if (other.gameObject.CompareTag("Oxigeno_Recuperar"))
                {
                    GameManager.Instance.AgregarOxigeno(other.gameObject.GetComponent<Tanque_oxigeno>().oxigenoRestaurar);
                    Destroy(other.gameObject);
                    itemsInTrigger.RemoveAt(i);
                }
                else if (other.gameObject.CompareTag("Mask_Recuperar"))
                {
                    GameManager.Instance.AgregarMascara(other.gameObject.GetComponent<Mascara>().mascara);
                    Destroy(other.gameObject);
                    itemsInTrigger.RemoveAt(i);
                }
            }
        }
    }
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
            //accedemos al gamemanager , le añadimos la vida de la caja
            GameManager.Instance.AddHealth(other.gameObject.GetComponent<HealthObject>().health);

            Destroy(other.gameObject);
        }

        //Agregar items a la lista para recoger con E
        if (other.gameObject.CompareTag("Oxigeno_Recuperar") || other.gameObject.CompareTag("Mask_Recuperar"))
        {
            itemsInTrigger.Add(other);
        }
        //if (other.gameObject.CompareTag("Oxigeno_Recuperar") && Input.GetKeyDown(KeyCode.E))
        //{
        //    //accedemos al gamemanager , le añadimos la municion de la caja , del script ammobox
        //    GameManager.Instance.AgregarOxigeno(other.gameObject.GetComponent<Tanque_oxigeno>().oxigenoRestaurar);

        //    Destroy(other.gameObject);
        //}
        //if (other.gameObject.CompareTag("Mask_Recuperar") && Input.GetKeyDown(KeyCode.E))
        //{
        //    //accedemos al gamemanager , le añadimos la municion de la caja , del script ammobox
        //    GameManager.Instance.AgregarMascara(other.gameObject.GetComponent<Mascara>().mascara);

        //    Destroy(other.gameObject);
        //}

        if (other.gameObject.CompareTag("DeathFloor"))
        {
            //perder vida
            GameManager.Instance.LoseHealth(50);
            //respawnear a nuestro player

            GetComponent<CharacterController>().enabled = false; // Desactivar el CharacterController temporalmente
            gameObject.transform.position = startPosition.position;
            GetComponent<CharacterController>().enabled = true; // Reactivar el CharacterController

        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Remover items de la lista si salimos del trigger
        if (other.gameObject.CompareTag("Oxigeno_Recuperar") || other.gameObject.CompareTag("Mask_Recuperar"))
        {
            itemsInTrigger.Remove(other);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            //perder vida - DESACTIVADO: Solo balas visuales
            //GameManager.Instance.LoseHealth(5);
            Debug.Log("Bala enemiga detectada (visual solamente)");
        }

    }


}
