using HireCore.Estados;
using HireCore.Modelo;

namespace HireCore.Notificaciones
{
    public class NotificadorNomina : IObservadorNotificacion
    {
        public void Notificar(Candidato candidato, IEstadoCandidato estadoAnterior, IEstadoCandidato estadoNuevo)
        {
            if (estadoNuevo.GetNombre() == "CONTRATADO")
            {
                Console.WriteLine($"[Email a Nómina]: {candidato.GetNombre} fue contratado.");
            }
        }
    }
}
