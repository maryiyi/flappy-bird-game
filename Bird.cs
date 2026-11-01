namespace FlappyBird;

public class Bird
{
    public float Y { get; private set; }
    public float SpeedY { get; private set; }
    public float Width { get; }
    public float Height { get; }
    public bool HasShield { get; private set; }
    public bool IsAlive { get; private set; }

    public float X => 100f;  
    private const float Gravity = 900f;
    private const float JumpSpeed = 300f;
    private const float MaxFallSpeed = 600f;
    private const float StartY = 300f;

    public float Top => Y;
    public float Bottom => Y + Height;
    public float Left => X;
    public float Right => X + Width;

    public Bird(float width, float height)
    {
        Width = width;
        Height = height;
        Reset();
    }

    public void Flap()
    {
        SpeedY = -JumpSpeed;
    }
    public void Fall(float dt)
    {
        SpeedY += Gravity * dt;
        if (SpeedY > MaxFallSpeed)
        {
            SpeedY = MaxFallSpeed;
        }

        Y += SpeedY * dt;
    }
    public void ActivateShield()
    {
        HasShield = true;
    }
    public void DeactivateShield()
    {
        HasShield = false;
    }
    public void Reset()
    {
        Y = StartY;
        SpeedY = 0;
        DeactivateShield();
        IsAlive = true;
    }
    public void Kill() => IsAlive = false;
}