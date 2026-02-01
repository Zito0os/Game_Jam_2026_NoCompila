using UnityEngine;

public class FootstepAudio : MonoBehaviour
{
    [Header("References")]
    public CharacterController controller;
    public AudioSource audioSource;

    [Header("Footstep Clips")]
    public AudioClip walkLoop;
    public AudioClip runLoop;

    [Header("Run Settings")]
    public KeyCode runKey = KeyCode.LeftShift;

    [Header("Movement")]
    public float moveThreshold = 0.2f;

    void Start()
    {
        if (controller == null)
            controller = GetComponent<CharacterController>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Time.timeScale == 0) return;
        if (!controller.isGrounded) StopSteps();

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        float move = new Vector2(h, v).magnitude;

        if (move < moveThreshold)
        {
            StopSteps();
            return;
        }

        bool isRunning = Input.GetKey(runKey);
        PlaySteps(isRunning);
    }

    void PlaySteps(bool running)
    {
        AudioClip targetClip = running ? runLoop : walkLoop;

        if (audioSource.clip == targetClip && audioSource.isPlaying)
            return; // ya está sonando el correcto

        audioSource.clip = targetClip;
        audioSource.Play();
    }

    void StopSteps()
    {
        if (audioSource.isPlaying)
            audioSource.Stop();
    }
}
