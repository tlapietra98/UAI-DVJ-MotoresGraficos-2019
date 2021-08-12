using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlCamara : MonoBehaviour {

    public GameObject Jugador;

	void Start ()
    {
    }
	
	void LateUpdate ()
    {
        transform.position = new Vector3(Jugador.transform.position.x, Jugador.transform.position.y + 5f, transform.position.z);

		
	}
}
