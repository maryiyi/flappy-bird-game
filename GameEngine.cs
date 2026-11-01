namespace FlappyBird;

public class GameEngine
{
    public GameState State { get; private set; }
    public int Score { get; private set; }
    public int HighScore { get; private set; }
    public Bird Bird { get; } = new Bird(34, 31);
    
    private readonly List<Obstacle> _obstacles = new();
    public IReadOnlyList<Obstacle> Obstacles => _obstacles;

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
    public void Update(float dt)
    {
        if (State != GameState.Playing) return;
        Bird.Fall(dt);

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
        foreach (var obstacle in _obstacles)
        {
            if (obstacle.IsFinished) continue;
            if (obstacle.CheckCol(Bird))
            {
                obstacle.OnHit(Bird);
                if (!Bird.IsAlive)
                {
                    GameOver();
                    break;
                }
            }
        }
    }

    private void GameOver()
    {
        State = GameState.GameOver;
        if (Score > HighScore)
        {
            HighScore = Score;
            _fileManager.SaveHighScore(HighScore);
        }
    }
}