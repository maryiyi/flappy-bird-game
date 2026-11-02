namespace FlappyBird;

class Program
{
    static void Main(string[] args)
    {
        var game = new GameEngine();
        var renderer = new Renderer();

        renderer.Run(game);
    }
}