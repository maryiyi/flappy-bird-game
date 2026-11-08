namespace FlappyBird;

public class GameEngine
{
    public GameState State => _state.Type;
    public int Score { get; private set; }
    public int HighScore { get; private set; }
    public Bird Bird { get; }
    
    private readonly List<Obstacle> _obstacles = new();
    public IReadOnlyList<Obstacle> Obstacles => _obstacles;
    private readonly FileManager _fileManager;
    private readonly Random _random = new Random();
    private float _spawnTimer;
    private IGameState _state = new MenuState();

    public const float WorldWidth = 400f;
    public const float WorldHeight = 600f;
    public const float GroundY = 540f;
    public const float SpawnInterval = 2f;
    public const float BirdWidth = 51f;
    public const float BirdHeight = 36f;

    public GameEngine()
    {
        Bird = new Bird(BirdWidth, BirdHeight);
        _fileManager = new FileManager();
        
        HighScore = _fileManager.LoadHighScore();
        ChangeState(new MenuState());
    }

    public void Start()
    {
        Bird.Reset();
        _obstacles.Clear();
        Score = 0;
        _spawnTimer = 0f;
        ChangeState(new PlayingState());
    }
    public void Update(float dt)
    {
        dt = Math.Min(dt, 05f);
        
        if (State != GameState.Playing) return;
        Bird.Fall(dt);

        if (Bird.Bottom >= GroundY || Bird.Top <= 0)
        {
            GameOver();
            return;
            
        }
        
        _spawnTimer += dt;
        if (_spawnTimer >= SpawnInterval)
        {
            _spawnTimer = 0f;
            SpawnObstacle();
        }

        foreach (var obstacle in _obstacles)
        {
            obstacle.Move(dt);
        }

        CheckHit();
        
        for (int i = _obstacles.Count - 1; i >= 0; i--)
        {
            var obstacle = _obstacles[i];
            
            if (obstacle is Pipe pipe && !pipe.IsPassed)
            {
                if (Bird.Left > pipe.Right || pipe.IsFinished)
                {
                    pipe.MarkPassed();
                    Score+= obstacle.PointsWhenPassed();
                }
            }
            if (obstacle.IsOffScreen || obstacle.IsFinished)
            {
                _obstacles.RemoveAt(i);
            }
        }
    }

    public void Flap()
    {
        Bird.Flap();
    }

    private void CheckHit()
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
        ChangeState(new GameOverState());
        if (Score > HighScore)
        {
            HighScore = Score;
            _fileManager.SaveHighScore(HighScore);
        }
    }

    private void SpawnObstacle()
    {
        float CenterY = _random.Next(150, 390);
        _obstacles.Add(new Pipe(WorldWidth, CenterY));

        int chance = _random.Next(100);
        if (chance < 30)
        {
            _obstacles.Add(new ShieldBonus(WorldWidth + 200f, CenterY));
        }
        else if (chance < 40)
        {
            _obstacles.Add(new Bomb(WorldWidth + 200f, CenterY));
        }
    }

    public void ChangeState(IGameState newState)
    {
        _state = newState;
    }
}