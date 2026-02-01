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

    //public GameObject destination1;
    //public GameObject destination2;

    void Start()
    {
        if (destinations == null || destinations.Length == 0)
        {
            transform.gameObject.GetComponent<AI>().enabled = false;
        }

        //aqui mandamos al agente a el destino que es el destination1
        naveMeshAgent.destination = destinations[0].transform.position;

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
