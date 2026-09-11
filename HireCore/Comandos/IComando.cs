namespace HireCore.Comandos
{
    public interface IComando
    {
        void Ejecutar();
        void Deshacer();
    }
}
