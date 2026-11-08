namespace FlappyBird;

public class GameOverState : IGameState
{
    public GameState Type => GameState.GameOver;
    
    public void Flap(GameEngine game)
    {
        game.Start();
    }

    public void Restart(GameEngine game)
    {
        game.Start();
    }
}