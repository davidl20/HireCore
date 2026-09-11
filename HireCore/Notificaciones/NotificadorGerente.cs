using HireCore.Estados;
using HireCore.Modelo;

namespace HireCore.Notificaciones
{
    public class NotificadorGerente : IObservadorNotificacion
    {
        public void Notificar(Candidato candidato, IEstadoCandidato estadoAnterior, IEstadoCandidato estadoNuevo)
        {
            string nombreNuevo = estadoNuevo.GetNombre();
            if (nombreNuevo == "OFERTA" || nombreNuevo == "CONTRATADO")
            {
                Console.WriteLine($"[Email a Gerente de Contratación]: {candidato.GetNombre} pasó a {nombreNuevo}");
            }
        }
    }
}
