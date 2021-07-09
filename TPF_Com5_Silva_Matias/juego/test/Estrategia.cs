
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
			// si la raíz no es del IA
			if(!arbol.getDatoRaiz().EsPlanetaDeLaIA())
			{
				Pila<ArbolGeneral<Planeta>> pila = new Pila<ArbolGeneral<Planeta>>();
				pila.apilar(arbol);
				Pila<ArbolGeneral<Planeta>> p = capturarHijo(pila);
				
				ArbolGeneral<Planeta> origen = null;
				while(p.tope().getDatoRaiz().EsPlanetaDeLaIA())
					origen = p.desapilar();
				
				ArbolGeneral<Planeta> destino = p.tope();
				Movimiento mov = new Movimiento(origen.getDatoRaiz(), destino.getDatoRaiz());
				return mov;
			}
			
			else
			{
				Pila<ArbolGeneral<Planeta>> pila = new Pila<ArbolGeneral<Planeta>>();
				pila.apilar(arbol);
				Pila<ArbolGeneral<Planeta>> p = capturarRaiz(pila);
				
				// busco origen
				ArbolGeneral<Planeta> destino = null;
				while(!p.tope().getDatoRaiz().EsPlanetaDeLaIA())
					destino = p.desapilar();
				
				ArbolGeneral<Planeta> origen = p.tope();
				Movimiento mov = new Movimiento(origen.getDatoRaiz(), destino.getDatoRaiz());
				return mov;
			}
		}
		
		public Pila<ArbolGeneral<Planeta>> capturarHijo(Pila<ArbolGeneral<Planeta>> pila)
		{
			if(pila.tope().getDatoRaiz().EsPlanetaDeLaIA())
				return pila;
			foreach(ArbolGeneral<Planeta> hijo in pila.tope().getHijos())
			{
				pila.apilar(hijo);
				capturarHijo(pila);
				if(pila.tope().getDatoRaiz().EsPlanetaDeLaIA())
					return pila;
				pila.desapilar();
			}
			return pila;
		}
		
		public Pila<ArbolGeneral<Planeta>> capturarRaiz(Pila<ArbolGeneral<Planeta>> pila)
		{
			if(!pila.tope().getDatoRaiz().EsPlanetaDeLaIA())
				return pila;
			foreach(ArbolGeneral<Planeta> hijo in pila.tope().getHijos())
			{
				pila.apilar(hijo);
				capturarRaiz(pila);
				if(!pila.tope().getDatoRaiz().EsPlanetaDeLaIA())
					return pila;
				pila.desapilar();
			}
			return pila;
		}
	}
}
