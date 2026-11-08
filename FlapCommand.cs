namespace FlappyBird;

public class FlapCommand : IGameCommand
{
    public void Execute(GameEngine game)
    {
       game.Flap();
    }
}