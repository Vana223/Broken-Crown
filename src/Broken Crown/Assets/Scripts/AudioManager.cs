using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
    }

    public List<Sound> sounds;

    private Dictionary<string, Sound> soundDict;
    private AudioSource sfxSource;
    private AudioSource musicSource;
    private AudioSource walkSource;
    private AudioSource runSource;

    private float masterVolume = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        sfxSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.playOnAwake = false;

        walkSource = gameObject.AddComponent<AudioSource>();
        runSource = gameObject.AddComponent<AudioSource>();
        walkSource.loop = true;
        runSource.loop = true;
        walkSource.playOnAwake = false;
        runSource.playOnAwake = false;

        soundDict = new Dictionary<string, Sound>();
        foreach (var sound in sounds)
        {
            if (!soundDict.ContainsKey(sound.name))
                soundDict.Add(sound.name, sound);
        }

        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);

        if (soundDict.TryGetValue("Walk", out Sound walkSound))
        {
            walkSource.clip = walkSound.clip;
            walkSource.volume = walkSound.volume * masterVolume;
        }

        if (soundDict.TryGetValue("Run", out Sound runSound))
        {
            runSource.clip = runSound.clip;
            runSource.volume = runSound.volume * masterVolume;
        }

        musicSource.volume = masterVolume;
        walkSource.volume *= masterVolume;
        runSource.volume *= masterVolume;
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
        PlayerPrefs.SetFloat("MasterVolume", volume);

        musicSource.volume = volume;

        if (soundDict.TryGetValue("Walk", out Sound walkSound))
            walkSource.volume = walkSound.volume * masterVolume;

        if (soundDict.TryGetValue("Run", out Sound runSound))
            runSource.volume = runSound.volume * masterVolume;
    }

    public void Play(string soundName)
    {
        if (soundDict.TryGetValue(soundName, out Sound sound))
        {
            sfxSource.PlayOneShot(sound.clip, sound.volume * masterVolume);
        }
        else
        {
            Debug.LogWarning("SFX not found: " + soundName);
        }
    }

    public void PlayMusic(string musicName)
    {
        if (soundDict.TryGetValue(musicName, out Sound sound))
        {
            StartCoroutine(SwitchMusic(sound.clip, sound.volume * masterVolume));
        }
        else
        {
            Debug.LogWarning("Music not found: " + musicName);
        }
    }

    private IEnumerator SwitchMusic(AudioClip newClip, float targetVolume)
    {
        float t = 0f;
        float initialVolume = musicSource.volume;

        while (t < 1f)
        {
            musicSource.volume = Mathf.Lerp(initialVolume, 0f, t);
            t += Time.deltaTime;
            yield return null;
        }

        musicSource.Stop();
        musicSource.clip = newClip;
        musicSource.volume = 0f;
        musicSource.Play();

        t = 0f;
        while (t < 1f)
        {
            musicSource.volume = Mathf.Lerp(0f, targetVolume, t);
            t += Time.deltaTime;
            yield return null;
        }

        musicSource.volume = targetVolume;
    }

    public void FadeOutMusic(float duration = 1f)
    {
        StartCoroutine(FadeOutMusicCoroutine(duration));
    }

    private IEnumerator FadeOutMusicCoroutine(float duration)
    {
        float startVolume = musicSource.volume;
        float t = 0f;

        while (t < duration)
        {
            musicSource.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        musicSource.volume = 0f;
        musicSource.Stop();
    }

    public void PlayWalkLoop()
    {
        if (!walkSource.isPlaying)
        {
            walkSource.Play();
        }
    }

    public void StopWalkLoop()
    {
        if (walkSource.isPlaying)
        {
            walkSource.Stop();
        }
    }

    public void PlayRunLoop()
    {
        if (!runSource.isPlaying)
        {
            runSource.Play();
        }
    }

    public void StopRunLoop()
    {
        if (runSource.isPlaying)
        {
            runSource.Stop();
        }
    }
}
