using UniRx;

public interface IGameStateProvider
{
    IReadOnlyReactiveProperty<GameState> CurrentState { get; }
}