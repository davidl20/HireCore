namespace HireCore.Estados
{
    public abstract class EstadoBase : IEstadoCandidato
    {
        protected HashSet<string> transicionesPermitidas { get; } = new(StringComparer.OrdinalIgnoreCase);

        public bool EsTransicionValida(string siguienteEstado)
        {
            return transicionesPermitidas.Contains(siguienteEstado);
        }

        public virtual string GetNombre()
        {
            return GetType().Name.Replace("Estado", "").ToUpper();
        }

        public void PermitirTransicion(string nombreSiguienteEstado)
        {
            transicionesPermitidas.Add(nombreSiguienteEstado);
        }
    }
}
