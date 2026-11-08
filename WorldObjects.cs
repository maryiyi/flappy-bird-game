namespace FlappyBird;

public abstract class WorldObjects
{
    public float X { get; protected set; }
    public float Y { get; protected set; }
    public float Width { get; protected set; }
    public float Height { get; protected set; }
    public bool IsFinished { get; protected set; }

    public bool IsOffScreen => X + Width < 0;
    protected const float Speed = 180f;

    public float Top => Y;
    public float Bottom => Y + Height;
    public float Left => X;
    public float Right => X + Width;

    protected WorldObjects(float x, float y, float width, float height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    public virtual void Move(float dt)
    {
        X -= Speed * dt;
    }

    public abstract void OnHit(Bird bird);

    public virtual bool CheckCol(Bird bird)
    {
        return bird.Right > Left && bird.Left < Right &&
               bird.Top < Bottom && bird.Bottom > Top;
    }

    public virtual int PointsWhenPassed() => 0;

    public virtual int TryGetPoint(Bird bird) => 0;
}