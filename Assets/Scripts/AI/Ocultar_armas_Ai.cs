using UnityEngine;

public class Ocultar_armas_Ai : MonoBehaviour
{
    public Transform posicionA;
    public Transform posicionB;
    public Transform posicionC;

    // El objeto que se va a mover
    public Transform objeto;

    // Referencia al script del enemigo
    private AI_Enemy_Hombre enemyScript;

    // Variables para detectar el estado
    private float distanceToPlayer;
    private GameObject player;

    void Start()
    {
        // Obtener el script del enemigo en este GameObject
        enemyScript = GetComponent<AI_Enemy_Hombre>();

        // Buscar el jugador
        player = FindObjectOfType<PlayerMovement>().gameObject;
    }

    void Update()
    {
        // Si no hay script de enemigo, no hacer nada
        if (enemyScript == null || player == null) return;

        // Calcular distancia al jugador
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // Detectar el estado del enemigo basándose en su lógica
        if (distanceToPlayer <= enemyScript.attackRange && enemyScript.followPlayer && enemyScript.isAlive)
        {
            // Está atacando - mover a posición A
            MoverObjeto(posicionA);
        }
        else if (distanceToPlayer <= enemyScript.distanceToFollowPlayer && enemyScript.followPlayer && enemyScript.isAlive)
        {
            // Está siguiendo al jugador - mover a posición B
            MoverObjeto(posicionB);
        }
        else
        {
            // Está patrullando o está muerto - mover a posición C
            MoverObjeto(posicionC);
        }
    }

    void MoverObjeto(Transform destino)
    {
        objeto.position = destino.position;
    }
}
