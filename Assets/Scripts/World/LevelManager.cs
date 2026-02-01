using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    // Enumeración de fases del juego
    public enum GamePhase
    {
        FASE_0_PRIMER_NOTA,
        FASE_1_SONIDO,
        FASE_2_PRIMERA_APARICION,
        FASE_3_PRIMER_SPAWN,
        FASE_4_ARMA,
        FASE_5_BATE_BEISBOL,
        FASE_6_MATANZA_FINAL
    }

    // Estado actual del juego
    private GamePhase currentPhase = GamePhase.FASE_0_PRIMER_NOTA;
    public GamePhase CurrentPhase => currentPhase;

    // Diccionario de descripciones de fases para los mensajes en pantalla
    private string[] phaseMessages = new string[]
    {
        "Ve a recoger la primera nota...",
        "Ve a escuchar el sonido...",
        "Ve a la primera aparición...",
        "Entra al spawn...",
        "Ve a buscar el bate de béisbol...",
        "Ve a completar la matanza final...",
        "Ve a buscar el arma..."
    };

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        Debug.Log($"LevelManager iniciado. Fase actual: {currentPhase}");
    }

    private void Update()
    {
        // Aquí puedes agregar lógica de eventos del juego si es necesario
    }

    /// <summary>
    /// Verifica si una fase específica está desbloqueada
    /// </summary>
    public bool IsPhaseUnlocked(GamePhase phase)
    {
        return phase == currentPhase;
    }

    /// <summary>
    /// Completa la fase actual y desbloquea la siguiente
    /// </summary>
    public void CompletePhase()
    {
        //if (currentPhase == GamePhase.FASE_COMPLETADA)
        //{
        //    Debug.LogWarning("El juego ya está completado");
        //    return;
        //}

        // Avanzar a la siguiente fase
        currentPhase++;
        Debug.Log($"Fase completada. Siguiente fase: {currentPhase} - {GetPhaseMessage()}");

        // Mostrar mensaje en pantalla (será implementado)
        OnPhaseChanged();
    }

    /// <summary>
    /// Obtiene el mensaje de la fase actual para mostrar en pantalla
    /// </summary>
    public string GetPhaseMessage()
    {
        if ((int)currentPhase < phaseMessages.Length)
        {
            return phaseMessages[(int)currentPhase];
        }
        return "Estado desconocido";
    }

    /// <summary>
    /// Obtiene el nombre descriptivo de la fase
    /// </summary>
    public string GetPhaseName(GamePhase phase)
    {
        return phase.ToString();
    }

    /// <summary>
    /// Evento que se dispara cuando cambia la fase (para implementar UI)
    /// </summary>
    private void OnPhaseChanged()
    {
        // Aquí implementarás la lógica para mostrar el mensaje en pantalla
        Debug.Log($"[UI] Mostrar mensaje: {GetPhaseMessage()}");

        // Ejemplo de cómo usarlo desde otro script:
        // MostrarMensajeFase(LevelManager.Instance.GetPhaseMessage());
    }

    /// <summary>
    /// Reinicia el juego a la fase inicial
    /// </summary>
    public void ResetLevel()
    {
        currentPhase = GamePhase.FASE_0_PRIMER_NOTA;
        Debug.Log("LevelManager reiniciado. Fase actual: " + currentPhase);
    }

    /// <summary>
    /// Obtiene toda la información de debug sobre el estado actual
    /// </summary>
    public string GetDebugInfo()
    {
        return $"Fase Actual: {currentPhase}\nMensaje: {GetPhaseMessage()}";
    }
}
