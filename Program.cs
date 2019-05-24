using System;
using SplashKitSDK;

public class Program
{
    public static void Main()
    {
        Window _window = new Window("SpaceGame", 800, 800);
        SpaceGame _SpaceGame = new SpaceGame(_window);

        while (!_window.CloseRequested && !_SpaceGame.ESC)
        {
            if (_SpaceGame.Restart)
            {
                _SpaceGame = new SpaceGame(_window);
            }
            SplashKit.ProcessEvents();
            _window.Clear(Color.RGBColor(193, 154, 107));
            _SpaceGame.Update();
            _SpaceGame.Draw();
            _window.Refresh();
        }
        _window.Close();
        _window = null;
    }
}

public interface IMovable
{
    void Move();
}