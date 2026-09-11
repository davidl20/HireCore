using HireCore.Estados;
using HireCore.Modelo;

namespace HireCore.Notificaciones
{
    public class NotificadorReclutador : IObservadorNotificacion
    {
        public void Notificar(Candidato candidato, IEstadoCandidato estadoAnterior, IEstadoCandidato estadoNuevo)
        {
            Console.WriteLine($"[Email a Reclutador ({candidato.GetReclutadorEmail})]: {candidato.GetNombre} cambió de {estadoAnterior.GetNombre()} a {estadoNuevo.GetNombre()}");
        }
    }
}
