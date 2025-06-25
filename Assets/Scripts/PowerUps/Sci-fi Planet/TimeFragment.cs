using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Time Fragment")]
public class TimeFragment : Base_PowerUp
{
    public float slowDuration = 1f;
    public float slowTimeScale = 0.3f;
    public override void Activate(Player_Controller player)
    {
        player.StartCoroutine(ApplyTimeFragmentEffect());
    }

    private IEnumerator ApplyTimeFragmentEffect()
    {
        Time.timeScale = slowTimeScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        yield return new WaitForSecondsRealtime(slowDuration);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }
}
