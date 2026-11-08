namespace FlappyBird;

public class PlayingState : IGameState
{
    public GameState Type => GameState.Playing;
    
    public void Flap(GameEngine game)
    {
        game.Flap();
    }

    public void Restart(GameEngine game)
    {
        game.Start();
    }
}