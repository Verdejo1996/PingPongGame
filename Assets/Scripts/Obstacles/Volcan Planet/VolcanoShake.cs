using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolcanoShake : MonoBehaviour
{
    public float interval = 10f;
    public float shakeDuration = 1f;
    public float shakeIntensity = 0.3f;
    public Camera mainCamera;
    public PlayerHit_Controller player;
    //public Transform cameraTarget;

    private Vector3 originalCamPos;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        originalCamPos = mainCamera.transform.position;
        StartCoroutine(QuakeRoutine());
    }

    IEnumerator QuakeRoutine()
    {
        yield return new WaitForSeconds(interval);

        while (true)
        {
            StartCoroutine(ScreenShake());
            if (!player.isMoving) // Si el jugador no se mueve
                player.ApplyShakeForce(); // Lo empujamos

            yield return new WaitForSeconds(shakeDuration);

            player.StopMovement();

            yield return new WaitForSeconds(interval - shakeDuration);
        }
    }

    IEnumerator ScreenShake()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            Vector3 offset = Random.insideUnitSphere * shakeIntensity;
            offset.z = 0; // no mover en Z si usás cámara lateral

            mainCamera.transform.position = originalCamPos + offset;
            elapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = originalCamPos;
    }
}
