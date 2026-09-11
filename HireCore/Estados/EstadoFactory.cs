namespace HireCore.Estados
{
    public class EstadoFactory
    {
        private readonly Dictionary<string, IEstadoCandidato> _mapaEstados = new(StringComparer.OrdinalIgnoreCase);

        public EstadoFactory()
        {
            var aplicado = Registrar(new Aplicado());
            var entrevista = Registrar(new Entrevista());
            var pruebaTecnica = Registrar(new PruebaTecnica());
            var oferta = Registrar(new Oferta());
            var verificacion = Registrar(new VerificacionReferencias());
            var contratado = Registrar(new Contratado());
            var rechazado = Registrar(new Rechazado());

            ConfigurarTransiciones(aplicado, entrevista, rechazado);
            ConfigurarTransiciones(entrevista, pruebaTecnica, rechazado);
            ConfigurarTransiciones(pruebaTecnica, oferta, rechazado);
            ConfigurarTransiciones(oferta, verificacion, rechazado);
            ConfigurarTransiciones(verificacion, contratado, rechazado);
        }

        public IEstadoCandidato ObtenerEstado(string nombreEstado)
        {
            if (!_mapaEstados.TryGetValue(nombreEstado, out var estado))
            {
                throw new InvalidOperationException($"Estado desconocido: {nombreEstado}");
            }
            return estado;
        }

        private T Registrar<T>(T estado) where T : IEstadoCandidato
        {
            _mapaEstados[estado.GetNombre()] = estado;
            return estado;
        }

        private void ConfigurarTransiciones(EstadoBase origen, params IEstadoCandidato[] destinosPermitidos)
        {
            foreach (var destino in destinosPermitidos)
            {
                origen.PermitirTransicion(destino.GetNombre());
            }
        }
    }
}
