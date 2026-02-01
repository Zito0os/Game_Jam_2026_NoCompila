using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BreathingAudio : MonoBehaviour
{
    [Header("References")]
    public AudioSource audioSource;
    public PlayerOxigen playerOxigen;

    [Header("Clips")]
    public AudioClip normalBreath;
    public AudioClip heavyBreath;

    [Header("Input")]
    public KeyCode runKey = KeyCode.LeftShift;

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float volumenSinMascara = 0.25f;
    [Range(0f, 1f)] public float volumenConMascara = 0.75f;
    public float velocidadCambio = 8f;

    [Header("Mask Filter (Low Pass)")]
    public AudioLowPassFilter lowPass;       // asigna o se crea solo
    public bool usarLowPass = true;

    // Cutoff en Hz: bajo = más “tapado”, alto = más “normal”
    public float cutoffConMascara = 900f;    // prueba 600–1500
    public float cutoffSinMascara = 18000f;  // casi sin filtro
    public float velocidadFiltro = 10f;

    float targetVolume;
    float targetCutoff;

    void Awake()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        if (playerOxigen == null) playerOxigen = FindObjectOfType<PlayerOxigen>();

        if (usarLowPass)
        {
            if (lowPass == null)
                lowPass = GetComponent<AudioLowPassFilter>();

            if (lowPass == null)
                lowPass = gameObject.AddComponent<AudioLowPassFilter>();

            lowPass.enabled = true;
        }
    }

    void Start()
    {
        SetNormal();
        ActualizarTargets(true);
        audioSource.volume = targetVolume;

        if (usarLowPass && lowPass != null)
            lowPass.cutoffFrequency = targetCutoff;
    }

    void Update()
    {
        // Clip por correr / no correr
        if (Input.GetKey(runKey)) SetHeavy();
        else SetNormal();

        // Targets por máscara
        ActualizarTargets(false);

        // Fade volumen
        audioSource.volume = Mathf.Lerp(audioSource.volume, targetVolume, Time.deltaTime * velocidadCambio);

        // Fade filtro
        if (usarLowPass && lowPass != null)
            lowPass.cutoffFrequency = Mathf.Lerp(lowPass.cutoffFrequency, targetCutoff, Time.deltaTime * velocidadFiltro);
    }

    void ActualizarTargets(bool instant)
    {
        bool conMascara = (playerOxigen != null && playerOxigen.esta_usando_mascara);

        targetVolume = conMascara ? volumenConMascara : volumenSinMascara;
        targetCutoff = conMascara ? cutoffConMascara : cutoffSinMascara;

        if (instant)
        {
            audioSource.volume = targetVolume;
            if (usarLowPass && lowPass != null)
                lowPass.cutoffFrequency = targetCutoff;
        }
    }

    void SetNormal()
    {
        if (audioSource.clip == normalBreath) return;
        audioSource.clip = normalBreath;
        audioSource.loop = true;
        audioSource.Play();
    }

    void SetHeavy()
    {
        if (audioSource.clip == heavyBreath) return;
        audioSource.clip = heavyBreath;
        audioSource.loop = true;
        audioSource.Play();
    }
}
