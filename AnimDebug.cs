using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Collections.Generic;

namespace QuadroEngine.UI
{
    class AnimDebug
    {
        private Sprite PlayButton;
        private Sprite PauseButton;
        private Sprite PrevFrame;
        public Window AnimDebugWindow;
        public Animation Target;

        private Button Play;

        public AnimDebug(Animation target, Font font)
        {
            Target = target;

            PlayButton = new Sprite(new Texture(@"resources\Play.png"));
            PauseButton = new Sprite(new Texture(@"resources\Pause.png"));
            PrevFrame = new Sprite(new Texture(@"resources\PrevFrame.png"));

            AnimDebugWindow = new Window
            {
                Name = new Text("Debug", font, 14),
                Position = new Vector2f(350, 150),
                Size = new Vector2f(300, 100),
                Labels = new List<TextW>()
            };
            AnimDebugWindow.Labels.Add(new TextW
            {
                DisplayedString = "Animation test",
                FillColor = Color.White,
                Font = font,
                CharacterSize = 14,
                SurfacePos = new Vector2f(10, 25),
                Attribute = Attribute.CenterHorizontal
            });

            AnimDebugWindow.Elements.Add(Play = new Button
            {
                Size = new Vector2f(20, 20),
                SurfacePos = new Vector2f(50, 50),
                Attribute = Attribute.CenterHorizontal,
                Icon = PauseButton,
                action = PlayPauseAnimation
            });

            AnimDebugWindow.Elements.Add(new Slider
            {
                Size = new Vector2f(200, 20),
                SurfacePos = new Vector2f(5, 80),
                Attribute = Attribute.CenterHorizontal,
                ValueFrom = 0,
                ValueTo = 100,
                Value = 75,
                font = font,
                AddText = " ms",
                TextPosition = TextPosition.RightOfSlider
            });
            if (Target != null)
            {
                AnimDebugWindow.Elements.Add(new Button
                {
                    Size = new Vector2f(20, 20),
                    SurfacePos = new Vector2f(165, 50),
                    Attribute = Attribute.None,
                    Icon = new Sprite(new Texture(@"resources\Play.png")),
                    action = Target.NextFrame
                });
                AnimDebugWindow.Elements.Add(new Button
                {
                    Size = new Vector2f(20, 20),
                    SurfacePos = new Vector2f(115, 50),
                    Attribute = Attribute.None,
                    Icon = PrevFrame,
                    action = Target.PreviousFrame
                });
            }
            else
            {
                AnimDebugWindow.Elements.Add(new Button
                {
                    Size = new Vector2f(20, 20),
                    SurfacePos = new Vector2f(165, 50),
                    Attribute = Attribute.None,
                    Icon = new Sprite(new Texture(@"resources\Play.png"))
                });
                AnimDebugWindow.Elements.Add(new Button
                {
                    Size = new Vector2f(20, 20),
                    SurfacePos = new Vector2f(115, 50),
                    Attribute = Attribute.None,
                    Icon = PrevFrame
                });
            }
        }

        public void Update(float DeltaTime)
        {
            if (Target != null)
            {
                if (Target.IsPlaying)
                {
                    Play.Icon = PauseButton;
                }
                else
                {
                    Play.Icon = PlayButton;
                }
            }
            AnimDebugWindow.Update(DeltaTime);
        }

        public void MouseCheck(MouseMoveEventArgs e)
        {
            AnimDebugWindow.MouseCheck(e);
        }

        public void MouseClick(MouseButtonEventArgs e)
        {
            AnimDebugWindow.MouseClick(e);
        }

        public void MouseRelease(MouseButtonEventArgs e)
        {
            AnimDebugWindow.MouseRelease(e);
        }

        public void MouseWheel(float Delta)
        {
            AnimDebugWindow.MouseWheel(Delta);
        }

        private void PlayPauseAnimation()
        {
            if (Target != null)
                Target.IsPlaying = !Target.IsPlaying;
        }

        public void Draw(Game game)
        {
            game.App.Draw(AnimDebugWindow);
        }
    }
}
