using UnityEngine;

public class Primer_Spawneo : MonoBehaviour
{
    public GameObject player;
    public GameObject prefabToSpawn;
    public Transform spawnPosition;

    private bool yaActivado = false;




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

            // Spawnear el prefab
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
}
