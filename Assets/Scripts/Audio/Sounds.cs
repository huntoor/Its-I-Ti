using UnityEngine.Audio;
using UnityEngine;
using System;
using System.ComponentModel;
using UnityEngine.Internal;

[Serializable]
public class Sounds
{
    [SerializeField] public string SoundName;

    [SerializeField] public AudioClip clip;

    [Range(0, 1)]
    [SerializeField] public float volume;

    [Range(.1f, 3f)]
    [SerializeField] public float pitch = 1;


    [SerializeField] public bool loop;

    [HideInInspector]
    public AudioSource source;
}
