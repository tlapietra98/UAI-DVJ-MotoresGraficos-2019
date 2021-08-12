using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GestorDePersistencia : MonoBehaviour {

    public static GestorDePersistencia instancia; // singleton

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

		// ^esto hace que solo haya siempre una instancia del Gestor

        DontDestroyOnLoad(gameObject);
    }


	// GUARDO LOS DATOS DEL JUGADOR
        public static void GuardarJugador(ControlJugador jugador)
    {
		Debug.Log ("Se llamo a la funcion GuardarJugador de GestorDePersistencia.");

        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/jugador.sav", FileMode.Create);
        JugadorData data = new JugadorData(jugador);
        Debug.Log(Application.persistentDataPath);
        bf.Serialize(stream, data);
        stream.Close();
    }

	public static bool JugadorExiste()
	{

		if (File.Exists(Application.persistentDataPath + "/jugador.sav"))
		{
			return true;	// devuelve true para indicar que hay un archivo guardado
		}
		else
		{
			return false;
		}
	}

	// CARGO LOS DATOS DEL JUGADOR
    public static int[] CargarJugador()
    {
		Debug.Log ("Se llamo a la funcion CargarJugador de GestorDePersistencia.");


        if (File.Exists(Application.persistentDataPath + "/jugador.sav"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + "/jugador.sav", FileMode.Open);
            JugadorData data = bf.Deserialize(stream) as JugadorData;
            stream.Close();
            return data.datos;	// devuelve el int array datos con la info guardada
        }
        else
        {
            int[] defaultState = new int[5];	// el defaultState seria con lo que empieza el jugador cuando le da a New Game
            defaultState[0] = 0;	// checkpoint
            defaultState[1] = 3;	// vidas
			defaultState[2] = 0;	// puntuacion igual a cubos recogidos (cantidad total)
			defaultState[3] = 1;	// nivel

            return defaultState;	//devuelve el int array con los datos default cuando se empieza de 0 ya que no hay datos guardados o se borraron
        }
    }

	// CARGO LOS DATOS DE LOS CUBOS COLECTADOS POR EL JUGADOR
	public static bool[] CargarCubos()
	{
		Debug.Log ("Se llamo a la funcion CargarCubos de GestorDePersistencia.");

		if (File.Exists(Application.persistentDataPath + "/jugador.sav"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream stream = new FileStream(Application.persistentDataPath + "/jugador.sav", FileMode.Open);
			JugadorData data = bf.Deserialize(stream) as JugadorData;
			stream.Close();
			return data.cubosDatos;	// devuelve el bool array cubosDatos con la info guardada
		}
		else
		{
			bool[] defaultState = new bool[20];	// el defaultState seria con lo que empieza el jugador cuando le da a New Game
			int contador = 0;
			foreach (bool b in defaultState) 
			{
				defaultState[contador] = false;	// todos falsos ya que no colectee ningun cubo
				contador++;
			}
			return defaultState;	//devuelve el int array con los datos default cuando se empieza de 0 ya que no hay datos guardados o se borraron
		}
	}

	// ELIMINO LOS DATOS DEL JUGADOR
    public static void EliminarJugador()
    {
		Debug.Log ("Se llamo a la funcion EliminarJugador de GestorDePersistencia.");

        if (File.Exists(Application.persistentDataPath + "/jugador.sav"))
        {
            try
            {
                File.Delete(Application.persistentDataPath + "/jugador.sav");
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
            
        }
    }
        
}

// ESTOS SON LOS DATOS QUE VOY A GUARDAR Y CARGAR
[Serializable]
public class JugadorData
{
    public int[] datos;
	public bool[] cubosDatos;

    public JugadorData(ControlJugador jugador)
    {
        datos = new int[4];
		datos[0] = jugador.checkpoint;
        datos[1] = jugador.vidas;
		datos[2] = jugador.puntuacion;
		datos[3] = jugador.nivel;


		cubosDatos = new bool[20];
		cubosDatos = jugador.cubosColectados;
    }
}
