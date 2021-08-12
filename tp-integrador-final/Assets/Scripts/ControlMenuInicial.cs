using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlMenuInicial : MonoBehaviour {

	public void Jugar()
	{
		GestorDeJuego.Jugar();
		
	}

	public void Continuar()
	{
		GestorDeJuego.Continuar();
		
	}
	public void Cerrar ()
	{
		GestorDeJuego.Cerrar();
	}
}
