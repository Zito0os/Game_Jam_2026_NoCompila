using UnityEngine;

public class WeaponBobbing : MonoBehaviour
{
    [Header("References")]
    public CharacterController controller; // si usas CharacterController, así detecta velocidad real

    [Header("Bobbing Settings")]
    public float bobSpeed = 8f;
    public float bobAmount = 0.05f;     // cuánto sube/baja
    public float bobSideAmount = 0.03f; // cuánto se mueve a los lados
    public float returnSpeed = 10f;     // qué tan rápido vuelve al centro

    private Vector3 startLocalPos;
    private float timer;

    void Start()
    {
        startLocalPos = transform.localPosition;

        // Si no lo asignaste, intenta buscarlo en el player
        if (controller == null)
            controller = GetComponentInParent<CharacterController>();
    }

    void Update()
    {
        float moveMagnitude = GetMoveMagnitude();

        // Si no te estás moviendo, vuelve suavemente a la posición inicial
        if (moveMagnitude < 0.05f)
        {
            timer = 0f;
            transform.localPosition = Vector3.Lerp(transform.localPosition, startLocalPos, Time.deltaTime * returnSpeed);
            return;
        }

        timer += Time.deltaTime * bobSpeed;

        float y = Mathf.Sin(timer) * bobAmount;
        float x = Mathf.Cos(timer * 0.5f) * bobSideAmount;

        Vector3 target = startLocalPos + new Vector3(x, y, 0f);
        transform.localPosition = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * returnSpeed);
    }

    float GetMoveMagnitude()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        return new Vector2(h, v).magnitude;
    }

}
