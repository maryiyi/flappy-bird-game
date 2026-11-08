namespace FlappyBird;

public interface IGameState
{
    GameState Type { get; }
    void Flap(GameEngine game);
    void Restart(GameEngine game);

}