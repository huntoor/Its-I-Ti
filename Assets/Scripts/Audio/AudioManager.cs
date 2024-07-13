using UnityEngine;
using System;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Sounds[] sounds;

    public static AudioManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        
        foreach (Sounds s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();

            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;

            s.source.loop = s.loop;
        }
    }

    public void PlaySound(string soundName)
    {
        Sounds s = Array.Find(sounds, sound => sound.SoundName == soundName);

        if (s == null)
        {
            Debug.LogWarning("Sound Name: " + soundName + " Not Found!!");
        }
        if (!s.source.isPlaying)
        {
            s.source.Play();
        }
    }

    public void PlaySound(string soundName, string soundToStop)
    {
        Sounds soundPlay = Array.Find(sounds, sound => sound.SoundName == soundName);
        Sounds soundStop = Array.Find(sounds, sound => sound.SoundName == soundToStop);

        if (soundPlay == null || soundStop == null)
        {
            Debug.LogWarning("Sound Name: " + soundName + " Not Found!!");
        }

        if (soundStop != null)
        {
            soundStop.source.Stop();
        }
        
        if (!soundPlay.source.isPlaying)
        {
            soundPlay.source.Play();
        }
    }

    public IEnumerator PlaySoundNext(string nextSoundName, string prevSoundName)
    {
        Sounds nextSoundToPlay = Array.Find(sounds, sound => sound.SoundName == nextSoundName);
        Sounds prevSound = Array.Find(sounds, sound => sound.SoundName == prevSoundName);

        var waitForRemaningTime = new WaitForSeconds(GetClipRemainingTime(prevSound.source));
        yield return waitForRemaningTime;

        nextSoundToPlay.source.Play();
    }

    public IEnumerator PlaySoundNext(AudioClip firstSoundClipName, bool isFirstClipLooping, AudioClip secondSoundClipName, bool isSecondClipLooping, AudioSource audioSource)
    {
        audioSource.clip = firstSoundClipName;
        audioSource.loop = isFirstClipLooping;
        audioSource.Play();

        var waitForRemaningTime = new WaitForSeconds(GetClipRemainingTime(audioSource));
        yield return waitForRemaningTime;

        audioSource.clip = secondSoundClipName;
        audioSource.loop = isSecondClipLooping;
        audioSource.Play();
    }

    private float GetClipRemainingTime(AudioSource source)
    {
         float remainingTime = (source.clip.length - source.time) / source.pitch;
        return IsReversePitch(source) ?
            (source.clip.length + remainingTime) :
            remainingTime;
    }

    private bool IsReversePitch(AudioSource source) {
        return source.pitch < 0f;
    }
}
