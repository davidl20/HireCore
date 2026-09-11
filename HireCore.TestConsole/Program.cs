using System;
using HireCore.Modelo;
using HireCore.Estados;
using HireCore.Notificaciones;
using HireCore.Auditoria;
using HireCore.Gestores;

namespace HireCore.TestConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== INICIALIZANDO SISTEMA DE GESTIÓN DE CANDIDATOS ===\n");

            // 1. Instanciar los servicios principales (Infraestructura)
            var factory = new EstadoFactory();
            var publicador = new PublicadorNotificaciones();
            var historial = new HistorialAuditoria();

            // 2. Suscribir los Notificadores (Patrón Observer)
            publicador.Suscribir(new NotificadorReclutador());
            publicador.Suscribir(new NotificadorGerente());
            publicador.Suscribir(new NotificadorNomina());
            publicador.Suscribir(new NotificadorPortalCandidato());

            // 3. Crear el Gestor (Fachada / Controlador principal)
            var gestor = new GestorCandidato(historial, publicador, factory);

            // 4. Crear un Candidato en estado inicial 'APLICADO'
            IEstadoCandidato estadoInicial = factory.ObtenerEstado("APLICADO");
            var candidato = new Candidato("Carlos Pérez", "reclutador@tech.com", estadoInicial);

            Console.WriteLine($"Candidato Creado: {candidato.GetNombre()} | Estado Inicial: {candidato.GetEstadoActual().GetNombre()}\n");

            // -------------------------------------------------------------
            // PRUEBA 1: Transiciones Válidas Secuenciales
            // -------------------------------------------------------------
            Console.WriteLine("--- PRUEBA 1: Transiciones Válidas ---");

            Console.WriteLine("\n> Avanzando a ENTREVISTA:");
            gestor.CambiarEstado(candidato, "ENTREVISTA", "Usuario_HR_1");

            Console.WriteLine("\n> Avanzando a PRUEBATECNICA:");
            gestor.CambiarEstado(candidato, "PRUEBATECNICA", "Usuario_LiderTecnico");

            Console.WriteLine($"\nEstado Actual de {candidato.GetNombre()}: {candidato.GetEstadoActual().GetNombre()}\n");

            // -------------------------------------------------------------
            // PRUEBA 2: Intentar una Transición Inválida (Lógica del State)
            // -------------------------------------------------------------
            Console.WriteLine("--- PRUEBA 2: Intento de Transición Inválida ---");
            try
            {
                Console.WriteLine("\n> Intentando saltar directo de PRUEBATECNICA a CONTRATADO (Debe fallar):");
                gestor.CambiarEstado(candidato, "CONTRATADO", "Usuario_HR_1");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR CAPTURADO CORRECTAMENTE]: {ex.Message}");
            }

            // -------------------------------------------------------------
            // PRUEBA 3: Deshacer Cambios (Comando + Memento)
            // -------------------------------------------------------------
            Console.WriteLine("\n--- PRUEBA 3: Funcionalidad Deshacer (Undo) ---");

            Console.WriteLine("\n> Ejecutando Deshacer 1:");
            gestor.DeshacerUltimaAccion();
            Console.WriteLine($"Estado tras Deshacer: {candidato.GetEstadoActual().GetNombre()}");

            Console.WriteLine("\n> Ejecutando Deshacer 2:");
            gestor.DeshacerUltimaAccion();
            Console.WriteLine($"Estado tras Deshacer: {candidato.GetEstadoActual().GetNombre()}");

            Console.WriteLine("\n=== FIN DE LAS PRUEBAS ===");
        }
    }
}