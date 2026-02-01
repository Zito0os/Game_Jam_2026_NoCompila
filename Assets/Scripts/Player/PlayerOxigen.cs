using System.Collections;
using UnityEngine;

public class PlayerOxigen : MonoBehaviour
{
    [Header("Estado")]
    public bool esta_usando_mascara = false;
    public bool esta_corriendo = false;

    private Coroutine oxigenoCoroutine;
    private Coroutine vidaCoroutine;
    private Coroutine sprintCoroutine;

    [Header("Referencias")]
    public GameObject Mascarilla;
    public Transform posicionMascaraGuardada; // opcional: dónde guardar la máscara al quitarla
    public Animator animator;

    [Header("FX Respiración")]
    public ParticleSystem respiracionFX;
    public bool limpiarParticulasAlQuitar = true;

    [Header("Animación máscara")]
    [Tooltip("Tiempo que tarda la animación de quitarse la máscara. Ajusta esto a la duración real de tu clip.")]
    public float delayQuitarMascara = 1.2f;

    // Cache
    private PlayerMovement playerMovement;
    private Coroutine quitarMascaraCoroutine;

    void Awake()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (Mascarilla == null)
            Debug.LogWarning("PlayerOxigen: Mascarilla no está asignada en el Inspector.");

        if (respiracionFX == null)
            Debug.LogWarning("PlayerOxigen: respiracionFX no está asignado en el Inspector.");
    }

    void Start()
    {
        // Estado inicial: sin máscara (según tu lógica actual)
        if (Mascarilla != null)
            Mascarilla.SetActive(false);

        if (respiracionFX != null)
            respiracionFX.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            esta_usando_mascara = !esta_usando_mascara;

            if (GameManager.Instance != null)
                GameManager.Instance.cambiar_bool_enmascarado(esta_usando_mascara);

            if (animator != null)
                animator.SetBool("enmascarado", esta_usando_mascara);

            CambiarEstado();
        }

        bool sprinting = (playerMovement != null) && playerMovement.isSprinting;

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

        // Si había una corrutina de "quitar máscara" corriendo, la cancelamos al cambiar estado
        if (quitarMascaraCoroutine != null)
        {
            StopCoroutine(quitarMascaraCoroutine);
            quitarMascaraCoroutine = null;
        }

        if (esta_usando_mascara)
        {
            // Asegurar que la máscara esté visible para animación/visual
            if (Mascarilla != null)
                Mascarilla.SetActive(true);

            // FX ON
            if (respiracionFX != null)
                respiracionFX.Play(true);

            oxigenoCoroutine = StartCoroutine(PerderOxigeno_Mascara());
            vidaCoroutine = StartCoroutine(Ganar_vida());
        }
        else
        {
            vidaCoroutine = StartCoroutine(Perder_vida());

            // FX OFF 
            if (respiracionFX != null)
            {
                respiracionFX.Stop(true,
                    limpiarParticulasAlQuitar
                        ? ParticleSystemStopBehavior.StopEmittingAndClear
                        : ParticleSystemStopBehavior.StopEmitting
                );
            }

            //esperar a que termine la animación
            quitarMascaraCoroutine = StartCoroutine(QuitarMascaraConDelay());
        }
    }

    IEnumerator QuitarMascaraConDelay()
    {
        // Espera para que se vea la animación de quitarse la máscara
        yield return new WaitForSeconds(delayQuitarMascara);

        // Mover la máscara a una posición guardada 
        if (Mascarilla != null && posicionMascaraGuardada != null)
        {
            Mascarilla.transform.position = posicionMascaraGuardada.position;
            Mascarilla.transform.rotation = posicionMascaraGuardada.rotation;
        }

        //  ocultar la máscara
        if (Mascarilla != null)
            Mascarilla.SetActive(false);

        quitarMascaraCoroutine = null;
    }

    IEnumerator PerderOxigeno_Mascara()
    {
        while (GameManager.Instance != null && GameManager.Instance.health > 0)
        {
            if (GameManager.Instance.oxigeno > 0)
            {
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
        while (GameManager.Instance != null && GameManager.Instance.health > 0)
        {
            yield return new WaitForSeconds(3f);
            GameManager.Instance.LoseHealth(5);
        }
    }

    IEnumerator Ganar_vida()
    {
        while (GameManager.Instance != null && GameManager.Instance.health > 0 && esta_usando_mascara)
        {
            yield return new WaitForSeconds(2f);
            GameManager.Instance.AddHealth(2);
        }
    }

    void IniciarSprintOxigeno()
    {
        if (sprintCoroutine != null)
            StopCoroutine(sprintCoroutine);

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
        while (GameManager.Instance != null && GameManager.Instance.oxigeno > 0 && esta_corriendo)
        {
            GameManager.Instance.PerderOxigeno(5);
            yield return new WaitForSeconds(1f);
        }
    }
}
