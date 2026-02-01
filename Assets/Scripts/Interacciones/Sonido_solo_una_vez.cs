using UnityEngine;

public class Sonido_solo_una_vez : MonoBehaviour
{
    private AudioSource audioSource;
    private bool yaReproducido = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
        if (audioSource == null)
        {
            Debug.LogWarning("No hay AudioSource en " + gameObject.name);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !yaReproducido)
        {
            yaReproducido = true;

            if (audioSource != null && audioSource.clip != null)
            {
                audioSource.Play();
                Debug.Log("Reproduciendo sonido: " + audioSource.clip.name);
                
                // Desactivar el trigger después de que termine el audio
                float audioDuration = audioSource.clip.length;
                Invoke("DesactivarTrigger", audioDuration);
            }
            else
            {
                Debug.LogWarning("No hay AudioClip asignado");
                DesactivarTrigger();
            }
        }
    }

    void DesactivarTrigger()
    {
        Debug.Log("Trigger desactivado");
        gameObject.SetActive(false);
    }
}
