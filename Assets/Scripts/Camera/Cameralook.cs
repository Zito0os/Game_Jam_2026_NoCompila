using UnityEngine;

public class Cameralook : MonoBehaviour
{
    [Header("Look")]
    public float sensitivity = 80f;
    public Transform playerBody;

    [Header("Camera Rig")]
    public Transform cameraTransform;   // tu cámara real
    public Transform cameraPivot;       // pivot que rota con pitch (este objeto suele ser el mismo que tiene este script)

    [Header("Camera Height / Offset")]
    public float cameraHeight = 1.6f;   // ajusta altura aquí
    public Vector3 extraOffset = Vector3.zero; // opcional por si quieres mover un poquito en X/Z

    [Header("Bobbing")]
    public float anguloBalanceo = 10f;
    public float suavizado = 6f;
    public float bobSpeed = 6f;

    [Header("Bobbing Smooth Start")]
    public float startBobDelay = 0.12f; // delay antes de que empiece a balancear al caminar
    public float bobRampTime = 0.20f;   // tiempo para subir de 0 a 1 el bobbing

    private float xRotation = 0f;

    // Para el delay/ramp
    private float moveTimer = 0f;
    private float bobWeight = 0f;

    void Start()
    {
        Debug.Log("Juego iniciado");
        Cursor.lockState = CursorLockMode.Locked;

        // Coloca la cámara a una altura inicial (editable)
        if (cameraTransform != null && cameraPivot != null)
        {
            cameraTransform.localPosition = new Vector3(0f, cameraHeight, 0f) + extraOffset;
        }
    }

    void Update()
    {
        // Mouse look
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Este objeto (pivot) controla el pitch
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // PlayerBody controla el yaw
        playerBody.Rotate(Vector3.up * mouseX);
    }

    void LateUpdate()
    {
        if (cameraTransform == null || cameraPivot == null) return;

        // Input movimiento
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        bool isMoving = (Mathf.Abs(x) > 0.01f || Mathf.Abs(z) > 0.01f);

        // Delay + ramp
        if (isMoving)
        {
            moveTimer += Time.deltaTime;

            // Espera un poquito antes de empezar el bobbing
            if (moveTimer >= startBobDelay)
            {
                // Sube suavemente bobWeight hacia 1
                bobWeight = Mathf.MoveTowards(bobWeight, 1f, Time.deltaTime / Mathf.Max(0.001f, bobRampTime));
            }
        }
        else
        {
            moveTimer = 0f;
            // Baja suavemente bobWeight hacia 0
            bobWeight = Mathf.MoveTowards(bobWeight, 0f, Time.deltaTime / Mathf.Max(0.001f, bobRampTime));
        }

        // Balanceo Z (roll) con peso
        float balanceoZ = Mathf.Sin(Time.time * bobSpeed) * anguloBalanceo * bobWeight;

        // No forces camera.position cada frame.
        // Solo aseguramos su offset local (altura) para que puedas editarla:
        cameraTransform.localPosition = Vector3.Lerp(
            cameraTransform.localPosition,
            new Vector3(0f, cameraHeight, 0f) + extraOffset,
            Time.deltaTime * 12f
        );

        // Rotación final: pivot + roll (balanceo)
        Quaternion targetRot = cameraPivot.rotation * Quaternion.Euler(0f, 0f, balanceoZ);

        cameraTransform.rotation = Quaternion.Lerp(
            cameraTransform.rotation,
            targetRot,
            Time.deltaTime * suavizado
        );
    }
}
