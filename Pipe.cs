namespace FlappyBird;

public class Pipe : WorldObjects
{
    public const float GapHeight = 150f;

    public Pipe(float x, float y, float width = 50f, float height = 600f) : base(x, y, width, height) { }

    public bool IsPassed { get; protected set; }

    public float GapTop => Y - GapHeight / 2f;
    public float GapBottom => Y + GapHeight / 2f;
    
    public override void OnHit(Bird bird)
    {
        if (bird.HasShield )
        {
            bird.DeactivateShield();
            IsFinished = true;
        }
        else
        {
            bird.Kill();
        }
    }

    public override bool CheckCol(Bird bird)
    {
        bool birdInsidePipe = bird.Right > Left && bird.Left < Right;
        bool collidingByY = bird.Top < GapTop || bird.Bottom > GapBottom;

        return collidingByY && birdInsidePipe;
    }
    
    public void MarkPassed()
    {
        IsPassed = true;
    }

    public override int PointsWhenPassed() => 1;

    public override int TryGetPoint(Bird bird)
    {
        if (IsPassed)
        {
            return 0;
        }
        if (bird.Left > Right || IsFinished)
        {
            MarkPassed();
            return PointsWhenPassed();
        }

        return 0;
    }
}