using UnityEngine;
using System.Collections.Generic;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    public void PlaySE(string soundName)
    {
        AudioClip clip = null;
       
        if (audioClips.TryGetValue(soundName, out clip) == false)
        {
            clip = Resources.Load<AudioClip>("Sounds/SE/" + soundName);

            audioClips.Add(soundName, clip);
        }

        audioSource.clip = clip;
        audioSource.PlayOneShot(clip);
    }

    public void PlayBGM(string soundName)
    {
        AudioClip clip = null;

        if (audioClips.TryGetValue(soundName, out clip) == false)
        {
            clip = Resources.Load<AudioClip>("Sounds/SE/" + soundName);

            audioClips.Add(soundName, clip);
        }

        audioSource.clip = clip;
        audioSource.PlayOneShot(clip);
    }

    public void SetVolume(float value)
    {
        audioSource.volume = value;
    }
}
