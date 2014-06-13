using System;
using System.Collections.Generic;
using System.Text;

namespace AlumnoEjemplos.Blizarro
{
    interface Icamara
    {
        void Initialize(ObjetoGrafico objeto);
        void Actualizar(ObjetoGrafico objeto);
        void Rotar(float rotacion);
    }
}
