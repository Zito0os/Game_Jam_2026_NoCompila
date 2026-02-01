using UnityEngine;

public class Tomar_nota : MonoBehaviour
{
    private bool tomar_nota;

    public Animator animator;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tomar_nota = true;

            animator.SetBool("Tomo_papel", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tomar_nota = false;
            
            animator.SetBool("Tomo_papel", false);
        }
    }
}
