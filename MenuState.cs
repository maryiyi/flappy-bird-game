namespace FlappyBird;

public class MenuState : IGameState
{
    public GameState Type => GameState.Menu;
    
    public void Flap(GameEngine game)
    {
        game.Start();
    }

    public void Restart(GameEngine game)
    {
        game.Start();
    }
}