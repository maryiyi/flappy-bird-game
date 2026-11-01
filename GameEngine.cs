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
    private readonly Random _random = new Random();
    private float _spawnTimer;

    private const float WorldWidth = 400f;
    private const float WorldHeight = 600f;
    private const float GroundY = 540f;
    private const float SpawnInterval = 2f;

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
        _spawnTimer = 0f;
        State = GameState.Playing;
    }
    public void Update(float dt)
    {
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


        for (int i = _obstacles.Count - 1; i >= 0; i--)
        {
            var obstacle = _obstacles[i];
            obstacle.Move(dt);
            
            if (obstacle is Pipe pipe && !pipe.IsPassed && Bird.Left > pipe.Right)
            {
                pipe.IsPassed = true;
                Score++;
            }
            if (obstacle.IsOffScreen || obstacle.IsFinished)
            {
                _obstacles.RemoveAt(i);
            }
        }
        CheckHit();
        
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
}