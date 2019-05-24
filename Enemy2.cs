using System;
using SplashKitSDK;

public class Enemy2 : AI, IMovable
{
    public Enemy2()
    {
        SpaceShipBitmap = SplashKit.BitmapNamed("Enemy2");
        Y = -SpaceShipBitmap.Height;
    }

    public override void Move()
    {
        Y += Speed / 3 * 4;
    }
}