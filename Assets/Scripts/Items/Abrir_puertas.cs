using UnityEngine;

public class Abrir_puertas : MonoBehaviour
{
    public Animator animator;
    public bool abrirAutomatico = true; // Si es true, se abre automáticamente. Si es false, necesita presionar E
    private bool playerCerca = false;
    private bool puertaAbierta = false;

    void Update()
    {
        // Si no es automático y el jugador está cerca, puede abrir con E
        if (!abrirAutomatico && playerCerca && Input.GetKeyDown(KeyCode.E))
        {
            puertaAbierta = !puertaAbierta;
            animator.SetBool("abrir_puerta", puertaAbierta);
            Debug.Log("Puerta " + (puertaAbierta ? "abierta" : "cerrada") + " con tecla E");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerCerca = true;

            // Si es automático, abrir la puerta
            if (abrirAutomatico)
            {
                puertaAbierta = true;
                animator.SetBool("abrir_puerta", true);
                Debug.Log("Puerta abierta automáticamente");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerCerca = false;

            // Si es automático, cerrar la puerta
            if (abrirAutomatico)
            {
                puertaAbierta = false;
                animator.SetBool("abrir_puerta", false);
                Debug.Log("Puerta cerrada automáticamente");
            }
        }
    }
}
