namespace FlappyBird;

public interface IGameCommand
{
    void Execute(GameEngine game);
}