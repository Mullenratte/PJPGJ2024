using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    // Audio Files to play
    [SerializeField]
    private AudioClip _music;

    [SerializeField, Range(0.0f, 1.0f)] 
    private float _volume = 0.3f;
    
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.loop = true;
    }

    private void Start()
    {
        _audioSource.clip = _music;
        _audioSource.Play();
    }

    private void Update()
    {
        _audioSource.volume = _volume;
    }
}
