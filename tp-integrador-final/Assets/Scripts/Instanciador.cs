using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instanciador : MonoBehaviour {

    public GameObject prefab;
    public int cantidad;
    public int frecuencia;
	public Vector2 rangosLimiteX;
	public Vector2 rangosLimiteY;
    private int timer;

	void Start ()
    {
        timer = 0;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        timer++;

        if (timer == frecuencia)
        {
            Instanciar();
        }


	}

    void Instanciar()
    {
        for (int i = cantidad; i > 0; i--)
        {
			// en el inspector indico los rangos de instanciacion limites en ejes X y Y, pero Z lo dejo solo
			Instantiate(prefab, new Vector3(Random.Range(rangosLimiteX.x, rangosLimiteX.y), Random.Range(rangosLimiteY.x, rangosLimiteY.y), 0f), Quaternion.identity);
        }

        timer = 0;
    }
}
