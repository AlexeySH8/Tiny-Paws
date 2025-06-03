using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] List<AudioClip> _musics;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        GameManager.Instance.OnGameStart += PlayMusic;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameStart -= PlayMusic;
    }

    private void PlayMusic()
    {
        var randomMusic = _musics[Random.Range(0, _musics.Count)];
        _audioSource.clip = randomMusic;
        _audioSource.Play();
    }
}
