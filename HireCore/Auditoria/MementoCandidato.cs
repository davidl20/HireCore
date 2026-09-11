using HireCore.Estados;

namespace HireCore.Auditoria
{
    public class MementoCandidato
    {
        private IEstadoCandidato estadoGuardado;
        private string usuarioAccion;
        private DateTime fechaHora;

        public MementoCandidato(IEstadoCandidato estadoGuardado, string usuarioAccion)
        {
            this.estadoGuardado = estadoGuardado;
            this.usuarioAccion = usuarioAccion;
            this.fechaHora = DateTime.Now;
        }

        public IEstadoCandidato GetEstadoGuardado()
        {
            return estadoGuardado;
        }

        public string GetUsuarioAccion()
        {
            return usuarioAccion;
        }

        public DateTime GetFechaHora()
        {
            return fechaHora;
        }
    }
}
