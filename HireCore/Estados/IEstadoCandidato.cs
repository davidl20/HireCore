namespace HireCore.Estados
{
    public interface IEstadoCandidato
    {
        public String GetNombre();
        public bool EsTransicionValida(string siguienteEstado);
        public void PermitirTransicion(string nombreSiguienteEstado);
    }
}
