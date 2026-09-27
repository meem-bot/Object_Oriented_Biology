using SplashKitSDK;

/// <summary>
/// The main entry point for the Cellular Visualisation program.
/// </summary>
public class Program
{
    /// <summary>
    /// Sets up the game window and runs the main animation loop.
    /// </summary>
    public static void Main()
    {
        Window window = new Window("Object-Oriented Biology", 1200, 800);
        Director animationDirector = new Director();

        while (!window.CloseRequested)
        {
            SplashKit.ProcessEvents();
            SplashKit.ClearScreen(SplashKit.RGBAColor(10, 15, 25, 255));

            animationDirector.Update();
            animationDirector.Draw();

            SplashKit.RefreshScreen(60);
        }
    }
}