namespace FlappyBird;

public class GameEngine
{
    public GameState State { get; private set; }
    public int Score { get; private set; }
    public int HighScore { get; private set; }
    public Bird Bird { get; }
    
    private readonly List<Obstacles> _obstacles { get; }
    public IReadOnlyList<Obstacles> Obstacles { get; }
    private FileManager _fileManager;

    private const float WorldWidth = 400f;
    private const float WorldHeight = 600f;
    private const float GroundY = 540f;

    public GameEngine()
    {
        Bird bird = new Bird(34, 31);
        _fileManager = new FileManager();
        
        HighScore = _fileManager.LoadHighScore();
        State = GameState.Menu;
    }

    public void Start()
    {
        if (State == GameState.Playing) return;
        Bird.Reset();
        _obstacles.Clear();
        Score = 0;
        State = GameState.Playing;
    }
    private void Update()
    {
        if (State == GameState.Playing) return;
        Bird.Fall(float dt);

        if (Bird.Bottom >= GroundY || Bird.Top <= 0)
        {
            GameOver();
        }
    }

    public void Flap()
    {
        Bird.Flap();
    }

    public void CheckHit()
    {
        throw new NotImplementedException();
    }

    public void GameOver()
    {
        State = GameState.GameOver;
        if (Score > HighScore)
        {
            HighScore = Score;
            _fileManager.SaveHighScore(HighScore);
        }
    }
}