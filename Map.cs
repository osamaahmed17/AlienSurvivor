using System;
using SplashKitSDK;

public class Map : IMovable
{
    private Window _gameWindow;
    private Bitmap _spaceBitmap;

    private Timer _myTimer;

    public const int LANE_LEFT = 0;
    public const int LANE_WIDTH = 200;

    public Map(Window window)
    {
        
        _gameWindow = window;
        _myTimer = new Timer("timer");
        _myTimer.Start();
        
    }
    public void Move()
    {
        SpaceMove();
    }
    public void Draw()
    {
        _spaceBitmap.Draw((_gameWindow.Width - _spaceBitmap.Width) / 2, 0);
       
    }

    public void SpaceMove()
    {
        if (_myTimer.Ticks < 200)
        {
            _spaceBitmap = SplashKit.BitmapNamed("Space");
        }
     
        if (_myTimer.Ticks >= 600)
        {
            _myTimer.Start();
        }
    }


}