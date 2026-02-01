using UnityEditor;
using UnityEngine;

public class Primera_aparicion : MonoBehaviour
{
    
    //public Transform spawner_de_enemigo;
    private bool primera_aparicion;
    public Animator animator;

    // Update is called once per frame

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            primera_aparicion = true;

            animator.SetBool("aparece", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            primera_aparicion = false;

            animator.SetBool("aparece", false);
        }
    }

}
