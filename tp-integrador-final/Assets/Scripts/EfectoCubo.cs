using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EfectoCubo : MonoBehaviour {

    public GameObject efectoPrefab;
    public GameObject ef;

	void Awake ()
    {
        ef = Instantiate(efectoPrefab, gameObject.transform.position, Quaternion.identity);
	}

}
