using System;
using SplashKitSDK;

public class Reward1 : AI, IMovable
{
    public Reward1()
    {
        SpaceShipBitmap = SplashKit.BitmapNamed("Reward1");
        Y = -SpaceShipBitmap.Height;
    }

    public override void Move()
    {
        Y += Speed * 2;
    }
}