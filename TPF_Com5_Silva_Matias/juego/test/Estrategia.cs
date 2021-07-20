
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
				
				// si es hoja y su población > 3, incremento contador
				if(aux.esHoja() && aux.getDatoRaiz().population > 3)
					x = x + 1;
			}
			return ("El número de planetas hojas con población mayor a 3 es: "+x);
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
			   	Pila<ArbolGeneral<Planeta>> raiz = new Pila<ArbolGeneral<Planeta>>();
			   	raiz.apilar(arbol);
			   	Pila<ArbolGeneral<Planeta>> caminoBot = encontrarBot(raiz);
			   	
			   	// reforzar raíz
			   	Pila<ArbolGeneral<Planeta>> subRaiz = new Pila<ArbolGeneral<Planeta>>();
			   	subRaiz.apilar(caminoBot.tope());
			   	Pila<ArbolGeneral<Planeta>> caminoR = reforzarRaiz(subRaiz, subRaiz.tope().getDatoRaiz().population);
			   	if(caminoR.tope() == null)
			   	{
			   		caminoR.desapilar();
			   		Planeta origen = caminoR.desapilar().getDatoRaiz();
			   		Planeta destino = caminoR.tope().getDatoRaiz();
			   		Movimiento mov = new Movimiento(origen, destino);
			   		return mov;
			   	}
			   	
			   	// buscar subárbol hijo sin planeta IA
			   	Pila<ArbolGeneral<Planeta>> subraiz = new Pila<ArbolGeneral<Planeta>>();
			   	subraiz.apilar(caminoBot.tope());
			   	Pila<ArbolGeneral<Planeta>> subarbol = buscarSubarbol(subraiz);
			   	
			   	// si existe
			   	if(!subarbol.tope().getDatoRaiz().EsPlanetaDeLaIA())
			   	{
			   		// retornar camino al hijo
			   		Planeta destino = subarbol.desapilar().getDatoRaiz();
			   		Planeta origen = subarbol.tope().getDatoRaiz();
			   		Movimiento mov = new Movimiento(origen, destino);
			   		return mov;
			   	}
			   	
			   	// si no existe
			   	else
			   	{
			   		// retornar camino a la raíz
			   		Planeta origen = caminoBot.desapilar().getDatoRaiz();
			   		Planeta destino = caminoBot.tope().getDatoRaiz();
			   		Movimiento mov = new Movimiento(origen, destino);
			   		return mov;
			   	}
			}
			
			// si la raíz es de la IA
			else
			{
				// reforzar raíz
			   	Pila<ArbolGeneral<Planeta>> raiz = new Pila<ArbolGeneral<Planeta>>();
			   	raiz.apilar(arbol);
			   	Pila<ArbolGeneral<Planeta>> CaminoR = reforzarRaiz(raiz, raiz.tope().getDatoRaiz().population);
			   	if(CaminoR.tope() == null)
			   	{
			   		CaminoR.desapilar();
			   		Planeta ori = CaminoR.desapilar().getDatoRaiz();
			   		Planeta dest = CaminoR.tope().getDatoRaiz();
			   		Movimiento m = new Movimiento(ori, dest);
			   		return m;
			   	}
			   	
				// buscar subárbol hijo
			   	Pila<ArbolGeneral<Planeta>> subarbol = buscarSubarbol(raiz);
			   	
			    // retornar camino al hijo
			   	Planeta destino = subarbol.desapilar().getDatoRaiz();
			   	Planeta origen = subarbol.tope().getDatoRaiz();
			   	Movimiento mov = new Movimiento(origen, destino);
			   	return mov;
			}
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
		
		public Pila<ArbolGeneral<Planeta>> reforzarRaiz(Pila<ArbolGeneral<Planeta>> pila, uint poblacion)
		{
			// si el planeta no es de la IA, retornar
			if(!pila.tope().getDatoRaiz().EsPlanetaDeLaIA())
				return pila;
			
			// si el planeta tiene más población que la raíz, marcar terminado(null), y retornar
			if(pila.tope().getDatoRaiz().population > poblacion)
			{
				pila.apilar(null);
				return pila;
			}
			
			// si no, recursión por cada hijo
			foreach(ArbolGeneral<Planeta> hijo in pila.tope().getHijos())
			{
				pila.apilar(hijo);
				reforzarRaiz(pila, poblacion);
				if(pila.tope() == null)
					return pila;
				pila.desapilar();
			}
			return pila;
		}
	}
}
