using HireCore.Comandos;

namespace HireCore.Auditoria
{
    public class HistorialAuditoria
    {
        private readonly Stack<IComando> _historial = new();

        public void RegistrarYEjecutar(IComando comando)
        {
            comando.Ejecutar();
            _historial.Push(comando);
        }

        public void DeshacerUltimo()
        {
            if (_historial.Count > 0)
            {
                var ultimoComando = _historial.Pop();
                ultimoComando.Deshacer();
            }
        }
    }
}
