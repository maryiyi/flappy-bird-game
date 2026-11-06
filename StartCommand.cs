namespace FlappyBird;

public class StartCommand : IGameCommand
{
    public void Execute(GameEngine game)
    {
        game.Start();
    }
}