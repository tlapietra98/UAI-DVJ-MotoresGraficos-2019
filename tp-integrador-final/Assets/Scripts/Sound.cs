using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound : MonoBehaviour {

    public string nombre;
    public AudioClip clipSonido;

    [Range(0f, 1f)]
    public float volumen;

    [Range(.1f, 3f)]
    public float pitch;

    public bool repetir;

    [HideInInspector]
    public AudioSource fuenteAudio;
    
}
