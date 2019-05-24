using System;
using SplashKitSDK;

public class Reward2 : AI, IMovable
{
    public Reward2()
    {
        SpaceShipBitmap = SplashKit.BitmapNamed("Reward2");
        Y = -SpaceShipBitmap.Height;
    }

    public override void Move()
    {
        Y += Speed * 2;
    }
}