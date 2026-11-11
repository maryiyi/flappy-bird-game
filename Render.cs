using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace FlappyBird;

public class Renderer
{
    public void Run(GameEngine game)
    {
        var window = new RenderWindow(new VideoMode(new Vector2u((uint)GameEngine.WorldWidth, (uint)GameEngine.WorldHeight)), "Flappy Bird");
        window.SetFramerateLimit(60);
        window.SetKeyRepeatEnabled(false);

        var input = new InputHandler();
        
        window.Closed += (sender, e) => window.Close();
        window.KeyPressed += (sender, e) =>
        {
            input.Handle(e.Code)?.Execute(game);
        };

        Font? font = null;
        try
        {
            font = new Font("Assets/flappyFont.ttf");
        }
        catch (Exception ex) 
        {
            Console.WriteLine($"can't set the font: {ex}");    
        }
        
        Texture? birdTexture = null;
        try
        { 
            birdTexture = new Texture("Assets/bird.png");
        }
        catch (Exception ex) 
        {
            Console.WriteLine($"texture bird error: {ex}");    
        }

        Sprite? birdSprite = null;

        if (birdTexture != null)
        {
            birdSprite = new Sprite(birdTexture)
            {
                Scale = new Vector2f(
                    game.Bird.Width / birdTexture.Size.X,
                    game.Bird.Height / birdTexture.Size.Y
                )
            };
        }
        
        var clock = new Clock();

        while (window.IsOpen)
        {
            window.DispatchEvents();
            float dt = clock.Restart().AsSeconds();
            
            game.Update(dt);

            window.Clear(new Color(135, 206, 250));
            DrawWorld(window, game, font, birdSprite);

            window.Display();
        }
    }
    private void DrawWorld(RenderWindow window, GameEngine game, Font? font, Sprite? birdSprite)
    {
        foreach (var obstacle in game.ObjectsList)
        {
            if (obstacle.IsFinished) continue;

            if (obstacle is Pipe pipe)
            {
                var topPipe = new RectangleShape(new Vector2f(pipe.Width, pipe.GapTop))
                {
                    Position = new Vector2f(pipe.X, 0f),
                    FillColor = new Color(115, 190, 45),
                    OutlineColor = Color.Black,
                    OutlineThickness = 3f
                };
                window.Draw(topPipe);

                float bottomHeight = GameEngine.GroundY - pipe.GapBottom;
                if (bottomHeight > 0)
                {
                    var bottomPipe = new RectangleShape(new Vector2f(pipe.Width, bottomHeight))
                    {
                        Position = new Vector2f(pipe.X, pipe.GapBottom),
                        FillColor = new Color(115, 190, 45),
                        OutlineColor = Color.Black,
                        OutlineThickness = 3f
                    };
                    window.Draw(bottomPipe);
                }
            }
            else if (obstacle is Bomb bomb)
            {
                var bombShape = new CircleShape(bomb.Width / 2f)
                {
                    Position = new Vector2f(bomb.X, bomb.Y),
                    FillColor = Color.Black,
                    OutlineColor = Color.Red,
                    OutlineThickness = 4f
                };
                window.Draw(bombShape);
            }
            else if (obstacle is ShieldBonus shield)
            {
                var shieldShape = new CircleShape(shield.Width / 2f)
                {
                    Position = new Vector2f(shield.X, shield.Y),
                    FillColor = new Color(0, 255, 255, 70), 
                    OutlineColor = Color.Cyan,
                    OutlineThickness = 2f
                };
                window.Draw(shieldShape);
            }
        }

        var ground = new RectangleShape(new Vector2f(GameEngine.WorldWidth, GameEngine.WorldHeight - GameEngine.GroundY))
        {
            Position = new Vector2f(0f, GameEngine.GroundY),
            FillColor = new Color(220, 200, 130),
            OutlineColor = new Color(100, 80, 40),
            OutlineThickness = 4f
        };
        window.Draw(ground);

        var bird = game.Bird;

        if (bird.HasShield)
        {
            var shieldAura = new CircleShape(30f)
            {
                Position = new Vector2f(bird.X - 3f, bird.Y - 5f),
                FillColor = new Color(0, 255, 255, 70), 
                OutlineColor = Color.Cyan,
                OutlineThickness = 2f
            };
            window.Draw(shieldAura);
        }

        if (birdSprite != null)
        {
            birdSprite.Position = new Vector2f(game.Bird.X, game.Bird.Y);
        }
        window.Draw(birdSprite);
        
        
        if (font != null)
        {
            var scoreText = new Text(font, $" {game.Score}", 22)
            {
                Position = new Vector2f((GameEngine.WorldWidth/2)-10, 10f),
                FillColor = Color.White,
                OutlineColor = Color.Black,
                OutlineThickness = 4f
            };
            window.Draw(scoreText);

            var bestText = new Text(font, $"Best: {game.HighScore}", 18)
            {
                Position = new Vector2f(10f, 10f),
                FillColor = Color.Yellow,
                OutlineColor = Color.Black,
                OutlineThickness = 1.5f
            };
            window.Draw(bestText);

            if (game.Bird.HasShield)
            {
                var shieldText = new Text(font, "SHIELD ACTIVE!", 16)
                {
                    Position = new Vector2f(10f, 36f),
                    FillColor = Color.Cyan,
                    OutlineColor = Color.Black,
                    OutlineThickness = 1.5f
                };
                window.Draw(shieldText);
            }

            if (game.State == GameState.Menu)
            {
                var menuText = new Text(font, "Press [Space] to Start", 25)
                {
                    Position = new Vector2f(64f, 220f),
                    FillColor = Color.White,
                    OutlineColor = Color.Black,
                    OutlineThickness = 3f
                };
                window.Draw(menuText);
            }

            if (game.State == GameState.GameOver)
            {
                var overText = new Text(font, "GAME OVER", 48)
                {
                    Position = new Vector2f(82f, 220f),
                    FillColor = Color.Red,
                    OutlineColor = Color.Black,
                    OutlineThickness = 3f
                };
                window.Draw(overText);

                var restartText = new Text(font, "Press [Space] to Restart", 18)
                {
                    Position = new Vector2f(91f, 275f),
                    FillColor = Color.White,
                    OutlineColor = Color.Black,
                    OutlineThickness = 2f
                };
                window.Draw(restartText);
            }
        }
    }
}