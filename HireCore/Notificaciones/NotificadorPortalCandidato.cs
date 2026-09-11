using HireCore.Estados;
using HireCore.Modelo;

namespace HireCore.Notificaciones
{
    public class NotificadorPortalCandidato : IObservadorNotificacion
    {
        public void Notificar(Candidato candidato, IEstadoCandidato estadoAnterior, IEstadoCandidato estadoNuevo)
        {
            Console.WriteLine($"[Portal Candidato]: Actualización de estado para {candidato.GetNombre}: {estadoNuevo.GetNombre()}");
        }
    }
}
