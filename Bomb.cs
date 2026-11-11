namespace FlappyBird;

public class Bomb : WorldObject
{
    public Bomb(float x, float y) : base(x, y, 30f, 30f) { }
    
    public override void OnHit(Bird bird)
    {
        if (bird.HasShield)
        {
            bird.DeactivateShield();
        }
        else
        {
            bird.Kill();
        }

        IsFinished = true;
    }
}