using UnityEngine;
using UnityEngine.UIElements;

public class EnemyShoot : MonoBehaviour
{
    public GameObject enemyBullet;
    public Transform sapwnBulletPoint;

    private Transform playerPosition;
    public float bulletVelocity = 100f;





    // Referencia al script del enemigo
    private AI_Enemy_Hombre enemyScript;

    // Variables para detectar el estado
    private float distanceToPlayer;
    private GameObject player;

    // Cooldown para disparos
    private float shootCooldown = 2f;
    private float nextShootTime = 0f;


    void Start()
    {
        playerPosition = FindObjectOfType<PlayerInteractions>().transform;

        // Obtener el script del enemigo en este GameObject
        enemyScript = GetComponent<AI_Enemy_Hombre>();

        // Buscar el jugador
        player = FindObjectOfType<PlayerMovement>().gameObject;

        Debug.Log("EnemyShoot inicializado. EnemyScript: " + (enemyScript != null) + ", Player: " + (player != null));
    }

    
    void Update()
    {
        // Si no hay script de enemigo o jugador, no hacer nada
        if (enemyScript == null || player == null)
        {
            Debug.LogWarning("EnemyScript o Player es null");
            return;
        }

        // Calcular distancia al jugador
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // Detectar el estado del enemigo basándose en su lógica
        if (distanceToPlayer <= enemyScript.attackRange && enemyScript.followPlayer && enemyScript.isAlive)
        {
            // Solo disparar si ha pasado el cooldown
            if (Time.time >= nextShootTime)
            {
                ShootPlayer();
                nextShootTime = Time.time + shootCooldown;
            }
        }
    }

    void ShootPlayer()
    {
        Debug.Log("Disparando al jugador");
        Vector3 playerDirection = playerPosition.position - transform.position;

        GameObject newbullet;

        newbullet = Instantiate(enemyBullet, sapwnBulletPoint.position, sapwnBulletPoint.rotation);

        if (newbullet != null)
        {
            Rigidbody rb = newbullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(playerDirection * bulletVelocity, ForceMode.Force);
                Debug.Log("Bala instanciada y fuerza aplicada");
            }
            else
            {
                Debug.LogError("La bala no tiene Rigidbody!");
            }
        }
        else
        {
            Debug.LogError("No se pudo instanciar la bala!");
        }
    }
}
