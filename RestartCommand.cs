namespace FlappyBird;

public class RestartCommand : IGameCommand
{
    public void Execute(GameEngine game)
    {
        game.Restart();
    }
}