namespace FlappyBird;

public class Bird
{
    private float Y;
    private float SpeedY;
    private float Width;
    private float Height;
    private bool HasShield;

    private const float X = 100f;
    private const float Gravity = 900f;
    private const float JumpSpeed = 300f;
    private const float MaxFallSpeed = 600f;
    private const float StartY = 300f;

    public float Top => Y;
    public float Bottom => Y + Height;

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
        SpeedY = Gravity * dt;
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

    }
}