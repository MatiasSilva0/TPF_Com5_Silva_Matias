using System;
using System.Collections.Generic;

namespace DeepSpace
{

	public class Pila<T>
	{		
		private List<T> datos = new List<T>();
	
		public void apilar(T elem) {
			this.datos.Add(elem);
		}
	
		public T desapilar() {
			T temp = this.datos[this.datos.Count - 1];
			this.datos.RemoveAt(this.datos.Count - 1);
			return temp;
		}
		
		public T tope() {
			return this.datos[this.datos.Count - 1]; 
		}
		
		public bool esVacia() {
			return this.datos.Count == 0;
		}
	}
}
