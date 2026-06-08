using System.Collections;
using UnityEngine;

public class WaterShieldEffect : MonoBehaviour
{
    public ParticleSystem generateEffect;
    public ParticleSystem sustainEffect;
    public ParticleSystem disappearEffect;
    public ParticleSystem hitEffect;
    public float generateDelay = 1.5f;
    public float destroyDelay = 1f;

    public void ShowShield()
    {
        StopAllCoroutines();
        StartCoroutine(ShowShieldRoutine());
    }

    private IEnumerator ShowShieldRoutine()
    {
        generateEffect.Play();
        yield return new WaitForSeconds(generateDelay);
        sustainEffect.Play();
    }
    
    public void OnHit()
    {
        hitEffect.Play();
    }

    public void HideShield()
    {
        StopAllCoroutines();
        StartCoroutine(HideShieldRoutine());
    }

    private IEnumerator HideShieldRoutine()
    {
        sustainEffect.Stop();
        disappearEffect.Play();
        float waitTime = disappearEffect.main.duration + destroyDelay;
        yield return new WaitForSeconds(waitTime);

        // 停止并清除所有特效，保留对象以供后续重播
        generateEffect.Stop();
        generateEffect.Clear();
        sustainEffect.Stop();
        sustainEffect.Clear();
        disappearEffect.Stop();
        disappearEffect.Clear();
        hitEffect.Stop();
        hitEffect.Clear();
    }
}
