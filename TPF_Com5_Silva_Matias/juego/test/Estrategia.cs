
using System;
using System.Collections.Generic;
namespace DeepSpace
{

	class Estrategia
	{
		
        public String Consulta1( ArbolGeneral<Planeta> arbol)
		{
			Cola<ArbolGeneral<Planeta>> c = new Cola<ArbolGeneral<Planeta>>();
			ArbolGeneral<Planeta> aux = null;
			bool check = false;
			c.encolar(arbol);
			
			// busco raíz de la IA
			// si check = true, encontré la raíz
			
			while(!check)
			{
				aux = c.desencolar();
				if(aux.getDatoRaiz().EsPlanetaDeLaIA())
					check = true;
				else
				{
					foreach(ArbolGeneral<Planeta> hijo in aux.getHijos())
						c.encolar(hijo);
				}
			}
			
			// le busco la altura a la raíz
			return "La distancia entre la raíz y la hoja más lejana es: "+aux.altura();
		}

		public String Consulta2( ArbolGeneral<Planeta> arbol)
		{
			Cola<ArbolGeneral<Planeta>> c = new Cola<ArbolGeneral<Planeta>>();
			ArbolGeneral<Planeta> aux;
			int x = 0; // contador
			c.encolar(arbol);
			
			// mientras cola no se vacíe
			while(!c.esVacia())
			{
				aux = c.desencolar();
				foreach(ArbolGeneral<Planeta> hijo in aux.getHijos())
					c.encolar(hijo);
				
				// si población > 3, incremento contador
				if(aux.esHoja() && aux.getDatoRaiz().population > 3)
					x = x + 1;
			}
			return ("El número de planetas con población mayor a 3 es: "+x);
		}
		
		public String Consulta3( ArbolGeneral<Planeta> arbol)
		{
			Cola<ArbolGeneral<Planeta>> c = new Cola<ArbolGeneral<Planeta>>();
			ArbolGeneral<Planeta> aux;
			c.encolar(arbol);
			c.encolar(null);
			uint pro = promedio(arbol);
			uint count = 0;
			uint nivel = 0;
			string mensaje = "Número de planetas con población mayor al promedio:\n";
			
			// mientras cola no se vacíe
			while(!c.esVacia())
			{
				aux = c.desencolar();
				
				// si no terminé con el nivel
				if(aux != null)
				{
					// si población > promedio, incremento contador
					if(aux.getDatoRaiz().population > pro)
						count = count + 1;
					foreach(ArbolGeneral<Planeta> hijo in aux.getHijos())
						c.encolar(hijo);
				}
				
				// si ya terminé con el nivel
				else
				{
					// imprimo mensaje del nivel
					mensaje = mensaje + ("Nivel "+nivel+": "+count+"\n");
					
					// reinicio contador y paso al sig. nivel
					nivel = nivel + 1;
					count = 0;
					
					// si la cola está vacía, terminé de explorar el árbol 
					if(!c.esVacia())
						c.encolar(null);
				}
			}
			return mensaje;
		}
		
		public uint promedio(ArbolGeneral<Planeta> arbol)
		{
			Cola<ArbolGeneral<Planeta>> c = new Cola<ArbolGeneral<Planeta>>();
			ArbolGeneral<Planeta> aux;
			c.encolar(arbol);
			uint pro = 0;
			uint count = 0;
			
			// mientras cola no se vacíe
			while(!c.esVacia())
			{
				aux = c.desencolar();
				
				// sumo al total la población del planeta e incremento contador
				pro = pro + aux.getDatoRaiz().population;
				count = count + 1;
				foreach(ArbolGeneral<Planeta> hijo in aux.getHijos())
					c.encolar(hijo);
			}
			return (pro / count);
		}
		
		public Movimiento CalcularMovimiento(ArbolGeneral<Planeta> arbol)
		{
			// si la raíz no es de la IA
			if(!arbol.getDatoRaiz().EsPlanetaDeLaIA())
			{
			   	// encontrar Bot
			   	Pila<ArbolGeneral<Planeta>> pila1 = new Pila<ArbolGeneral<Planeta>>();
			   	pila1.apilar(arbol);
			   	Pila<ArbolGeneral<Planeta>> pila2 = encontrarBot(pila1);
			   	
			   	// buscar subárbol hijo
			   	Pila<ArbolGeneral<Planeta>> pila3 = new Pila<ArbolGeneral<Planeta>>();
			   	pila3.apilar(pila2.tope());
			   	Pila<ArbolGeneral<Planeta>> pila4 = buscarSubarbol(pila3);
			   	
			   	// si existe
			   	if(!pila4.tope().getDatoRaiz().EsPlanetaDeLaIA())
			   	{
			   		// retornar camino al hijo
			   		Planeta destino = pila4.desapilar().getDatoRaiz();
			   		while(!pila4.tope().getDatoRaiz().EsPlanetaDeLaIA())
			   			destino = pila4.desapilar().getDatoRaiz();
			   		Planeta origen = pila4.tope().getDatoRaiz();
			   		Movimiento mov = new Movimiento(origen, destino);
			   		return mov;
			   	}
			   	
			   	// si no existe
			   	else
			   	{
			   		// retornar camino a la raíz
			   		Planeta origen = pila2.desapilar().getDatoRaiz();
			   		Planeta destino = pila2.tope().getDatoRaiz();
			   		Movimiento mov = new Movimiento(origen, destino);
			   		return mov;
			   	}
			}
			
			return null;
		}
		
		public Pila<ArbolGeneral<Planeta>> encontrarBot(Pila<ArbolGeneral<Planeta>> pila)
		{
			// si es de la IA, retornar
			if(pila.tope().getDatoRaiz().EsPlanetaDeLaIA())
				return pila;
			
			// si no es de la IA, recursión por cada hijo
			foreach(ArbolGeneral<Planeta> hijo in pila.tope().getHijos())
			{
				pila.apilar(hijo);
				encontrarBot(pila);
				if(pila.tope().getDatoRaiz().EsPlanetaDeLaIA())
					return pila;
				pila.desapilar();
			}
			return pila;
		}
		
		public Pila<ArbolGeneral<Planeta>> buscarSubarbol(Pila<ArbolGeneral<Planeta>> pila)
		{
			// si no es de la IA, retornar
			if(!pila.tope().getDatoRaiz().EsPlanetaDeLaIA())
				return pila;
			
			// si es de la IA, recursión por cada hijo
			foreach(ArbolGeneral<Planeta> hijo in pila.tope().getHijos())
			{
				pila.apilar(hijo);
				buscarSubarbol(pila);
				if(!pila.tope().getDatoRaiz().EsPlanetaDeLaIA())
					return pila;
				pila.desapilar();
			}
			return pila;
		}
	}
}
