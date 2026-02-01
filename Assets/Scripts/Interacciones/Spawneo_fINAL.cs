using UnityEngine;

public class Spawneo_final : MonoBehaviour
{
    public GameObject player;
    public GameObject prefabToSpawn;
    public Transform spawnPosition;

    private bool yaActivado = false;
    private bool spawneando = false;
    public float spawnInterval = 5f; // Intervalo entre spawns




    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !yaActivado)
        {
            yaActivado = true;

            // Activar el player
            if (player != null)
            {
                player.SetActive(true);
            }

            // Spawnear el prefab inicial
            if (prefabToSpawn != null)
            {
                Vector3 spawnPos = (spawnPosition != null) ? spawnPosition.position : transform.position;
                Quaternion spawnRot = (spawnPosition != null) ? spawnPosition.rotation : transform.rotation;
                
                GameObject spawned = Instantiate(prefabToSpawn, spawnPos, spawnRot);
                Debug.Log("Prefab spawneado: " + prefabToSpawn.name);
            }
            else
            {
                Debug.LogWarning("No hay prefab asignado para spawnear");
            }

            // Iniciar el sistema de spawneo continuo
            IniciarSpawneoContinuo();

            // Desactivar el collider inmediatamente
            Collider triggerCollider = GetComponent<Collider>();
            if (triggerCollider != null)
            {
                triggerCollider.enabled = false;
            }

            // Desactivar este GameObject completamente
            gameObject.SetActive(false);
        }
    }

    void IniciarSpawneoContinuo()
    {
        spawneando = true;
        Debug.Log("Sistema de spawneo continuo iniciado");
        InvokeRepeating("SpawnearEnemigo", spawnInterval, spawnInterval);
    }

    void SpawnearEnemigo()
    {
        // Verificar si el jugador está vivo
        if (GameManager.Instance != null && GameManager.Instance.health <= 0)
        {
            DetenerSpawneo();
            return;
        }

        // Spawnear el prefab
        if (prefabToSpawn != null)
        {
            Vector3 spawnPos = (spawnPosition != null) ? spawnPosition.position : transform.position;
            Quaternion spawnRot = (spawnPosition != null) ? spawnPosition.rotation : transform.rotation;
            
            GameObject spawned = Instantiate(prefabToSpawn, spawnPos, spawnRot);
            Debug.Log("Enemigo spawneado automáticamente: " + prefabToSpawn.name);
        }
    }

    void DetenerSpawneo()
    {
        if (spawneando)
        {
            CancelInvoke("SpawnearEnemigo");
            spawneando = false;
            Debug.Log("Sistema de spawneo detenido - Jugador muerto");
        }
    }
}
