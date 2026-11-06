namespace FlappyBird;

public class FlapCommand : IGameCommand
{
    public void Execute(GameEngine game)
    {
        if (game.State == GameState.GameOver || game.State == GameState.Menu)
        {
            game.Start();
        }
        else if (game.State == GameState.Playing)
        {
            game.Flap();
        }
    }
}