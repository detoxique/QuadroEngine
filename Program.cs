namespace QuadroEngine
{
    class Program
    {
        public static Game game;
        static void Main(string[] args)
        {
            game = new Game("Quadro Engine", 1280, 720);

            while (game.App.IsOpen)
            {
                game.Update();
            }
        }
    }
}
