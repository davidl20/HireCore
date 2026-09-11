using HireCore.Estados;
using HireCore.Modelo;

namespace HireCore.Notificaciones
{
    public interface IObservadorNotificacion
    {
        void Notificar(Candidato candidato, IEstadoCandidato estadoAnterior, IEstadoCandidato estadoNuevo);
    }
}

