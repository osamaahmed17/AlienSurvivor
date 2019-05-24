using System;
using SplashKitSDK;

public class Enemy1 : AI, IMovable
{
    public Enemy1()
    {
        SpaceShipBitmap = SplashKit.BitmapNamed("Enemy1");
        Y = -SpaceShipBitmap.Height;
    }

    public override void Move()
    {
        Y += Speed;
    }
}