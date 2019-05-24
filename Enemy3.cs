using System;
using SplashKitSDK;

public class Enemy3 : AI, IMovable
{
    public Enemy3()
    {
        SpaceShipBitmap = SplashKit.BitmapNamed("Enemy3");
        Y = -SpaceShipBitmap.Height;
    }

    public override void Move()
    {
        Y += Speed / 3 * 5;
    }
}