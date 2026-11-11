namespace FlappyBird;

public class GameEngine
{
    public GameState State => _state.Type;
    public int Score { get; private set; }
    public int HighScore { get; private set; }
    public Bird Bird { get; }

    private readonly List<WorldObject> _objectsList = new();
    public IReadOnlyList<WorldObject> ObjectsList => _objectsList;
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
        SaveScore();
        Bird.Reset();
        _objectsList.Clear();
        Score = 0;
        _spawnTimer = 0f;
        ChangeState(new PlayingState());
    }

    public void Update(float dt)
    {
        dt = Math.Min(dt, 0.05f);

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
            SpawnObjects();
        }

        foreach (var obstacle in _objectsList)
        {
            obstacle.Move(dt);
        }

        CheckHit();

        for (int i = _objectsList.Count - 1; i >= 0; i--)
        {
            var worldObject = _objectsList[i];
            Score += worldObject.TryGetPoint(Bird);

            if (worldObject.IsOffScreen || worldObject.IsFinished)
            {
                _objectsList.RemoveAt(i);
            }
        }
    }

    public void Flap()
    {
        _state.Flap(this);
    }

    internal void GameFlap()
    {
        Bird.Flap();
    }

    private void CheckHit()
    {
        foreach (var worldObjects in _objectsList)
        {
            if (worldObjects.IsFinished) continue;
            if (worldObjects.CheckCol(Bird))
            {
                worldObjects.OnHit(Bird);
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
        SaveScore();
    }

    private void SaveScore()
    {
        if (Score > HighScore)
        {
            HighScore = Score;
            _fileManager.SaveHighScore(HighScore);
        }
    }

private void SpawnObjects()
    {
        float CenterY = _random.Next(150, 390);
        _objectsList.Add(new Pipe(WorldWidth, CenterY));

        int chance = _random.Next(100);
        if (chance < 30)
        {
            _objectsList.Add(new ShieldBonus(WorldWidth + 200f, CenterY));
        }
        else if (chance < 40)
        {
            _objectsList.Add(new Bomb(WorldWidth + 200f, CenterY));
        }
    }

    private void ChangeState(IGameState newState)
    {
        _state = newState;
    }
}