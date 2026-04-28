using System.Collections;
using UnityEngine;

public class BallPowerEffects : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private TrailRenderer trail;
    [SerializeField] private ParticleSystem explosionParticles;

    [Header("Curve Shot")]
    [SerializeField] private float curveDuration = 3f;
    public bool isCurveShotActive = false;

    [Header("Fire Explosion")]
    private bool fireExplosionEnabled = false;
    private Color originalColor;
    private Color explosionColor;

    [Header("Heavy Ball")]
    public bool isHeavy = false;

    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();

        if (trail == null)
            trail = GetComponent<TrailRenderer>();
    }

    public void EnableFireExplosion(Color color)
    {
        fireExplosionEnabled = true;
        explosionColor = color;

        if (trail != null)
        {
            originalColor = trail.material.color;
            trail.material.color = explosionColor;
        }
    }

    public IEnumerator CurveShot(float force)
    {
        Vector3 curve = new(0.17f, 0f, 0f);

        if (isCurveShotActive)
        {
            rb.AddForce(curve * force, ForceMode.Impulse);

            yield return new WaitForSeconds(curveDuration);

            isCurveShotActive = false;
        }
    }

    public void TryApplyFireExplosion(Collider other)
    {
        if (!fireExplosionEnabled)
            return;

        if (!other.CompareTag("Bot"))
            return;

        if (other.TryGetComponent<IA_Controller>(out var ia))
        {
            ia.ApplyDisorientation(2f);
            fireExplosionEnabled = false;

            if (trail != null)
                trail.material.color = originalColor;

            if (explosionParticles != null)
                explosionParticles.Play();
        }
    }
}