using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.AI;

public class AI_Enemy_Hombre : MonoBehaviour
{

    public NavMeshAgent naveMeshAgent;

    public Transform[] destinations;

    public float distanceToFollowPath;

    private int i = 0;

    public bool followPlayer;

    //[Header("---------Follow Player---------")]

    private float distanceToPlayer;

    public float distanceToFollowPlayer = 15;
    public float attackRange = 2.5f; // Rango de ataque
    public float maxHeightDifference = 1.5f; // Diferencia máxima de altura permitida

    private GameObject player;

    public float liveEnemy = 100;
    public bool isAlive = true;

    [Header("Attack Settings")]
    public float attackDamage = 10f;
    public float attackCooldown = 1.5f;
    private float nextAttackTime = 0f;
    public Animator animator;
    public string attackTrigger = "Attack";

    public bool atacando;

    public float morirAfterSeconds = 3.8f;

    [Header("Random Movement")]
    public bool useRandomMovement = false; // Si no hay destinations, usar movimiento random
    public float randomMoveRadius = 20f; // Radio para generar posiciones random
    private Vector3 randomDestination;
    private bool usingRandomMovement = false;

    //public GameObject destination1;
    //public GameObject destination2;

    void Start()
    {
        //busca el objeto del jugador en la escena que tenga el script PlayerMovement
        player = FindObjectOfType<PlayerMovement>().gameObject;

        // Obtener animator si no está asignado
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        // Inicializar el estado de vida en el animator
        if (animator != null)
        {
            animator.SetBool("isAlive", true);
        }

        // Verificar destinations
        if (destinations == null || destinations.Length == 0 || destinations[0] == null)
        {
            // Si no hay destinations válidos, usar movimiento random
            useRandomMovement = true;
            usingRandomMovement = true;
            GenerateRandomDestination();
            naveMeshAgent.destination = randomDestination;
            Debug.Log("No hay destinations válidos asignados, usando movimiento random");
            
            // Desactivar el script AI base si existe
            AI aiScript = GetComponent<AI>();
            if (aiScript != null)
            {
                aiScript.enabled = false;
            }
        }
        else
        {
            //aqui mandamos al agente a el destino que es el destination1
            naveMeshAgent.destination = destinations[0].transform.position;
            usingRandomMovement = false;
        }
    }


    void Update()
    {
        // Si el enemigo está muerto, no ejecutar ninguna lógica
        if (!isAlive) return;

        //si pones solo transform entra al que tiene asignado el script en este caso es el enemy
        //calcula la distancia de punto a a punto b 
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // Verificar diferencia de altura
        float heightDifference = Mathf.Abs(transform.position.y - player.transform.position.y);
        bool canEngagePlayer = heightDifference <= maxHeightDifference;

        if (distanceToPlayer <= attackRange && followPlayer && canEngagePlayer)
        {
            // Está en rango de ataque - detenerse y atacar
            AttackPlayer();
        }
        else if (distanceToPlayer <= distanceToFollowPlayer && followPlayer && canEngagePlayer)
        {
            // Está en rango de seguimiento pero no de ataque
            FollowPlayer();
        }
        else
        {
            //si no esta cerca del jugador entonces sigue el camino
            EnemyPath();
        }
        animator.SetBool("atacando", atacando);





    }

    public void EnemyPath()
    {
        atacando = false;
        naveMeshAgent.isStopped = false; // Asegurarse que puede moverse

        // Si está usando movimiento random
        if (usingRandomMovement)
        {
            naveMeshAgent.destination = randomDestination;

            // Si llegó a la posición random, generar una nueva
            if (Vector3.Distance(transform.position, randomDestination) <= distanceToFollowPath)
            {
                GenerateRandomDestination();
            }
        }
        else
        {
            // Usar destinations normales
            naveMeshAgent.destination = destinations[i].transform.position;

            //si la distancia entre el agente y el destino es menor o igual a la distancia que queremos para seguir el camino, entonces cambiamos al siguiente destino
            if (Vector3.Distance(transform.position, destinations[i].position) <= distanceToFollowPath)
            {
                //si el destino actual no es el ultimo destino, entonces cambiamos al siguiente destino
                if (destinations[i] != destinations[destinations.Length - 1])
                {
                    i = i + 1;
                }
                else
                {
                    i = 0; //si es el ultimo destino, entonces volvemos al primer destino
                }
            }
        }
    }

    void GenerateRandomDestination()
    {
        // Generar una posición random alrededor de la posición actual
        Vector2 randomCircle = Random.insideUnitCircle * randomMoveRadius;
        Vector3 randomPoint = transform.position + new Vector3(randomCircle.x, 0, randomCircle.y);

        // Verificar si el punto está en el NavMesh
        UnityEngine.AI.NavMeshHit hit;
        if (UnityEngine.AI.NavMesh.SamplePosition(randomPoint, out hit, randomMoveRadius, UnityEngine.AI.NavMesh.AllAreas))
        {
            randomDestination = hit.position;
            Debug.Log("Nueva posición random generada: " + randomDestination);
        }
        else
        {
            // Si no encuentra un punto válido, usar la posición actual
            randomDestination = transform.position;
            Debug.LogWarning("No se encontró un punto válido en el NavMesh, quedándose en posición actual");
        }
    }




    public void FollowPlayer()
    {
        atacando = false;
        // Aqui podemos hacer que el agente siga al jugador
        naveMeshAgent.isStopped = false; // Asegurarse que puede moverse
        naveMeshAgent.destination = player.transform.position;
    }

    public void AttackPlayer()
    {
        
        atacando = true;
        // Detener el movimiento del agente
        naveMeshAgent.isStopped = true;

        // Mirar hacia el jugador
        Vector3 direction = (player.transform.position - transform.position).normalized;
        direction.y = 0; // Mantener solo rotación horizontal
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        // Verificar si puede atacar (cooldown)
        if (Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown;

            //// Reproducir animación de ataque si existe
            //if (animator != null && !string.IsNullOrEmpty(attackTrigger))
            //{
            //    animator.SetBool("atacando",true);
            //}

            // Hacer daño al jugador
            if (GameManager.Instance != null)
            {
                GameManager.Instance.PerderMascara(10);

                GameManager.Instance.LoseHealth((int)attackDamage);
                Debug.Log("Enemigo atacó al jugador causando " + attackDamage + " de daño");
            }
            else
            {
                Debug.LogError("GameManager.Instance es NULL!");
            }
        }
    }


    public void GrenadeImpact(float damage)
    {
        LooseLife(damage);
    }


    public void LooseLife(float LiveToLose)
    {
        if (!isAlive)
        {
            Debug.Log("Enemigo ya está muerto, ignorando daño");
            return; // Si ya está muerto, no hacer nada
        }

        liveEnemy = liveEnemy - LiveToLose;
        Debug.Log("Enemigo recibió " + LiveToLose + " de daño. Vida restante: " + liveEnemy);

        if (liveEnemy <= 0)
        {
            Debug.Log("Enemigo muerto, iniciando Die()");
            Die();
        }
    }

    void Die()
    {
        isAlive = false;
        atacando = false;
        animator.SetBool("isAlive", isAlive);

        Debug.Log("Enemigo muriendo - iniciando animación");

        // Cambiar el estado de animación
        if (animator != null)
        {
            animator.SetBool("isAlive", false);
            animator.SetBool("atacando", false);
            Debug.Log("Animator isAlive establecido a false");
        }
        else
        {
            Debug.LogError("Animator es NULL!");
        }

        // Desactivar el NavMeshAgent
        if (naveMeshAgent != null)
        {
            naveMeshAgent.isStopped = true;
            naveMeshAgent.velocity = Vector3.zero;
        }

        // Destruir después de 5 segundos
        Destroy(gameObject,morirAfterSeconds);
        Debug.Log("Enemigo será destruido en 5 segundos");
    }


}
