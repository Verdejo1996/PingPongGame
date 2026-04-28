using System.Collections;
using UnityEngine;

public class BallVisualEffects : MonoBehaviour
{
    [Header("Trail Renderer")]
    [SerializeField] private TrailRenderer trailBall;
    [SerializeField] private Color colorSoft = Color.blue;
    [SerializeField] private Color colorStrong = Color.red;

    [Header("Camera Shake")]
    [SerializeField] private Camera_Shake cameraShake;

    [Header("Particles")]
    [SerializeField] private ParticleSystem effectParticles;

    public void ChangeColorTrail(float force)
    {
        if (trailBall == null)
            return;

        if (force < 10)
        {
            trailBall.material.color = colorSoft;
        }
        else
        {
            trailBall.material.color = colorStrong;

            if (cameraShake != null)
            {
                StartCoroutine(cameraShake.Shake(0.2f, 0.1f));
            }
        }
    }

    public void ActiveEffectControl()
    {
        if (effectParticles != null)
        {
            effectParticles.Play();
            return;
        }

        ParticleSystem ps = GetComponentInChildren<ParticleSystem>();

        if (ps != null)
            ps.Play();
    }

    public void DeactivateEffectControl()
    {
        if (effectParticles != null)
        {
            effectParticles.Stop();
            return;
        }

        ParticleSystem ps = GetComponentInChildren<ParticleSystem>();

        if (ps != null)
            ps.Stop();
    }
}