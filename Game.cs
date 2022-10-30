using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

using QuadroEngine.UI;

using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;
using VelcroPhysics.Shared;
using VelcroPhysics.Collision.Filtering;
using Microsoft.Xna.Framework;
using VelcroPhysics.Utilities;

namespace QuadroEngine
{
    public enum Theme
    {
        Dark,
        Light
    };
    public class Game
    {
        public RenderWindow App;
        
        private Clock DTime = new Clock();
        public float DeltaTime = 0, FPS = 0, MaxFPS = 0;
        private Text fps = new Text();
        private Text maxfps = new Text();
        private UI.Window physdeb;
        private Slider tm;
        private CheckBox ts;
        private Notification notification;

        private RectangleShape rectfps;
        private Clock fpstimer = new Clock();
        private Clock JumpTime = new Clock();
        public Entity Camera = new Entity();
        public GameObject character;

        public List<Font> Fonts;
        public List<UI_Element> UIs;
        public List<Layer> Layers;
        public List<GameObject> gameObjects;
        
        public Color Background = new Color(255, 255, 255);
        public bool FPSDraw = true, GravityEnable = true;
        public Theme theme = Theme.Dark;

        Animator CamMovement;
        Vector2f center = new Vector2f();
        Animation Expl;

        // Velcro
        public World world;
        public float TimeMultiply = 1;
        public Game(string title, uint width, uint height)
        {
            ContextSettings settings = new ContextSettings(1, 0, 4);
            App = new RenderWindow(new VideoMode(width, height), title, Styles.Default, settings);
            App.SetFramerateLimit(145);

            Initialize();
        }

        public Game(string title)
        {
            ContextSettings settings = new ContextSettings(1, 0, 1);
            App = new RenderWindow(new VideoMode(VideoMode.DesktopMode.Width, VideoMode.DesktopMode.Height), title, Styles.Fullscreen, settings);

            Initialize();
        }

        public void Update()
        {
            DeltaTime = DTime.ElapsedTime.AsSeconds();
            DTime.Restart();

            App.DispatchEvents();
            App.Clear(Background);

            Input();

            TimeMultiply = tm.Value;

            world.Step(DeltaTime * TimeMultiply);

            foreach (GameObject go in gameObjects)
            {
                if (go != null)
                    go.Update(DeltaTime, Camera);
            }

            foreach (UI_Element ui in UIs)
            {
                if (ui != null)
                    ui.Update(DeltaTime);
            }

            foreach (Layer lr in Layers)
            {
                if (lr != null)
                    App.Draw(lr);
            }

            center = new Vector2f(character.WorldPosition.X - App.Size.X / 2, character.WorldPosition.Y - App.Size.Y / 2);

            if (Camera.Position != center)
            {
                Camera.Position = CamMovement.Lerp(Camera.Position, center, DeltaTime * 4);
            }

            //App.SetTitle(character.WorldPosition.X.ToString() + " " + character.WorldPosition.Y.ToString());

            DrawFPS();

            App.Display();
            FPS++;
        }

        private void Input()
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.A))
            {
                character.body.ApplyForce(new Vector2(-5, 0));
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.D))
            {
                character.body.ApplyForce(new Vector2(5, 0));
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.W) && character.body.LinearVelocity.Y < 0.0005f && character.body.LinearVelocity.Y >= 0)
            {
                //JumpTime.Restart();
                character.body.ApplyForce(new Vector2(0, -270));
            }

        }

        private void DrawFPS()
        {
            // FPS counter
            if (FPS > MaxFPS)
            {
                MaxFPS = FPS;
                maxfps.DisplayedString = "Max: " + MaxFPS.ToString();
            }
            if (fpstimer.ElapsedTime.AsSeconds() >= 1f)
            {
                fps.DisplayedString = "FPS: " + FPS.ToString();
                Console.Log("FPS:", FPS.ToString());
                FPS = 0;
                fpstimer.Restart();
            }
            if (fps.GetLocalBounds().Width < maxfps.GetLocalBounds().Width)
            {
                rectfps.Size = new Vector2f(maxfps.GetLocalBounds().Width + 5,
                    fps.GetLocalBounds().Height + maxfps.GetLocalBounds().Height + 8);
            }
            else
            {
                rectfps.Size = new Vector2f(fps.GetLocalBounds().Width + 5,
                    fps.GetLocalBounds().Height + maxfps.GetLocalBounds().Height + 8);
            }
            if (FPSDraw)
            {
                App.Draw(rectfps);
                App.Draw(fps);
                App.Draw(maxfps);
            }
        }

        private void Initialize()
        {
            Fonts = new List<Font>();
            Layers = new List<Layer>();
            UIs = new List<UI_Element>();
            gameObjects = new List<GameObject>();

            world = new World(new Vector2(0, 20));
            ConvertUnits.SetDisplayUnitToSimUnitRatio(64f);

            character = new GameObject
            {
                body = BodyFactory.CreateRectangle(world, 
                ConvertUnits.ToSimUnits(20), 
                ConvertUnits.ToSimUnits(50), 
                1f, 
                ConvertUnits.ToSimUnits(new Vector2(50, 50)), 
                0, 
                BodyType.Dynamic),
                shape = new RectangleShape
                {
                    Origin = new Vector2f(10, 25),
                    Size = new Vector2f(20, 50),
                    FillColor = Color.Black
                }
            };

            GameObject ground = new GameObject
            {
                body = BodyFactory.CreateRectangle(world, 
                ConvertUnits.ToSimUnits(300), 
                ConvertUnits.ToSimUnits(20), 
                1f,
                ConvertUnits.ToSimUnits(new Vector2(20, 300))),
                shape = new RectangleShape
                {
                    Origin = new Vector2f(150, 10),
                    Size = new Vector2f(300, 20),
                    FillColor = Color.Black
                }
            };
            ground.body.Friction = 1;

            GameObject ground2 = new GameObject
            {
                body = BodyFactory.CreateRectangle(world,
                ConvertUnits.ToSimUnits(7000),
                ConvertUnits.ToSimUnits(20),
                1f,
                ConvertUnits.ToSimUnits(new Vector2(0, 450))),
                shape = new RectangleShape
                {
                    Origin = new Vector2f(3500, 10),
                    Size = new Vector2f(7000, 20),
                    FillColor = Color.Black
                }
            };
            ground2.body.Friction = 1;

            GameObject ground3 = new GameObject
            {
                body = BodyFactory.CreateRectangle(world,
                ConvertUnits.ToSimUnits(20),
                ConvertUnits.ToSimUnits(400),
                1f,
                ConvertUnits.ToSimUnits(new Vector2(-360, 260))),
                shape = new RectangleShape
                {
                    Origin = new Vector2f(10, 200),
                    Size = new Vector2f(20, 400),
                    FillColor = Color.Black
                }
            };
            ground3.body.Friction = 1;

            GameObject ground4 = new GameObject
            {
                body = BodyFactory.CreateRectangle(world,
                ConvertUnits.ToSimUnits(20),
                ConvertUnits.ToSimUnits(400),
                1f,
                ConvertUnits.ToSimUnits(new Vector2(360, 260))),
                shape = new RectangleShape
                {
                    Origin = new Vector2f(10, 200),
                    Size = new Vector2f(20, 400),
                    FillColor = Color.Black
                }
            };
            ground4.body.Friction = 1;

            Layer l = new Layer();

            for (int i = 0; i < 1000; i++)
            {
                GameObject box = new GameObject
                {
                    body = BodyFactory.CreateRectangle(world,
                    ConvertUnits.ToSimUnits(50),
                    ConvertUnits.ToSimUnits(50),
                    0.05f,
                    ConvertUnits.ToSimUnits(new Vector2(110 + i * 5, 160 - i * 100)),
                    0,
                    BodyType.Dynamic),
                    sprite = new Sprite
                    {
                        Texture = new Texture(@"resources\WoodenCrate.png"),
                        Origin = new Vector2f(25, 25)
                    }
                };
                box.body.Friction = 1;
                gameObjects.Add(box);
                l.Objects.Add(box);
            }

            character.body.FixedRotation = true;
            gameObjects.Add(character);
            gameObjects.Add(ground);
            gameObjects.Add(ground2);
            gameObjects.Add(ground3);
            gameObjects.Add(ground4);
            
            Layers.Add(l);
            l.Objects.Add(character);
            l.Objects.Add(ground);
            l.Objects.Add(ground2);
            l.Objects.Add(ground3);
            l.Objects.Add(ground4);

            //Expl = new Animation(new Texture("resources/Explosion.png"), new IntRect(0, 0, 96, 96), 100, 12, true);
            //Expl.WorldPosition = new Vector2f(-280, -80);
            //gameObjects.Add(Expl);
            //l.Objects.Add(Expl);

            Fonts.Add(new Font("font.ttf"));

            UIs.Add(new Button
            {
                Position = new Vector2f(40, 660),
                Label = new Text("Close", Fonts[0], 14),
                Size = new Vector2f(100, 40),
                action = Close
            });
            l.Objects.Add(UIs[0]);

            physdeb = new UI.Window
            {
                Position = new Vector2f(40, 50),
                Size = new Vector2f(200, 100),
                Name = new Text("Physics debug", Fonts[0], 14)
            };

            tm = new Slider
            {
                SurfacePos = new Vector2f(0, 35),
                Attribute = UI.Attribute.CenterHorizontal,
                ValueFrom = -1,
                ValueTo = 1,
                Value = 1,
                Size = new Vector2f(100, 20),
                DrawValue = true,
                ValueText = new Text("", Fonts[0], 14)
            };

            Text tstext = new Text("Stop Time", Fonts[0], 14);
            ts = new CheckBox(new Vector2f(0, 0), tstext, new Vector2f(15, 60), TimeStop, TimeRelease);

            notification = new Notification("я твою мать ебал", "нет я твою, хуесос", Fonts[0], new Vector2f(1280, 720));
            UIs.Add(notification);

            physdeb.Elements.Add(tm);
            physdeb.Elements.Add(ts);

            UIs.Add(physdeb);
            l.Objects.Add(physdeb);
            l.Objects.Add(notification);

            fps = new Text("", Fonts[0], 14);

            maxfps = new Text("", Fonts[0], 12);
            maxfps.Position = new Vector2f(0, 15);

            rectfps = new RectangleShape();
            rectfps.FillColor = new Color(0, 0, 0, 120);

            CamMovement = new Animator();
            CamMovement.Time = 1f;
            CamMovement.To = 1;

            App.Closed += App_Closed;
            App.KeyPressed += App_KeyPressed;
            App.MouseButtonPressed += App_MouseButtonPressed;
            App.MouseButtonReleased += App_MouseButtonReleased;
            App.MouseMoved += App_MouseMoved;
            App.MouseWheelScrolled += App_MouseWheelScrolled;
            App.TextEntered += App_TextEntered;
            App.Resized += App_Resized;
            App.KeyReleased += App_KeyReleased;
        }

        public void TimeStop()
        {
            TimeMultiply = 0;
        }

        public void TimeRelease()
        {
            TimeMultiply = 1;
        }

        private void App_KeyReleased(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.E)
                FPSDraw = !FPSDraw;
        }

        public void Close()
        {
            Console.LogToFile();
            App.Close();
        }

        private void App_Resized(object sender, SizeEventArgs e)
        {
            foreach (UI_Element ui in UIs)
            {
                if (ui != null)
                    ui.Resized(e);
            }

            App.SetView(new View(new FloatRect(0, 0, e.Width, e.Height)));
            center = new Vector2f(character.WorldPosition.X - e.Width / 2, character.WorldPosition.Y - e.Height / 2);
            UIs[0].Position = new Vector2f(20, (int)e.Height - 60);
        }

        private void App_TextEntered(object sender, TextEventArgs e)
        {
            foreach (UI_Element ui in UIs)
            {
                if (ui != null)
                    ui.TextEntered(e);
            }
        }

        private void App_MouseWheelScrolled(object sender, MouseWheelScrollEventArgs e)
        {
            foreach (UI_Element ui in UIs)
            {
                if (ui != null)
                    ui.MouseWheel(e.Delta);
            }
        }

        private void App_MouseMoved(object sender, MouseMoveEventArgs e)
        {
            foreach (UI_Element ui in UIs)
            {
                if (ui != null)
                    ui.MouseCheck(e);
            }
            
            foreach (GameObject go in gameObjects)
            {
                if (go != null)
                    go.MoveToMouse(e, Camera);
            }

            if (tm.MouseHoverSlider)
            {
                TimeMultiply = tm.Value;
            }
        }

        private void App_MouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            foreach (UI_Element ui in UIs)
            {
                if (ui != null)
                    ui.MouseRelease(e);
            }
        }

        private void App_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            foreach (UI_Element ui in UIs)
            {
                if (ui != null)
                    ui.MouseClick(e);
            }

            //foreach (GameObject go in gameObjects)
            //{
            //    go.Click(e, Camera);
            //}
        }

        private void App_KeyPressed(object sender, KeyEventArgs e)
        {
            foreach (UI_Element ui in UIs)
            {
                if (ui != null)
                    ui.KeyPressed(e);
            }
        }

        private void App_Closed(object sender, EventArgs e)
        {
            Console.LogToFile();
            App.Close();
        }
    }
}
