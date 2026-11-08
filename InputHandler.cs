using System.Diagnostics;
using SFML.Window;

namespace FlappyBird;

public class InputHandler
{
    public IGameCommand? Handle(Keyboard.Key key)
    {
        return key switch
        {
            Keyboard.Key.Space => new FlapCommand(),
            Keyboard.Key.R => new RestartCommand(),
            _ => null
        };
    }
}