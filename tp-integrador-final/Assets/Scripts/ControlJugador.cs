using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// TO DO : ver si es posible reutilizar el instanciador random para otras cosas que tengan mas sentido
// TO DO : descifrar que carajo es el error 'SerializedObject taret has been destroyed' y porque sucede luego del Start tras cargar el siguiente nivel

public class ControlJugador : MonoBehaviour {

    private Rigidbody rb;
	public CapsuleCollider col;
	public GameObject checkpoint1;	// recordar que el gameobject que va referenciado es el 'checkpoint piso', no el checkpoint
	public GameObject checkpoint2;
	public GameObject checkpoint3;
	public GameObject endpoint;	// al llegar a este gameobject, se completo el nivel y se pasa al siguiente

	public GameObject[] cubos;	// array de referencia a los cubos para afectarlos

	public bool[] cubosColectados;	// array de bool para saber cuales cubos ya recogi

	[Range(1, 20)]
	public float velocidad;

	public int checkpoint;

    public int vidas;

	public int puntuacion;	// contador de cubos recogidos (cantidad total)

	public int nivel;

	public bool nivelCompleto;

    public LayerMask capaPiso;

	[Range(1, 20)]
	public float fuerzaSalto;

	private float caidaGravedad = 2.5f;
	private float saltoCorto = 2f;


    public Text textoJugadorStats;
    public Text objetivo;
    public Text textoGanaste;
    public Text textoTryAgain;

    //private bool puedoDobleSaltar;

    void Start ()
    {
       // Guardar();

        rb = GetComponent<Rigidbody>();

        col = GetComponent<CapsuleCollider>();

        //puedoDobleSaltar = false;

		Cargar();	// esto inicializa las variables de checkpoint, vidas, cont (que seria la cantidad de cubos recogidos), y nivel

		SetearCubos();	// toco cubos para desactivarlos si es que ya los habia recogido la ultima vez

		Reposicionar();	// muevo al jugador a donde deberia estar segun su checkpoint

        //cont = 0; esta de mas creo

		inicializarTextoJugadorStats();

		nivelCompleto = false;

		Debug.Log ("Indique que nivelCompleto es igual a false, ya que acabo de empezar un nuevo nivel...");

    }

    void Update()
    {
  
    }

    // ACA HAGO EL MOVIMIENTO Y EL INPUT DEL JUGADOR
    void FixedUpdate ()
    {
        float movHorizontal = Input.GetAxis("Horizontal");

        // Movimiento Izquierda
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || movHorizontal == -1)
        {
            transform.Translate(Vector3.left * velocidad * Time.deltaTime);
        }

        // Movimiento Derecha
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || movHorizontal == 1)
        {
            transform.Translate(Vector3.right * velocidad * Time.deltaTime);
        }

        // Salto
		if(EstaEnPiso() && (Input.GetKeyDown(KeyCode.Space)) && rb.velocity.y <= 0f)	// si estoy en el piso apretando el boton de salto
        {																				// lo de velocity es para que no me tome que aprete dos veces y me saque volando
            Debug.Log("Esta en el piso y estoy apretando Space");

            GestorDeAudio.instancia.ReproducirSonido("jump_01");	// Reproduzco el efecto de sonido de salto

            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);	// Salto

            //puedoDobleSaltar = true;
        }
        // Doble salto
		//else if (puedoDobleSaltar && (Input.GetKeyDown(KeyCode.Space)) && rb.velocity.y >= 0)
        //{
        //    Debug.Log("Esta en el aire y estoy apretando Space");
		//
        //    GestorDeAudio.instancia.ReproducirSonido("jump_01");
        //    rb.AddForce(Vector3.up * fuerzaSalto / 2, ForceMode.Impulse);
		//
        //    puedoDobleSaltar = false;
        //}

		// MEJORA AL SALTO, PARA QUE CAIGA MAS RAPIDO
		if (rb.velocity.y < 0) // Si mi velocidad vertical es menor a 0, osea que estoy cayendo
		{
			rb.velocity += Vector3.up * Physics.gravity.y * (caidaGravedad - 1) * Time.deltaTime; // Aumento la velocidad con la que caigo
		}
		else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space)) // Si dejo de presionar la tecla o boton de salto antes de peder mi velocidad vertical
		{
			rb.velocity += Vector3.up * Physics.gravity.y * (saltoCorto - 1) * Time.deltaTime;
		}
		// ^esto significa que solo voy a poder tener velocidad vertical positiva si estoy saltando y que si suelto el boton de salto voy a empezar a caer

        // Me caigo del nivel
		//if (gameObject.transform.position.y < -3)
		//{
		//	
		//	RecibirDaño();
		//
		//}


		// Empezar desde cero el nivel una vez muerto apretando Y
		if (Input.GetKeyDown (KeyCode.Y) && vidas == 0) 
		{
			GestorDeJuego.Jugar();
		}

		// Cerrar el juego una vez muerto apretando N
		if (Input.GetKeyDown (KeyCode.N) && vidas == 0) 
		{
			GestorDeJuego.Cerrar();
		}



		// Si complete el nivel, no estoy en el ultimo nivel, y apreto Y
		if (Input.GetKeyDown (KeyCode.Y) && nivelCompleto && nivel < 3) 
		{
			SiguienteNivel();
			
		}


			
    }

    private bool EstaEnPiso()
    {
        Vector3 vector = new Vector3(col.bounds.center.x, col.bounds.min.y, col.bounds.center.z);
        return Physics.CheckCapsule(col.bounds.center, vector, col.radius * .9f, capaPiso);
    }




	// FUNCIONES DE COLISION Y TRIGGERS

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coleccionable"))
        {
			Debug.Log ("Se ha hecho contacto con un cubo coleccionable.");

			RecogerCubo(other.gameObject);

        }

		if (other.gameObject.CompareTag("Checkpoint")) 
		{
			Debug.Log("Se ha hecho contacto con un checkpoint.");

			checkpoint = other.gameObject.GetComponent<CheckpointNumber>().number; // anoto el checkpoint

			// Guardar checkpoint, vidas, y cubos
			Guardar();
			Debug.Log("Se ha guardado el checkpoint " + checkpoint);

		}

		if (other.gameObject.CompareTag ("Endpoint")) 
		{
			Debug.Log("Se ha hecho contacto con el endpoint del nivel.");

			CompletarNivel ();


		}
			

		if (other.gameObject.CompareTag ("Malo")) 	// si entro en contacto con una bala, trampa, o enemigo
		{
			Debug.Log("Se ha hecho contacto con un objeto malo.");

			RecibirDaño();	// llamo a la funcion que me saca vida

		}

		if (other.gameObject.CompareTag ("PlataformaM"))	// si me paro sobre una plataforma movediza
		{
			Debug.Log ("Se ha hecho contacto con una plataforma movediza.");

			transform.parent = other.transform;	// el transform del jugador se vuelve hijo del de la plataforma
		}

    }

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag ("PlataformaM"))	// si salgo de estar sobre una plataforma movediza
		{
			transform.parent = null;	// el transform del jugador deja de tener parent
		}
		
	}
		

	// FUNCIONes DE COMPLETAR EL NIVEL y PASAR AL SIGUIENTE

	public void CompletarNivel()
	{
		Debug.Log ("Estoy ejecutando la funcion CompletarNivel...");

		nivelCompleto = true; // establezco que complete el nivel

		this.gameObject.GetComponent<MeshRenderer>().enabled = false;	// hago invisible al gameObject
		rb.isKinematic = true; //hago que no sea afectado por fisica asi me queda en la posicion donde llegue al endpoint
		velocidad = 0;	// Hago que no me pueda mover mas
		fuerzaSalto = 0; // Hago que no pueda saltar

		GestorDeAudio.instancia.ReproducirSonido("power_up_01");	// Reproduzco el efecto de sonido para cuando completo un nivel


		setearTextoJugadorStats ();


		// esto eran cosas que iban en la funcion SiguienteNivel, pero que puse aca para ver si hace que se vaya el error

		// tengo que resetear checkpoint, volver a poner todos los bools de cubos colectados como falsos, cambiar el valor de nivel para marcar que estoy en el siguiente, guardar los datos
		// y llamar al gestor para que cargue la siguiente escena, la cual va a cargar los nuevos datos del jugador
		checkpoint = 0;

		int contador = 0;
		foreach (bool b in cubosColectados) 
		{
			cubosColectados[contador] = false;	// todos falsos ya que no colectee ningun cubo del siguiente nivel
			contador++;
		}

		nivel++;	// pase al siguiente nivel

		Debug.Log ("Cambie el checkpoint a " + checkpoint + ", los cubos colectados a false, y el nivel a " + nivel);

		Guardar ();	// guardo los nuevos datos

		Debug.Log ("Guarde los datos...");

	}

	public void SiguienteNivel()
	{
		Debug.Log ("Estoy ejecutando la funcion SiguienteNivel...");


		// volver a insertar aca lo que movi


		GestorDeJuego.Continuar ();	// le digo al gestor de juego que "continue" en base a los datos guardados


	}



	// FUNCIONES DE MUERTE Y REPOSICIONAMIENTO

	public void Reposicionar()
	{
		Debug.Log ("Estoy ejecutando la funcion Reposicionar...");

		switch (checkpoint) 
		{
		case 0:
			rb.position = new Vector3 (0f, 1f, 0f);
			break;
		
		case 1:
			rb.position = new Vector3 (checkpoint1.transform.position.x, checkpoint1.transform.position.y + 1f, 0f);
			break;

		case 2:
			rb.position = new Vector3 (checkpoint2.transform.position.x, checkpoint2.transform.position.y + 1f, 0f);
			break;

		case 3:
			rb.position = new Vector3 (checkpoint3.transform.position.x, checkpoint3.transform.position.y + 1f, 0f);
			break;
		}
	}


	public void RecibirDaño()
	{

		Debug.Log ("Estoy ejecutando la funcion RecibirDaño...");


		GestorDeAudio.instancia.ReproducirSonido("hit_01");	// Reproduzco el efecto de sonido para cuando me dañan

		if (vidas <= 1)	// Si tengo 1 vida, osea que moriria...
		{
			Muerte ();
		}	
		// Si tengo mas de una vida
		else if (vidas > 1)
		{

			vidas--;	// restale una vida

			Reposicionar();	// muevo al jugador a su ultima posicion guardada

			Guardar();	// guardo lo que paso, es decir que perdi una vida

			setearTextoJugadorStats();	// actualizar la UI

		}
			
	}
		

	public void Muerte()	// el jugador murio, ahora que?
	{

		Debug.Log ("Estoy ejecutando la funcion Muerte...");


		GestorDeJuego.EliminarProgreso();	//llamo a la funcion que borra los datos que tenia


		this.gameObject.GetComponent<MeshRenderer>().enabled = false;	// hago invisible al gameObject
		rb.isKinematic = true; //hago que no sea afectado por fisica asi me queda en la posicion donde murio
		//rb.position = new Vector3(0f, 1f, 0f);	// me muevo a la posicion inicial original
		// deberia cambiar esto para que en vez de moverme al punto original del 1er nivel la camara quede donde me mori

		vidas = 0;	// Me quedo sin vidas
		velocidad = 0;	// Hago que no me pueda mover mas
		fuerzaSalto = 0; // Hago que no pueda saltar

		GestorDeAudio.instancia.ReproducirSonido("explosion_01");	// Reproduzco el efecto de sonido para cuando me muero


		// aca deberia haber un efecto de particular en la posicion donde recibí el daño que me mato


		setearTextoJugadorStats();	// actualizo la UI

	}



	// FUNCION PARA MANEJAR LOS CUBOS RECOLECTADOS

	public void RecogerCubo(GameObject cubo)
	{
		Debug.Log ("Estoy ejecutando la funcion RecogerCubo...");


		GestorDeAudio.instancia.ReproducirSonido("coin_01");	// reproduzco el sonido correspondiente a recoger un cubo de materia oscura

		puntuacion = puntuacion + 1;	// agrego a la puntuacion
		setearTextoJugadorStats();

		cubosColectados [cubo.GetComponent<IdentidadCubo> ().identidad] = true;	// en la posicion correspondiente del array marco que recogi ese cubo

		cubo.GetComponent<EfectoCubo>().ef.SetActive(false);
		cubo.SetActive(false);
		
	}

	public void DesaparecerCubo(GameObject cubo)
	{
		Debug.Log ("Estoy ejecutando la funcion DesaparecerCubo...");

		cubo.GetComponent<EfectoCubo>().ef.SetActive(false);
		cubo.SetActive(false);
	}

	public void SetearCubos()
	{
		Debug.Log ("Estoy ejecutando la funcion SetearCubos...");

		int contador = 0;

		foreach (bool b in cubosColectados) 	// en base a lo que me dice el array de bools de los cubos, voy a desactivar los cubos que tengo referenciados
		{
			if (b == true) 
			{	// si es true, lo colectee, osea que no deberia aparecer mas, chau chau

				DesaparecerCubo (cubos [contador].gameObject);
				//cubos[contador].gameObject.GetComponent<EfectoCubo>().ef.SetActive(false);
				//cubos[contador].gameObject.SetActive(false);	


				Debug.Log ("Intente destruir el cubo " + cubos [contador].name + " en la posicion array " + contador);
			} 
			else 
			{
				// si b es false, significa que no lo colectee todavia, osea que no se le hace nada
			}
			contador++;
		}

	}



	// FUNCIONES QUE TIENE QUE VER CON LA MEMORIA


	public void Guardar()
	{
		Debug.Log ("Estoy ejecutando la funcion Guardar...");

		GestorDePersistencia.GuardarJugador(this);
	}

	public void Cargar()
	{
		Debug.Log ("Estoy ejecutando la funcion Cargar...");

		int[] estadoCargado = GestorDePersistencia.CargarJugador();

		checkpoint = estadoCargado[0]; // estadoCargado 0 es el checkpoint

		vidas = estadoCargado[1]; // estadoCargado 1 es la cantidad de vidas que le queda antes que tenga que empezar desde el inicio del nivel

		puntuacion = estadoCargado[2];	// puntuacion, la cantidad de cubos recogidos al llegar al checkpoint

		nivel = estadoCargado[3]; // el nivel en el que se esta acutalmente


		// vv asigno los valores de los bools guardados al array de bools que corresponde a si los cubos fueron colectados
		cubosColectados = GestorDePersistencia.CargarCubos(); 

	}



    
	// FUNCIONES QUE TIENEN QUE VER CON LA UI

	void inicializarTextoJugadorStats()	// inicializa la UI
	{
		Debug.Log ("Estoy ejecutando la funcion inicializarTextoJugadorStats...");

		textoGanaste.text = "";
		objetivo.text = "Objetivo: Llegar al final del nivel y recolectar materia oscura en el camino.";
		setearTextoJugadorStats();
		textoTryAgain.text = "";


		//esto podria estar hecho mejor

	}


	void setearTextoJugadorStats()	// actualiza la UI en base a las variables
	{

		Debug.Log ("Estoy ejecutando la funcion setearTextoJugadorStats...");

		textoJugadorStats.text = "Materia Oscura: " + puntuacion.ToString() + "\n" + "Vidas: " + vidas.ToString();

		if (nivelCompleto && nivel == 3 && puntuacion == 60)
		{
			textoGanaste.text = "Completaste el juego y recolectaste la maxima cantidad de materia oscura! Felicitaciones!";
		}
		if (nivelCompleto)
		{
			textoGanaste.text = "Completaste el nivel! Tu progreso se ha guardado. Presiona Y para pasar al siguiente nivel.";
		}
		if (vidas == 0)
		{
			textoGanaste.text = "No tenes mas vidas, presiona Y para volver a jugar, o N para cerrar el juego!";
		}

		Debug.Log ("Ejecute la funcion setearTextoJugadorStats...");

	}






}
