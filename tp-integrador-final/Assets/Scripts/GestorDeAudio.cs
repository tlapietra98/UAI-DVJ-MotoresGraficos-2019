using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GestorDeAudio : MonoBehaviour {

    public Sound[] sonidos;
    public static GestorDeAudio instancia; // singleton

    private void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sonidos)
        {
            s.fuenteAudio = gameObject.AddComponent<AudioSource>();
            s.fuenteAudio.clip = s.clipSonido;
            s.fuenteAudio.volume = s.volumen;
            s.fuenteAudio.pitch = s.pitch;
            s.fuenteAudio.loop = s.repetir;
        }
    }

    public void ReproducirSonido(string nombre)
    {
        Debug.Log("Pido que me ejecute un sonido...");

        Sound s = Array.Find(sonidos, sound => sound.nombre == nombre);

		if (s == null) 
		{
			Debug.LogWarning ("Sonido: " + nombre + " no encontrado!");
			return;
		} 
		else 
		{
			Debug.Log ("Sonido: " + nombre + " reproducido.");
		}

        s.fuenteAudio.Play();

    }

	public void DetenerSonido(string nombre)
	{
		Debug.Log("Pido que detenga un sonido...");

		Sound s = Array.Find(sonidos, sound => sound.nombre == nombre);

		if (s == null) 
		{
			Debug.LogWarning ("Sonido: " + nombre + " no encontrado!");
			return;
		} 
		else 
		{
			Debug.Log ("Sonido " + nombre + " detenido.");
		}

		s.fuenteAudio.Stop();

	}



}
