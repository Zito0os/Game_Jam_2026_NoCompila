using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public GameObject enemyBullet;
    public Transform sapwnBulletPoint;

    private Transform playerPosition;
    public float bulletVelocity = 100f;



    void Start()
    {
        playerPosition = FindObjectOfType<PlayerInteractions>().transform;

        Invoke("ShootPlayer",3);
    }

    
    void Update()
    {
        
    }

    void ShootPlayer()
    {
        Vector3 playerDirection = playerPosition.position - transform.position;

        GameObject newbullet;

        newbullet = Instantiate(enemyBullet,sapwnBulletPoint.position, sapwnBulletPoint.rotation);

        newbullet.GetComponent<Rigidbody>().AddForce(playerDirection * bulletVelocity,ForceMode.Force);

        Invoke("ShootPlayer", 3);
    }
}
