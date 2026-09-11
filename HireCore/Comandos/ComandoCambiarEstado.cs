using HireCore.Auditoria;
using HireCore.Estados;
using HireCore.Modelo;
using HireCore.Notificaciones;

namespace HireCore.Comandos
{
    public class ComandoCambiarEstado : IComando
    {
        private readonly Candidato _candidato;
        private readonly IEstadoCandidato _nuevoEstado;
        private readonly string _usuario;
        private readonly PublicadorNotificaciones _publicador;
        private MementoCandidato? _memento;

        public ComandoCambiarEstado(Candidato candidato, IEstadoCandidato nuevoEstado, string usuario, PublicadorNotificaciones publicador)
        {
            _candidato = candidato;
            _nuevoEstado = nuevoEstado;
            _usuario = usuario;
            _publicador = publicador;
        }

        public void Deshacer()
        {
            if (_memento == null) return;

            var estadoAnterior = _candidato.GetEstadoActual();
            var estadoRestaurado = _memento.GetEstadoGuardado();

            _candidato.SetEstadoActual(estadoRestaurado);

            Console.WriteLine($"[AUDITORÍA] {_memento.GetUsuarioAccion} deshizo el cambio realizado el {_memento.GetFechaHora():yyyy-MM-dd HH:mm:ss}");

            _publicador.NotificarCambio(_candidato, estadoAnterior, estadoRestaurado);
        }

        public void Ejecutar()
        {
            var estadoActual = _candidato.GetEstadoActual();
            if (!estadoActual.EsTransicionValida(_nuevoEstado.GetNombre()))
            {
                throw new InvalidOperationException($"Transición inválida: {estadoActual.GetNombre()} -> {_nuevoEstado.GetNombre()}");
            }

            _memento = new MementoCandidato(estadoActual, _usuario);
            _candidato.SetEstadoActual(_nuevoEstado);
            _publicador.NotificarCambio(_candidato, estadoActual, _nuevoEstado);
        }
    }
}
