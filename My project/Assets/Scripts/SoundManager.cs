using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;

    public static SoundManager Instance => _instance;

    private Dictionary<string, AudioClip> _sounds = new();

    private float _volume = 1f;

    public float Volume { get => _volume; set => _volume = Mathf.Clamp(value, 0f, 1f); }

    public void Awake()
    {
        _instance = this;

        GetSounds();
    }
    public void PlaySound(string pSound, float pPitch = 1f)
    {
        if (!_sounds.ContainsKey(pSound))
            throw new System.Exception("Sound not found in dictionary");

        AudioSource audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.volume = _volume;
        audioSource.pitch = Mathf.Clamp(pPitch, 0.01f, 3f);
        audioSource.clip = _sounds[pSound];
        audioSource.Play();

        StartCoroutine(RemoveSoundOnceDone(audioSource, audioSource.clip.length));
    }
    private void GetSounds()
    {
        AudioClip[] audioArray = Resources.LoadAll<AudioClip>("Audio");

        foreach (AudioClip audioClip in audioArray)
        {
            _sounds.Add(audioClip.name, audioClip);
        }
    }
    private IEnumerator RemoveSoundOnceDone(AudioSource pAudioSource, float pTime)
    {
        yield return new WaitForSeconds(pTime);

        Destroy(pAudioSource);
    }
}
