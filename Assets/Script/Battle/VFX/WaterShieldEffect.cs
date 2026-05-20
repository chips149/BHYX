using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterShieldEffect : MonoBehaviour
{
    public ParticleSystem generateEffect;
    public ParticleSystem sustainEffect;
    public ParticleSystem disappearEffect;

    public void ShowShield()
    {
        generateEffect?.Play();
        sustainEffect?.Play();
    }

    public void HideShield()
    {
        sustainEffect?.Stop();
        disappearEffect?.Play();
    }
}
