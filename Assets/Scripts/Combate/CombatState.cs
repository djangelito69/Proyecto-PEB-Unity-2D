using UnityEngine;

/// <summary>
/// Define los posibles estados del combate.
/// Cada estado representa una fase clara del flujo de combate.
/// </summary>
public enum CombatState
{
    Idle,              // Esperando inicio
    PlayerTurn,        // Jugador puede elegir acción
    ExecutingAction,   // Una acción se está ejecutando (con retrasos)
    EnemyTurn,         // Turno del enemigo
    Victory,           // Jugador ganó
    Defeat,            // Jugador perdió
    Paused             // Combate en pausa
}

/// <summary>
/// Máquina de estados simple para el combate.
/// Gestiona transiciones y asegura que solo ocurran en el momento adecuado.
/// </summary>
public class CombatStateMachine
{
    private CombatState _currentState = CombatState.Idle;
    public CombatState CurrentState => _currentState;

    // Eventos para que otros sistemas se entere de cambios de estado
    public System.Action<CombatState, CombatState> OnStateChanged;

    public void SetState(CombatState newState)
    {
        // ⚠️ CAMBIO CRÍTICO: Se eliminó la verificación "if (_currentState == newState) return;"
        // Esa línea causaba que las transiciones redundantes se ignoraran
        // Lo que provocaba que los botones quedaran bloqueados

        CombatState previousState = _currentState;
        _currentState = newState;

        Debug.Log($"[COMBATE] Estado: {previousState} → {newState}");
        OnStateChanged?.Invoke(previousState, newState);
    }

    public bool IsInState(CombatState state) => _currentState == state;
    public bool CanPlayerAct() => _currentState == CombatState.PlayerTurn;
    public bool IsActionExecuting() => _currentState == CombatState.ExecutingAction;
}