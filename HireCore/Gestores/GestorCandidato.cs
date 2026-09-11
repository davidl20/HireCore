using HireCore.Auditoria;
using HireCore.Comandos;
using HireCore.Estados;
using HireCore.Modelo;
using HireCore.Notificaciones;

namespace HireCore.Gestores
{
    public class GestorCandidato
    {
        private readonly HistorialAuditoria _historial;
        private readonly PublicadorNotificaciones _publicador;
        private readonly EstadoFactory _factory;

        public GestorCandidato(HistorialAuditoria historial, PublicadorNotificaciones publicador, EstadoFactory factory)
        {
            _historial = historial;
            _publicador = publicador;
            _factory = factory;
        }

        public void CambiarEstado(Candidato candidato, string nuevoEstadoNombre, string usuario)
        {
            var nuevoEstado = _factory.ObtenerEstado(nuevoEstadoNombre);
            var comando = new ComandoCambiarEstado(candidato, nuevoEstado, usuario, _publicador);
            _historial.RegistrarYEjecutar(comando);
        }

        public void DeshacerUltimaAccion()
        {
            _historial.DeshacerUltimo();
        }
    }
}
