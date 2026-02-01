using UnityEngine;

/// <summary>
/// Script genérico para triggers que verifican el progreso del juego.
/// Asigna este script a cada trigger y especifica qué fase desbloquea.
/// </summary>
public class TriggerProgresion : MonoBehaviour
{
    [Header("Configuración de Fase")]
    [SerializeField] private LevelManager.GamePhase requiredPhase;
    [SerializeField] private bool completePhaseOnTrigger = true;

    private bool hasBeenTriggered = false;
    private Collider triggerCollider;

    private void Start()
    {
        triggerCollider = GetComponent<Collider>();

        if (triggerCollider == null)
        {
            Debug.LogError("TriggerProgresion necesita un Collider con IsTrigger activado en: " + gameObject.name);
        }

        // Inicialmente el trigger está bloqueado
        UpdateTriggerState();

        Debug.Log($"Trigger '{gameObject.name}' configurado para fase: {requiredPhase}");
    }

    private void Update()
    {
        // Actualizar estado del trigger si cambió la fase
        UpdateTriggerState();
    }

    /// <summary>
    /// Actualiza si el trigger debe estar activo o bloqueado
    /// </summary>
    private void UpdateTriggerState()
    {
        if (LevelManager.Instance == null)
        {
            Debug.LogError("LevelManager no encontrado");
            return;
        }

        bool isUnlocked = LevelManager.Instance.IsPhaseUnlocked(requiredPhase);

        if (triggerCollider != null)
        {
            triggerCollider.enabled = isUnlocked;
        }

        // Cambiar color visual si está disponible
        VisualFeedback(isUnlocked);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        // Verificar si esta fase está desbloqueada
        if (!LevelManager.Instance.IsPhaseUnlocked(requiredPhase))
        {
            Debug.LogWarning($"Trigger '{gameObject.name}' no está desbloqueado aún. Fase requerida: {requiredPhase}");
            return;
        }

        if (hasBeenTriggered)
        {
            Debug.Log($"Trigger '{gameObject.name}' ya fue activado");
            return;
        }

        hasBeenTriggered = true;
        Debug.Log($"✓ Trigger '{gameObject.name}' activado correctamente");

        // Completar la fase si está configurado
        if (completePhaseOnTrigger)
        {
            LevelManager.Instance.CompletePhase();
        }

        // Desactivar el collider después de ser usado
        if (triggerCollider != null)
        {
            triggerCollider.enabled = false;
        }

        gameObject.SetActive(false);
    }

    /// <summary>
    /// Proporciona feedback visual sobre si el trigger está desbloqueado
    /// </summary>
    private void VisualFeedback(bool isUnlocked)
    {
        // Puedes cambiar el color del objeto según si está desbloqueado
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            if (isUnlocked)
            {
                // Verde si está desbloqueado
                renderer.material.color = new Color(0, 1, 0, 0.3f);
            }
            else
            {
                // Rojo si está bloqueado
                renderer.material.color = new Color(1, 0, 0, 0.3f);
            }
        }
    }

    /// <summary>
    /// Método para forzar completar esta fase desde otro script si es necesario
    /// </summary>
    public void ForceTrigger()
    {
        if (!hasBeenTriggered)
        {
            hasBeenTriggered = true;
            if (completePhaseOnTrigger)
            {
                LevelManager.Instance.CompletePhase();
            }
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Reinicia el trigger para poder usarlo de nuevo
    /// </summary>
    public void ResetTrigger()
    {
        hasBeenTriggered = false;
        gameObject.SetActive(true);
        UpdateTriggerState();
    }
}
