using UnityEngine;

public class Cuchillo_take : MonoBehaviour
{
    public bool tiene_cuchillo = false;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            tiene_cuchillo = true;
            Debug.Log("¡Cuchillo recogido!");

            Destroy(gameObject);
        }
    }
}
