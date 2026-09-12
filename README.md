[README.md](https://github.com/user-attachments/files/32146843/README.md)
# HireCore

## Sistema de Gestión de Candidatos (.NET / C#)

HireCore es un motor de gestión del flujo de contratación de candidatos desarrollado en C# (.NET). La solución aplica Arquitectura Orientada a Objetos, patrones de diseño GoF y principios SOLID para construir un sistema desacoplado, auditable y extensible.

---

## 📌 Contexto del problema

HireCore es un sistema interno de seguimiento de candidatos (ATS) utilizado por una empresa para gestionar el proceso de contratación.

Un candidato avanza por diferentes etapas, como:

```text
APLICADO → ENTREVISTA → OFERTA → CONTRATADO
```

También puede ser rechazado en cualquier punto del proceso.

### Implementación inicial

La implementación original concentraba la lógica en una única clase llamada `GestorDeCandidato`.

Esta clase era responsable de:

- Conocer los estados del candidato.
- Determinar qué transiciones eran válidas.
- Modificar el estado.
- Enviar las notificaciones correspondientes.

Conceptualmente:

```text
GestorDeCandidato
   ├── Conoce las etapas
   ├── Valida transiciones
   ├── Cambia estados
   └── Envía notificaciones
```

Este enfoque genera un alto acoplamiento y dificulta la evolución del sistema.

---

## ❗ Problemática

Recursos Humanos necesita ampliar el proceso de contratación.

### 1. Nuevas etapas

Se requiere agregar:

- `Prueba Técnica` entre `Entrevista` y `Oferta`.
- `Verificación de Referencias` entre `Oferta` y `Contratado`.

El flujo esperado sería:

```text
APLICADO
    ↓
ENTREVISTA
    ↓
PRUEBA TÉCNICA
    ↓
OFERTA
    ↓
VERIFICACIÓN DE REFERENCIAS
    ↓
CONTRATADO
```

Cada etapa debe declarar a qué etapas puede pasar el candidato.

No todas las transiciones son válidas. Por ejemplo:

```text
APLICADO ─────────────────→ CONTRATADO
              ✗
```

Además, agregar una nueva etapa en el futuro no debe requerir modificar las etapas existentes.

### 2. Notificaciones diferenciadas

Los diferentes participantes necesitan recibir información diferente:

| Destinatario | Cambios que recibe |
|---|---|
| Reclutador | Todos los cambios |
| Gerente de contratación | Oferta y Contratado |
| Nómina | Contratado |
| Portal del candidato | Todos los cambios excepto notas internas |

`GestorDeCandidato` no debe conocer directamente a cada destinatario.

La solución debe permitir agregar nuevos tipos de notificación sin modificar la lógica principal.

### 3. Deshacer con auditoría

Recursos Humanos necesita poder deshacer la última transición si fue realizada por error.

Por ejemplo:

```text
ENTREVISTA
     ↓
RECHAZADO
```

Si el rechazo fue accidental:

```text
RECHAZADO
     ↓
   DESHACER
     ↓
ENTREVISTA
```

Además, el sistema debe mantener un historial de auditoría indicando quién realizó cada cambio y cuándo ocurrió.

---

## 🎯 Objetivos del rediseño

La solución busca:

- Incorporar nuevas etapas sin modificar las existentes.
- Definir las transiciones permitidas de manera extensible.
- Desacoplar las notificaciones del gestor de candidatos.
- Agregar nuevos destinatarios fácilmente.
- Encapsular los cambios de estado como operaciones.
- Permitir deshacer la última operación.
- Mantener un historial para auditoría.
- Aplicar principios SOLID.
- Reducir el acoplamiento entre componentes.

---

# 📐 Arquitectura y Patrones de Diseño

La solución integra cinco patrones de diseño GoF:

```text
[GestorCandidato] ──(Command)──> [ComandoCambiarEstado] ──(Memento)──> [MementoCandidato]
       │
       ├──(Factory)──> [EstadoFactory]
       │
       ├──(Observer)──> [PublicadorNotificaciones] ──> [Notificadores Concretos]
       │
       └──(State)───> [IEstadoCandidato / EstadoBase]
```

---

## 1. State

**Patrón:** Comportamiento

### Propósito

Manejar los diferentes estados del candidato y validar las reglas de transición.

Estados contemplados:

- `Aplicado`
- `Entrevista`
- `PruebaTecnica`
- `Oferta`
- `VerificacionReferencias`
- `Contratado`
- `Rechazado`

### Implementación

`IEstadoCandidato` y `EstadoBase` encapsulan la lógica relacionada con los estados y las transiciones permitidas.

Esto evita concentrar toda la lógica en grandes bloques `if` / `switch`.

### Beneficio

Agregar nuevas etapas no requiere modificar la lógica de las etapas existentes.

---

## 2. Observer

**Patrón:** Comportamiento

### Propósito

Desacoplar la lógica del cambio de estado del envío de notificaciones.

### Implementación

`PublicadorNotificaciones` actúa como sujeto y notifica a los observadores suscritos:

- `NotificadorReclutador`
- `NotificadorGerente`
- `NotificadorNomina`
- `NotificadorPortalCandidato`

Cuando ocurre una transición, los observadores interesados reciben la información correspondiente.

### Beneficio

Es posible agregar nuevos notificadores sin modificar `GestorCandidato`.

---

## 3. Command

**Patrón:** Comportamiento

### Propósito

Encapsular las solicitudes de cambio de estado como objetos ejecutables y reversibles.

### Implementación

`ComandoCambiarEstado` representa una operación de cambio de estado y permite:

- Ejecutar la operación.
- Revertir la operación.

### Beneficio

La transición deja de ser una operación directamente acoplada al gestor y puede ser almacenada en un historial.

---

## 4. Memento

**Patrón:** Comportamiento

### Propósito

Conservar el estado previo del candidato para permitir la restauración y el Undo.

### Implementación

`MementoCandidato` almacena una fotografía del estado previo de la entidad.

`HistorialAuditoria` administra el historial de comandos mediante una pila:

```text
Stack<IComando>
```

Esto mantiene la semántica:

```text
LIFO
Last In, First Out
```

Es decir, la última operación realizada es la primera que puede deshacerse.

---

## 5. Factory

**Patrón:** Creacional

### Propósito

Centralizar la creación y configuración de los estados y de la matriz de transiciones permitidas.

### Implementación

`EstadoFactory` registra los estados e inyecta dinámicamente la configuración del flujo.

Esto evita que las reglas de transición estén hardcodeadas dentro de cada estado.

### Beneficio

Agregar una nueva etapa puede realizarse mediante una nueva implementación y su configuración, sin modificar las clases existentes.

---

# 🔗 Command + Memento

Estos dos patrones trabajan conjuntamente para implementar la funcionalidad de deshacer.

### Command

Se encarga de representar la operación:

```text
¿Qué operación se ejecutó?
```

### Memento

Conserva la información necesaria para restaurar el estado:

```text
¿A qué estado debemos regresar?
```

### Flujo

```text
Ejecutar transición
       ↓
Guardar estado anterior
       ↓
Ejecutar operación
       ↓
Registrar auditoría
       ↓
Notificar observadores
```

Para deshacer:

```text
Obtener última operación
       ↓
Recuperar Memento
       ↓
Restaurar estado anterior
```

---

# ⚙️ Principios SOLID aplicados

## S — Single Responsibility Principle

Cada componente tiene una responsabilidad específica:

- `GestorCandidato` coordina.
- `ComandoCambiarEstado` ejecuta una operación.
- `MementoCandidato` conserva el estado.
- Los notificadores envían mensajes.
- `EstadoFactory` crea y configura estados.

## O — Open/Closed Principle

El sistema puede extenderse agregando nuevas etapas o notificadores sin modificar el código existente.

Por ejemplo, se podría incorporar una nueva etapa como:

```text
EntrevistaIngles
```

mediante una nueva clase y su correspondiente registro.

## L — Liskov Substitution Principle

Las implementaciones concretas de estados y notificadores respetan los contratos definidos por sus abstracciones:

- `IEstadoCandidato`
- `IObservadorNotificacion`

## I — Interface Segregation Principle

Se utilizan interfaces pequeñas y cohesivas:

- `IEstadoCandidato`
- `IComando`
- `IObservadorNotificacion`

## D — Dependency Inversion Principle

Las clases de alto nivel dependen de abstracciones en lugar de implementaciones concretas.

La configuración se realiza mediante Inyección de Dependencias.

---

# 📊 Decisiones de diseño

| Criterio | Solución implementada | Justificación |
|---|---|---|
| Configuración de transiciones | Inyección externa en `EstadoFactory` | Favorece OCP y evita modificar clases existentes |
| Notificaciones | Observadores coordinados por `PublicadorNotificaciones` | Mantiene el desacoplamiento de Observer |
| Auditoría y Undo | `HistorialAuditoria` con `Stack<IComando>` | Mantiene la semántica LIFO para deshacer |

---

# 🔄 Flujo de una transición

El flujo general de una transición es:

```text
1. Solicitar transición
          ↓
2. Validar transición
          ↓
3. Guardar estado anterior
          ↓
4. Ejecutar transición
          ↓
5. Registrar auditoría
          ↓
6. Notificar observadores
```

---

# 🧪 Ejemplo de funcionamiento

Un candidato comienza en:

```text
APLICADO
```

Avanza a:

```text
APLICADO
    ↓
ENTREVISTA
```

Luego:

```text
ENTREVISTA
    ↓
PRUEBATECNICA
```

Y posteriormente:

```text
PRUEBATECNICA
    ↓
OFERTA
```

Si se intenta realizar una transición no permitida:

```text
PRUEBATECNICA
       ↓
CONTRATADO
```

el sistema rechaza la operación.

---

# 🚫 Problemas de la implementación original

La implementación original concentraba demasiadas responsabilidades en `GestorDeCandidato`.

Esto producía:

### Alto acoplamiento

El gestor dependía directamente de estados y servicios concretos.

### Violación de Open/Closed Principle

Agregar una etapa requería modificar la lógica existente.

### Violación de Single Responsibility Principle

Una sola clase tenía múltiples responsabilidades.

### Dificultad para implementar Undo

No existía un mecanismo adecuado para almacenar y restaurar estados anteriores.

### Dificultad para extender las notificaciones

Agregar nuevos destinatarios implicaba modificar el gestor.

---

# ✅ Resultado del rediseño

Después de aplicar los patrones:

```text
                 ┌──────────────┐
                 │   Gestor     │
                 └──────┬───────┘
                        │
                        ▼
                 ┌──────────────┐
                 │    State     │
                 └──────┬───────┘
                        │
              ┌─────────┴─────────┐
              ▼                   ▼
        ┌──────────┐       ┌────────────┐
        │ Command  │       │  Observer  │
        └────┬─────┘       └────────────┘
             │
             ▼
        ┌──────────┐
        │ Memento  │
        └──────────┘
```

Cada patrón tiene una responsabilidad específica:

| Patrón | Responsabilidad |
|---|---|
| **State** | Gestionar las etapas y sus transiciones |
| **Factory** | Crear y configurar los estados |
| **Observer** | Gestionar las notificaciones |
| **Command** | Encapsular las operaciones |
| **Memento** | Conservar el estado para Undo |

---

# 🚀 Guía de ejecución y pruebas

## Prerrequisitos

- .NET 8.0 SDK o superior.
- Visual Studio 2022 o VS Code.

## Clonar el repositorio

```bash
git clone https://github.com/davidl20/HireCore.git
cd HireCore
```

## Restaurar dependencias y compilar

```bash
dotnet build
```

## Ejecutar el proyecto de pruebas

```bash
dotnet run --project HireCore.TestConsole
```

---

# 🧪 Ejemplo de salida en consola

```text
=== INICIALIZANDO SISTEMA DE GESTIÓN DE CANDIDATOS ===

Candidato Creado: Carlos Pérez | Estado Inicial: APLICADO

--- PRUEBA 1: Transiciones Válidas ---

> Avanzando a ENTREVISTA:

[Notificación Portal]: El candidato Carlos Pérez pasó a estado ENTREVISTA.

[Notificación Reclutador]: Estado de Carlos Pérez actualizado por Usuario_HR_1.

> Avanzando a PRUEBATECNICA:

[Notificación Gerente]: Evaluando avance técnico de Carlos Pérez.

--- PRUEBA 2: Intento de Transición Inválida ---

> Intentando saltar directo de PRUEBATECNICA a CONTRATADO:

[ERROR CAPTURADO CORRECTAMENTE]:
Transición no permitida de PRUEBATECNICA a CONTRATADO

--- PRUEBA 3: Funcionalidad Deshacer (Undo) ---

> Ejecutando Deshacer 1:

Estado tras Deshacer: PRUEBATECNICA

=== FIN DE LAS PRUEBAS ===
```

---

# 📁 Estructura conceptual

```text
Estados
   ├── IEstadoCandidato
   └── EstadoBase

Creación
   └── EstadoFactory

Notificaciones
   ├── PublicadorNotificaciones
   └── Notificadores concretos

Operaciones
   └── ComandoCambiarEstado

Deshacer y auditoría
   ├── MementoCandidato
   └── HistorialAuditoria

Coordinación
   └── GestorCandidato
```

---

# 📐 Diagrama de clases

El proyecto incluye el diagrama de clases correspondiente a la solución final.

El diagrama representa:

- Clases.
- Interfaces.
- Atributos.
- Métodos.
- Herencia.
- Implementaciones.
- Composición.
- Dependencias.
- Relaciones entre los componentes de los patrones utilizados.

El diagrama debe mantenerse sincronizado con la implementación actual del proyecto.

---

# 🎓 Objetivo académico

Este proyecto corresponde al reto evaluativo de **Principios y Patrones de Diseño**.

El objetivo es demostrar cómo los patrones de diseño GoF y los principios SOLID permiten transformar una implementación altamente acoplada en una solución:

- Extensible.
- Mantenible.
- Desacoplada.
- Auditable.
- Orientada a objetos.

La solución permite que `GestorCandidato` coordine el proceso sin conocer directamente los detalles concretos de las etapas, los destinatarios de las notificaciones ni la lógica interna necesaria para deshacer una operación.

---

# 👨‍💻 Autor

**David L.**

GitHub: [davidl20](https://github.com/davidl20)

Repositorio: [HireCore](https://github.com/davidl20/HireCore)

