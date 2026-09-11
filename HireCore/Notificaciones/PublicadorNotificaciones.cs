using HireCore.Estados;
using HireCore.Modelo;

namespace HireCore.Notificaciones
{
    public class PublicadorNotificaciones
    {
        private readonly List<IObservadorNotificacion> _observadores = new();

        public void Suscribir(IObservadorNotificacion observador)
        {
            _observadores.Add(observador);
        }

        public void Desuscribir(IObservadorNotificacion observador)
        {
            _observadores.Remove(observador);
        }

        public void NotificarCambio(Candidato candidato, IEstadoCandidato anterior, IEstadoCandidato nuevo)
        {
            foreach (var observador in _observadores)
            {
                observador.Notificar(candidato, anterior, nuevo);
            }
        }
    }
}
