using HireCore.Estados;

namespace HireCore.Modelo
{
    public class Candidato
    {
        private string nombre;
        private string reclutadorEmail;
        private IEstadoCandidato estadoActual;

        public Candidato(string nombre, string reclutadorEmail, IEstadoCandidato estadoInicial)
        {
            this.nombre = nombre;
            this.reclutadorEmail = reclutadorEmail;
            this.estadoActual = estadoInicial;
        }

        public string GetNombre()
        {
            return nombre;
        }

        public string GetReclutadorEmail()
        {
            return reclutadorEmail;
        }

        public IEstadoCandidato GetEstadoActual()
        {
            return estadoActual;
        }

        public void SetEstadoActual(IEstadoCandidato nuevoEstado)
        {
            this.estadoActual = nuevoEstado;
        }
    }
}
