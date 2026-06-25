using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    private static GameObject _root;
    private static AudioSource _bgmSource;
    private static readonly List<AudioSource> _sfxPool = new();
    private static readonly Dictionary<string, AudioClip> _cache = new();
    private const int MaxSfx = 8;

    private static bool _isMuted;
    private static bool _bgmMuted;
    private static bool _sfxMuted;
    private static float _bgmVolume = 1f;

    public static bool IsMuted
    {
        get => _isMuted;
        set
        {
            _isMuted = value;
            AudioListener.pause = value;
            PlayerPrefs.SetInt("SoundMuted", value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public static bool IsBgmMuted
    {
        get => _bgmMuted;
        set
        {
            _bgmMuted = value;
            if (_bgmSource != null) _bgmSource.mute = value;
            PlayerPrefs.SetInt("BgmMuted", value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public static bool IsSfxMuted
    {
        get => _sfxMuted;
        set
        {
            _sfxMuted = value;
            foreach (var src in _sfxPool)
                src.mute = value;
            PlayerPrefs.SetInt("SfxMuted", value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public static float BGMVolume
    {
        get => _bgmVolume;
        set
        {
            _bgmVolume = Mathf.Clamp01(value);
            if (_bgmSource != null) _bgmSource.volume = _bgmVolume;
            PlayerPrefs.SetFloat("BGMVolume", _bgmVolume);
            PlayerPrefs.Save();
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Init()
    {
        _isMuted = PlayerPrefs.GetInt("SoundMuted", 0) == 1;
        AudioListener.pause = _isMuted;

        _bgmMuted = PlayerPrefs.GetInt("BgmMuted", 0) == 1;
        _sfxMuted = PlayerPrefs.GetInt("SfxMuted", 0) == 1;
        _bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 0.5f);

        // 只创建一次根节点和 AudioSource 池
        _root = new GameObject("[SoundManager]");
        Object.DontDestroyOnLoad(_root);

        _bgmSource = _root.AddComponent<AudioSource>();
        _bgmSource.loop = true;
        _bgmSource.playOnAwake = false;
        _bgmSource.spatialBlend = 0f;
        _bgmSource.volume = _bgmVolume;
        _bgmSource.mute = _bgmMuted;

        for (int i = 0; i < MaxSfx; i++)
        {
            var src = _root.AddComponent<AudioSource>();
            src.playOnAwake = false;
            src.spatialBlend = 0f;
            src.mute = _sfxMuted;
            _sfxPool.Add(src);
        }
    }

    // ── 音效（一次播放） ──────────────────────────────────

    public static void Play(string clipPath, float volume = 1f)
    {
        var clip = GetClip(clipPath);
        if (clip == null) return;

        var src = GetFreeSfxSource();
        src?.PlayOneShot(clip, volume);
    }

    /// <summary>循环播放音效（如燃烧声），返回 AudioSource 以便停止</summary>
    public static AudioSource PlayLoopingSfx(string clipPath, float volume = 1f)
    {
        var clip = GetClip(clipPath);
        if (clip == null) return null;

        var src = GetFreeSfxSource();
        if (src == null) return null;

        src.clip = clip;
        src.loop = true;
        src.volume = volume;
        src.Play();
        return src;
    }

    // ── 背景音乐（循环） ──────────────────────────────────

    public static void PlayBGM(string clipPath)
    {
        var clip = GetClip(clipPath);
        if (clip == null) return;

        if (_bgmSource.isPlaying && _bgmSource.clip == clip) return; // 正在播同一首

        _bgmSource.clip = clip;
        _bgmSource.Play();
    }

    public static void StopBGM()
    {
        _bgmSource.Stop();
        _bgmSource.clip = null;
    }

    public static void ToggleMute()
    {
        IsMuted = !IsMuted;
    }

    public static void ToggleBgmMute()
    {
        IsBgmMuted = !IsBgmMuted;
    }

    public static void ToggleSfxMute()
    {
        IsSfxMuted = !IsSfxMuted;
    }

    // ── 工具方法 ──────────────────────────────────────────

    private static AudioClip GetClip(string path)
    {
        if (string.IsNullOrEmpty(path)) return null;
        if (_cache.TryGetValue(path, out var clip)) return clip;

        clip = Resources.Load<AudioClip>(path);
        if (clip != null) _cache[path] = clip;
        return clip;
    }

    private static AudioSource GetFreeSfxSource()
    {
        foreach (var src in _sfxPool)
        {
            if (!src.isPlaying) return src;
        }
        // 所有 AudioSource 都在播放中 → 覆盖第一个
        return _sfxPool[0];
    }
}
