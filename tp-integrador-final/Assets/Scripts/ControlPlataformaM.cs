using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPlataformaM : MonoBehaviour {

	public float velocidad;

	// TIENEN QUE SER SOLO 2 WAYPOINTS PARA QUE FUNCIONE
	public Transform[] waypoints;	// aca es donde voy a poner los waypoints de la plataforma

	private int wp;

	// Use this for initialization
	void Start () 
	{
		wp = 0;

	}
	
	void FixedUpdate () 
	{
		transform.position = Vector3.MoveTowards (transform.position, waypoints [wp].transform.position, (velocidad * Time.deltaTime));	// me muevo hacia el wp actual

		if (Vector3.Distance (waypoints [wp].transform.position, transform.position) <= 0) // si la distancia entre el wp y yo es menor o igual a cero
		{																									
			if (wp == 0)
			{
				wp = 1;	// paso al otro waypoint
			}
			else if (wp == 1)
			{
				wp = 0;	// paso al otro waypoint
			}
		}


	}
}
