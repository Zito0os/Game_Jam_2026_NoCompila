using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.AI;

public class AI : MonoBehaviour
{

    public NavMeshAgent naveMeshAgent;

    public Transform[] destinations;

    public float distanceToFollowPath;

    private int i = 0;

    public bool followPlayer;

    //[Header("---------Follow Player---------")]

    private float distanceToPlayer;

    public float distanceToFollowPlayer = 15;

    private GameObject player;

    public float liveEnemy = 100;

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
    }


    void Update()
    {

        //float distance = Vector3.Distance(transform.position, destination1.transform.position);

        ////usando las distancias podemos hacer de todos que vaya en un loop de un lado al otro o asi o que si esta cerca de nosotros pues que nos empiece a seguir
        //if (distance < 2) {

        //    //aqui mandamos al agente a el destino que es el destination2
        //    naveMeshAgent.destination = destination2.transform.position;
        //}





        //si pones solo transform entra al que tiene asignado el script en este caso es el enemy
        //calcula la distancia de punto a a punto b 
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= distanceToFollowPlayer && followPlayer)
        {
            FollowPlayer();
        }
        else
        {
            //si no esta cerca del jugador entonces sigue el camino
            EnemyPath();
        }




    }

    public void EnemyPath()
    {
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
        // Aqui podemos hacer que el agente siga al jugador
        naveMeshAgent.destination = player.transform.position;
    }


    public void GrenadeImpact(float damage)
    {
        LooseLife(damage);
    }


    public void LooseLife(float LiveToLose)
    {
        liveEnemy = liveEnemy - LiveToLose;

        if(liveEnemy <= 0)
        {

            Destroy(gameObject);
        }
    }


}
