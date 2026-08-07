using UnityEngine;
using System.Collections.Generic;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] AudioSource sourceSE;
    [SerializeField] AudioSource sourceBGM;
    [SerializeField] Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    public void PlaySE(string soundName)
    {
        AudioClip clip = null;
       
        if (audioClips.TryGetValue(soundName, out clip) == false)
        {
            clip = Resources.Load<AudioClip>("Sounds/SE/" + soundName);

            audioClips.Add(soundName, clip);
        }

        sourceSE.clip = clip;
        sourceSE.PlayOneShot(clip);
    }

    public void PlayBGM(string soundName)
    {
        AudioClip clip = null;

        if (audioClips.TryGetValue(soundName, out clip) == false)
        {
            clip = Resources.Load<AudioClip>("Sounds/BGM/" + soundName);

            audioClips.Add(soundName, clip);
        }

        sourceBGM.clip = clip;
        sourceBGM.loop = true;
        sourceBGM.Play();
    }

    public void StopBGM()
    {
        sourceBGM.Stop();
    }

    public void SetVolume(float value)
    {
        sourceSE.volume = value;
        sourceBGM.volume = value;
    }
}
