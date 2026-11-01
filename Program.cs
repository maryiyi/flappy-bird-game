namespace FlappyBird;

class Program
{
    static void Main(string[] args)
    {
        var engine = new GameEngine();
        
        engine.Start();
        Console.WriteLine($"after Start(): {engine.State}");

        for (int i = 0; i < 5; i++)
        {
            engine.Update(0.1f);
            Console.WriteLine($"shot {i + 1}: bird Y = {engine.Bird.Y}, velocity = {engine.Bird.SpeedY}");
        }

        engine.Flap();
        Console.WriteLine($"after Flap(): velocity = {engine.Bird.SpeedY}");

        for (int i = 0; i < 25; i++)
        {
            if (i % 5 == 0)
            {
                engine.Flap();
            }
            engine.Update(0.1f);
        }
        Console.WriteLine($"obstacles count: {engine.Obstacles.Count}");
    }
}