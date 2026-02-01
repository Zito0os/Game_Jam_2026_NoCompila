using UnityEngine;
using System.Collections;

public class MeleeWeapon : MonoBehaviour
{
    [Header("Damage")]
    public float damage = 25f;
    public float range = 2f;
    public float hitRadius = 0.35f;      // qué tan ancho es el golpe
    public LayerMask player;        // asigna la layer Enemy

    [Header("Timing")]
    public float attackCooldown = 0.5f;

    [Header("Animation (optional)")]
    public Animator animator;
    public string attackTrigger = "Attack";

    [Header("Fake Swing (if no Animator)")]
    public bool useFakeSwingIfNoAnimator = true;
    public float swingDuration = 0.12f;
    public Vector3 swingRotation = new Vector3(-35f, 0f, 0f); // rotación del golpe (local)
    private Quaternion startRot;
    private bool isSwinging = false;

    [Header("Ray Origin (recommended: FirstPersonCamera)")]
    public Transform rayOrigin;          // arrastra tu FirstPersonCamera aquí

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip swingSound;         // sonido de "woosh"
    public AudioClip hitSound;           // sonido de impacto
    public Vector2 pitchRange = new Vector2(0.92f, 1.08f);

    private float nextAttackTime = 0f;

    void Start()
    {
        startRot = transform.localRotation;

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (rayOrigin == null)
        {
            Camera cam = GetComponentInParent<Camera>();
            if (cam != null) rayOrigin = cam.transform;
            else rayOrigin = transform;
        }
    }

    void Update()
    {
        if (Time.timeScale == 0) return;

        if (Input.GetButtonDown("Fire1") && Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown;

            // 1) Swing sound (siempre)
            PlayOneShotVaried(swingSound);

            // 2) Animación
            if (animator != null && !string.IsNullOrEmpty(attackTrigger))
            {
                animator.SetTrigger(attackTrigger);
            }
            else if (useFakeSwingIfNoAnimator && !isSwinging)
            {
                StartCoroutine(FakeSwing());
            }

            // 3) Daño
            DoMeleeHit();
        }
    }

    void DoMeleeHit()
    {
        Vector3 origin = rayOrigin.position;
        Vector3 forward = rayOrigin.forward;

        // OverlapSphere al frente (más generoso que solo raycast)
        Vector3 center = origin + forward * (range * 0.75f);
        Collider[] hits = Physics.OverlapSphere(center, hitRadius, player, QueryTriggerInteraction.Ignore);

        if (hits.Length == 0)
        {
            // fallback: raycast por si overlap no tocó nada
            if (Physics.Raycast(origin, forward, out RaycastHit hit, range, player, QueryTriggerInteraction.Ignore))
            {
                TryDamage(hit.collider);
            }
            return;
        }

        // Pega al más cercano
        Collider best = hits[0];
        float bestDist = (best.transform.position - origin).sqrMagnitude;

        for (int i = 1; i < hits.Length; i++)
        {
            float d = (hits[i].transform.position - origin).sqrMagnitude;
            if (d < bestDist)
            {
                best = hits[i];
                bestDist = d;
            }
        }

        TryDamage(best);
    }

    void TryDamage(Collider col)
    {
        // Tu enemigo usa el script AI con LooseLife()
        AI ai = col.GetComponentInParent<AI>();
        if (ai != null)
        {
            ai.LooseLife(damage);

            // Hit sound SOLO si realmente pegó a un enemigo
            PlayOneShotVaried(hitSound);
        }
    }

    void PlayOneShotVaried(AudioClip clip)
    {
        if (audioSource == null || clip == null) return;

        audioSource.pitch = Random.Range(pitchRange.x, pitchRange.y);
        audioSource.PlayOneShot(clip);
    }

    IEnumerator FakeSwing()
    {
        isSwinging = true;

        Quaternion endRot = startRot * Quaternion.Euler(swingRotation);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / swingDuration;
            transform.localRotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }

        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / swingDuration;
            transform.localRotation = Quaternion.Slerp(endRot, startRot, t);
            yield return null;
        }

        transform.localRotation = startRot;
        isSwinging = false;
    }

    void OnDrawGizmosSelected()
    {
        if (rayOrigin == null) return;

        Gizmos.color = Color.yellow;
        Vector3 center = rayOrigin.position + rayOrigin.forward * (range * 0.75f);
        Gizmos.DrawWireSphere(center, hitRadius);
    }
}