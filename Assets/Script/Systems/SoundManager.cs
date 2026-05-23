using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    private static AudioSource audioSource;
    private static AudioSource loopSource;
    private static readonly Dictionary<string, AudioClip> _cache = new();

    private static AudioSource GetSource()
    {
        var go = new GameObject("SoundManager");
        Object.DontDestroyOnLoad(go);
        audioSource = go.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
        return audioSource;
    }

    public static void Play(string clipPath, float volume = 1f)
    {
        if (!_cache.TryGetValue(clipPath, out var clip))
        {
            clip = Resources.Load<AudioClip>(clipPath);
            _cache[clipPath] = clip;
        }
        GetSource().PlayOneShot(clip, volume);
    }

    private static AudioSource GetLoopSource()
    {
        var go = new GameObject("SoundManager_Loop");
        Object.DontDestroyOnLoad(go);
        loopSource = go.AddComponent<AudioSource>();
        loopSource.playOnAwake = false;
        loopSource.spatialBlend = 0f;
        return loopSource;
    }

    public static void PlayLoop(string clipPath, float volume = 1f)
    {
        if (!_cache.TryGetValue(clipPath, out var clip))
        {
            clip = Resources.Load<AudioClip>(clipPath);
            _cache[clipPath] = clip;
        }
        var source = GetLoopSource();
        source.clip = clip;
        source.loop = true;
        source.volume = volume;
        source.Play();
    }

    public static void StopLoop()
    {
        var source = GetLoopSource();
        source.loop = false;
        source.Stop();
    }
}