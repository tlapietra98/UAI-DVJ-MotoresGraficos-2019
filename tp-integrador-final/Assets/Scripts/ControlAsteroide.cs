using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlAsteroide : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (transform.position.y < -3f)
        {
            Destroy(this.gameObject);
        }

	}

    private void OnCollisionEnter(Collision choque)
    {
        if (choque.gameObject.layer == 13) //Piso es la layer 13
        {
            Destroy(this.gameObject);
        }
    }
}
