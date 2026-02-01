using System.Collections;
using UnityEngine;

public class PlayerOxigen : MonoBehaviour
{
    public bool esta_usando_mascara = false;
    public bool esta_corriendo = false;

    private Coroutine oxigenoCoroutine;
    private Coroutine vidaCoroutine;
    private Coroutine sprintCoroutine;


    public GameObject Mascarilla;
    public Transform posicionMascaraGuardada; // Posición detrás del jugador donde se guardará la máscara

    public Animator animator;

    void Start()
    {
        // No inicializar nada aquí, solo esperar a que el jugador presione C
        Mascarilla.gameObject.SetActive(false); // La máscara empieza visible
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.C))
        {
            esta_usando_mascara = !esta_usando_mascara;
            GameManager.Instance.cambiar_bool_enmascarado(esta_usando_mascara);
            animator.SetBool("enmascarado", esta_usando_mascara);
            

            
            CambiarEstado();
        }
        

        bool sprinting = FindObjectOfType<PlayerMovement>().isSprinting;
        if (sprinting && !esta_corriendo)
        {
            esta_corriendo = true;
            IniciarSprintOxigeno();
        }
        else if (!sprinting && esta_corriendo)
        {
            esta_corriendo = false;
            DetenerSprintOxigeno();
        }


    }

    void CambiarEstado()
    {
       
        //animator.SetBool("enmascarado", GameManager.Instance.esta_enmascarado);
        // Detener corrutinas activas
        if (oxigenoCoroutine != null)
        {
            StopCoroutine(oxigenoCoroutine);
            oxigenoCoroutine = null;
        }

        if (vidaCoroutine != null)
        {
            StopCoroutine(vidaCoroutine);
            vidaCoroutine = null;
        }


        //Cambiar animación según el estado de la máscara
        //if (animator != null)
        //{
            

        //    Debug.Log("Animator: enmascarado = " + esta_usando_mascara);
        //}
        //else
        //{
        //    Debug.LogWarning("Animator no está asignado en el Inspector!");
        //}

        // Iniciar la correcta
        if (esta_usando_mascara)
        {
            oxigenoCoroutine = StartCoroutine(PerderOxigeno_Mascara());
            vidaCoroutine = StartCoroutine(Ganar_vida());
            Mascarilla.gameObject.SetActive(true);
        }
        else
        {
            vidaCoroutine = StartCoroutine(Perder_vida());
            // Mover la máscara a la posición guardada con un delay de 3 segundos
            //StartCoroutine(GuardarMascaraConDelay());
        }

    }

    //IEnumerator GuardarMascaraConDelay()
    //{
    //    // Esperar 3 segundos para que se reproduzca la animación de quitar la máscara
    //    yield return new WaitForSeconds(3f);
    //    // Mover la máscara a la posición guardada
    //    if (posicionMascaraGuardada != null)
    //    {
    //        Mascarilla.transform.position = posicionMascaraGuardada.position;
    //        Mascarilla.transform.rotation = posicionMascaraGuardada.rotation;
    //    }
    //    else
    //    {
    //        Debug.LogWarning("posicionMascaraGuardada no está asignada en el Inspector!");
    //    }

    //}






    IEnumerator PerderOxigeno_Mascara()
    {
        while (GameManager.Instance.health > 0)
        {
            if (GameManager.Instance.oxigeno > 0)
            {
                // Mientras hay oxígeno, quitar oxígeno y máscara
                yield return new WaitForSeconds(4f);
                GameManager.Instance.PerderOxigeno(4);
                GameManager.Instance.PerderMascara(1);
            }
            else
            {
                // Cuando el oxígeno llega a 0, quitar vida
                yield return new WaitForSeconds(2f);
                GameManager.Instance.LoseHealth(5);
            }
        }
    }


    IEnumerator Perder_vida()
    {
        while (GameManager.Instance.health > 0)
        {
            yield return new WaitForSeconds(3f);
            GameManager.Instance.LoseHealth(5);
        }
    }

    IEnumerator Ganar_vida()
    {
        while (GameManager.Instance.health > 0 && esta_usando_mascara)
        {
            yield return new WaitForSeconds(2f);
            GameManager.Instance.AddHealth(2);

        }
    }

    void IniciarSprintOxigeno()
    {
        if (sprintCoroutine != null)
        {
            StopCoroutine(sprintCoroutine);
        }
        sprintCoroutine = StartCoroutine(Perder_oxigeno_Sprint());
    }

    void DetenerSprintOxigeno()
    {
        if (sprintCoroutine != null)
        {
            StopCoroutine(sprintCoroutine);
            sprintCoroutine = null;
        }
    }

    IEnumerator Perder_oxigeno_Sprint()
    {
        while (GameManager.Instance.oxigeno > 0 && esta_corriendo)
        {
            GameManager.Instance.PerderOxigeno(5);
            yield return new WaitForSeconds(1f);
        }
    }





}
