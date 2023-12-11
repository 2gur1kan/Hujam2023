using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public class SoundDataBaseController : MonoBehaviour
{
    public static SoundDataBaseController Instance;

    public SoundDataBase _soundDatabase;

    List<AudioSource> audioSourceList = new();

    private bool playing = false;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// oynayan ses var ise true dondurur
    /// </summary>
    /// <returns></returns>
    public bool IsPlaying()
    {
        foreach (var AS in audioSourceList)
        {
            if (AS.isPlaying)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Belirli bir ses oynuyorsa true doner
    /// </summary>
    /// <param name="soundName"> searching sound name</param>
    /// <returns></returns>
    public bool IsPlaying(string soundName)
    {
        AudioClip clip = _soundDatabase.soundList.Where(c => c.soundName == soundName).First().sound;
        foreach (var audioSource in audioSourceList)
        {
            if (audioSource.clip == clip && audioSource.isPlaying)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// girilen sesi oynatýr
    /// </summary>
    /// <returns></returns>
    public void PlaySound(SoundEnum Name, Action onComplete = null)

    {
        if (playing) return;

        bool flag = false;
        foreach (var audioSource in audioSourceList)
        {
            if (!audioSource.isPlaying)
            {
                PlaySound(Name, _soundDatabase.soundList, audioSource, onComplete);
                flag = true;
                return;
            }
        }
        if (!flag)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSourceList.Add(audioSource);
            PlaySound(Name, _soundDatabase.soundList, audioSource, onComplete);
        }
    }

    /// <summary>
    /// Ocelikli bir ses yoksa diðer sesleri susturur ve istenilen sesi oynatýr
    /// </summary>
    /// <returns></returns>
    public void PlayOneSound(SoundEnum sfxName, Action onComplete = null)
    {
        if (playing) return;

        StopAllSound();
        bool flag = false;
        foreach (var audioSource in audioSourceList)
        {
            if (!audioSource.isPlaying)
            {
                PlaySound(sfxName, _soundDatabase.soundList, audioSource, onComplete);
                flag = true;
                return;
            }
        }
        if (!flag)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSourceList.Add(audioSource);
            PlaySound(sfxName, _soundDatabase.soundList, audioSource, onComplete);
        }
    }

    /// <summary>
    /// Butun sesleri durdurur ve istenilen sesi oynatir
    /// </summary>
    /// <returns></returns>
    public void PlayOnlyOneSound(SoundEnum sfxName, Action callback = null)
    {
        StopAllSound();

        playing = true;

        bool flag = false;
        foreach (var audioSource in audioSourceList)
        {
            if (!audioSource.isPlaying)
            {
                PlaySound(sfxName, _soundDatabase.soundList, audioSource, () =>
                {
                    if (callback != null)
                        callback();
                    playing = false;
                });
                flag = true;
                return;
            }
        }
        if (!flag)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSourceList.Add(audioSource);
            PlaySound(sfxName, _soundDatabase.soundList, audioSource, () =>
            {
                if (callback != null)
                    callback();
                playing = false;
            });
        }
    }

    public void StopSound(SoundEnum sfxName)
    {
        StopSound(sfxName, _soundDatabase.soundList);
    }

    public void StopAllSound()
    {
        foreach (var item in audioSourceList)
        {
            item.Stop();
        }
    }

    private void PlaySound(SoundEnum soundName, List<Sound> soundList, AudioSource audioOut, Action callback = null)
    {
        AudioClip clip = soundList.Where(c => c.soundName == soundName.ToString()).First().sound;

        if (clip != null)
        {
            PlaySound(clip, audioOut, callback);
            return;
        }
        Debug.LogWarning("No sound clip found with name " + soundName);
    }

    private void StopSound(SoundEnum soundName, List<Sound> soundList)
    {
        AudioClip clip = soundList.Where(c => c.soundName == soundName.ToString()).First().sound;

        if (clip != null)
        {
            StopSound(clip);
            return;
        }
        Debug.LogWarning("No sound clip found with name " + soundName);
    }

    private void PlaySound(AudioClip clip, AudioSource audioOut, Action callback = null)
    {
        if (!audioOut.isPlaying)
        {
            StartCoroutine(IEPlaySound(clip, audioOut, callback));
        }
    }

    IEnumerator IEPlaySound(AudioClip clip, AudioSource audioOut, Action onComplete)
    {
        audioOut.clip = clip;
        audioOut.Play();

        yield return new WaitUntil(() => audioOut.time >= clip.length);

        if (onComplete != null)
            onComplete();
    }

    private void StopSound(AudioClip clip)
    {
        foreach (var audioSource in audioSourceList)
        {
            if (audioSource.clip == clip && audioSource.isPlaying)
            {
                audioSource.Stop();
                return;
            }
        }
        Debug.LogWarning("No audioSource playing clip with name " + clip.name);
    }

    public float FindTime(SoundEnum soundName)
    {
        AudioClip clip = _soundDatabase.soundList.Where(c => c.soundName == soundName.ToString()).First().sound;
        return clip.length;
    }
}
