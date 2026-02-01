using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiarEscenaCualquierTecla : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float tiempoEspera = 4f;
    [SerializeField] private bool wrapAround = true;
    [SerializeField] private bool mostrarContador = true;

    private float tiempoInicio;
    private bool puedeCambiar = false;

    private void Start()
    {
        tiempoInicio = Time.time;
    }

    private void Update()
    {

        float tiempoTranscurrido = Time.time - tiempoInicio;


        if (!puedeCambiar && tiempoTranscurrido >= tiempoEspera)
        {
            puedeCambiar = true;

            if (mostrarContador)
                Debug.Log("¡Ya puedes presionar cualquier tecla para continuar!");
        }

        // Solo permite cambiar escena DESPUÉS del tiempo de espera
        if (puedeCambiar && Input.anyKeyDown)
        {
            IrASiguienteEscena();
        }

    }

    private void IrASiguienteEscena()
    {
        int indiceActual = SceneManager.GetActiveScene().buildIndex;
        int indiceAnterior = indiceActual + 1;


        SceneManager.LoadScene(indiceAnterior);
    }
}