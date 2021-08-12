using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GestorDeJuego : MonoBehaviour {


	public static GestorDeJuego instancia; // singleton

	private void Awake()
	{
		if (instancia == null) {
			instancia = this;
		} else {
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);

	}
	

	public void Start()
	{
		MusicPlay ();	// cuando empiezo el juego reproduzco el tema
		
	}


	// FUNCIONES PRINCIPALES

	public static void Jugar()	// jugar implica empezar desde 0 en el 1er nivel
	{
		Debug.Log ("Se llamo a la funcion Jugar de GestorDeJuego.");

		// aca va codigo para empezar desde 0

		EliminarProgreso();
		Debug.Log("Empezamos desde 0...");
		SceneManager.LoadScene(1);	// esto carga el 1er nivel

	
		// digo al juego que me ponga la musica que corresponde al nivel 1
		GestorDeJuego.MusicPlay(1);
	}

	public static void Continuar()	// continuar implica cargar el nivel y checkpoint guardado
	{
		Debug.Log("Se llamo a la funcion Continuar de GestorDeJuego.");

		// aca va codigo para continuar desde el nivel guardado
		if (GestorDePersistencia.JugadorExiste ()) {
			int[] datos = GestorDePersistencia.CargarJugador ();

			SceneManager.LoadScene (datos [3]);	// el index 3 es el int que indica el nivel	


			// digo al juego que me ponga la musica que corresponde al nivel
			GestorDeJuego.MusicPlay(datos[3]);
		} 
		else 
		{
			Debug.Log("No hay ningun archivo para continuar, por favor seleccione Jugar para empezar...");
		}
	}


	public static void Cerrar()
	{
		Debug.Log ("Se llamo a la funcion Cerrar de GestorDeJuego.");
		Application.Quit();
	}


	public static void EliminarProgreso()
	{
		Debug.Log ("Se llamo a la funcion EliminarProgreso de GestorDeJuego.");

		GestorDePersistencia.EliminarJugador();	// esto elimina los datos guardados

	}

		
	// FUNCION DE SIGUIENTE NIVEL
	//public static void SiguienteNivel()
	//{
	//	
	//	
	//}




	// FUNCIONES MUSICA


	public static void MusicPlay()
	{
		Debug.Log ("Se llamo a la funcion MusicPlay de GestorDeJuego.");

		AllMusicStop ();

		// al empezar el juego se empieza a reproducir la musica
		GestorDeAudio.instancia.ReproducirSonido("cron_audio_8-bit_retro03");
	
	}

	public static void MusicPlay(int nivel)
	{
		Debug.Log ("Se llamo a la funcion MusicPlay de GestorDeJuego (especificacdo nivel).");

		// al empezar el nivel, se empieza a reproducir la musica

		AllMusicStop();

		switch (nivel) 
		{
		case 1:
			GestorDeAudio.instancia.ReproducirSonido("cron_audio_8-bit_modern01");
			break;

		case 2:
			GestorDeAudio.instancia.ReproducirSonido("cron_audio_8-bit_modern02");;
			break;

		case 3:
			GestorDeAudio.instancia.ReproducirSonido("cron_audio_8-bit_modern03");;
			break;
		}
	}

	public static void AllMusicStop()
	{
		Debug.Log ("Se llamo a la funcion AllMusicStop de GestorDeJuego.");

		// basta de musica que estoy por reproducir musica!

		GestorDeAudio.instancia.DetenerSonido("cron_audio_8-bit_modern01");
		GestorDeAudio.instancia.DetenerSonido("cron_audio_8-bit_modern02");
		GestorDeAudio.instancia.DetenerSonido("cron_audio_8-bit_modern03");
		GestorDeAudio.instancia.DetenerSonido("cron_audio_8-bit_retro03");
		
	}




	// FUNCIONES UI




}
