using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotar : MonoBehaviour {

    public Vector3 rotacion;

	void Start ()
    {
		
	}
	
	void Update ()
    {
        transform.Rotate(rotacion * Time.deltaTime);
	}
}
