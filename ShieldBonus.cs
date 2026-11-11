namespace FlappyBird;

public class ShieldBonus : WorldObject
{
    public ShieldBonus(float x, float y) : base(x, y, 32f, 32f) { }
    
    public override void OnHit(Bird bird)
    {
        bird.ActivateShield();
        IsFinished = true;
    }
}