/////////////////////////Program.cs/////////////////////////////////////

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

//////////////////////////////////////////////////////////////////////////////

//////////////////////////////////////AI.cs///////////////////////////////////
using System;
using SplashKitSDK;

public abstract class AI : IMovable
{
    public Bitmap SpaceShipBitmap;
    public double X;
    public double Y;
    public double Speed;
    public int Lane;
    public bool IsOverLine;
    public AI()
    {
        double r = SplashKit.Rnd();
        if (r < 0.2)
        {
            X = Map.LANE_LEFT;
            Lane = 1;
        }
        if (r >= 0.2 && r < 0.4)
        {
            X = Map.LANE_LEFT + Map.LANE_WIDTH;
            Lane = 2;
        }
        if (r >= 0.4 && r < 0.6)
        {
            X = Map.LANE_LEFT + Map.LANE_WIDTH * 2;
            Lane = 3;
        }
        if (r >= 0.6 && r < 0.8)
        {
            X = Map.LANE_LEFT + Map.LANE_WIDTH * 3;
            Lane = 4;
        }
        if (r >= 0.8)
        {
            X = Map.LANE_LEFT + Map.LANE_WIDTH * 4;
            Lane = 5;
        }
    }

    public void Draw()
    {
        SpaceShipBitmap.Draw(X, Y);
    }

    public virtual void Move() { }

    public bool ColliedWith(Player p)
    {
        return SpaceShipBitmap.BitmapCollision(X, Y, p.SpaceShipBitmap, p.X, p.Y);
    }
}

//////////////////////////////////////////////////////////////////////////////

/////////////////////////////Enemy 1.cs/////////////////////////////////////
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
//////////////////////////////////////////////////////////////////////////////

/////////////////////////////Enemy 2.cs/////////////////////////////////////

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

//////////////////////////////////////////////////////////////////////
/////////////////////////////Enemy 3.cs/////////////////////////////////////
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

/////////////////////////////////////////////////////////////////////////
/////////////////////////////Map.Cs////////////////////////////////////

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
/////////////////////////////////////////////////////////////////////////
//////////////////////////////Player.cs////////////////////////////////////
using System;
using SplashKitSDK;

public class Player : IMovable
{
    private Window _gameWindow;
    public Bitmap SpaceShipBitmap;
    public double X;
    public double Y;
    public bool Quit;
    public delegate string GetBitmapName(Bitmap bitmap);

    GetBitmapName GetBitmap = delegate (Bitmap bitmap)
    {
        return SplashKit.BitmapName(bitmap);
    };

    public Player(Window window)
    {
        SpaceShipBitmap = SplashKit.BitmapNamed("Player1");
        _gameWindow = window;
        X = Map.LANE_LEFT + Map.LANE_WIDTH * 2;
        Y = _gameWindow.Height - SpaceShipBitmap.Height;
    }

    public void SwapPlayer(Bitmap p, GetBitmapName getname)
    {
        if (getname(p) == "Player1")
        {
            SpaceShipBitmap = SplashKit.BitmapNamed("Player2");
        }
        if (getname(p) == "Player2")
        {
            SpaceShipBitmap = SplashKit.BitmapNamed("Player1");
        }
        if (getname(p) == "Player1S")
        {
            SpaceShipBitmap = SplashKit.BitmapNamed("Player2S");
        }
        if (getname(p) == "Player2S")
        {
            SpaceShipBitmap = SplashKit.BitmapNamed("Player1S");
        }
    }

    public void HandleInput()
    {
        int movement = Map.LANE_WIDTH;
        int speed = 4;
        if (SplashKit.KeyReleased(KeyCode.RightKey) || SplashKit.KeyReleased(KeyCode.DKey))
        {
            X += movement;
        }
        if (SplashKit.KeyReleased(KeyCode.LeftKey) || SplashKit.KeyReleased(KeyCode.AKey))
        {
            X -= movement;
        }
        if (SplashKit.KeyDown(KeyCode.UpKey) || SplashKit.KeyDown(KeyCode.WKey))
        {
            Y -= speed;
        }
        if (SplashKit.KeyDown(KeyCode.DownKey) || SplashKit.KeyDown(KeyCode.SKey))
        {
            Y += speed;
        }
        if (SplashKit.KeyReleased(KeyCode.LeftCtrlKey))
        {
            SwapPlayer(SpaceShipBitmap, GetBitmap);
        }
        if (SplashKit.KeyReleased(KeyCode.EscapeKey))
        {
            Quit = true;
        }
    }

    public void Move()
    {
        HandleInput();
        StayInTrack();
    }

    public void Draw()
    {
        SpaceShipBitmap.Draw(X, Y);
    }

    private void StayInTrack()
    {
        if (X >= Map.LANE_LEFT + Map.LANE_WIDTH * 5) //the right side of track
        {
            X -= Map.LANE_WIDTH;
        }
        if (X < Map.LANE_LEFT) //the left side of track
        {
            X += Map.LANE_WIDTH;
        }
        if (Y > _gameWindow.Height - SpaceShipBitmap.Height)
        {
            Y = _gameWindow.Height - SpaceShipBitmap.Height;
        }
        if (Y < 0)
        {
            Y = 0;
        }
    }
}

/////////////////////////////////////////////////////////////////////////
///////////////////////////Reward1.cs////////////////////////////////////////
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
/////////////////////////////////////////////////////////////////////////
///////////////////////////Reward2.cs////////////////////////////////////////
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
/////////////////////////////////////////////////////////////////////////
/////////////////////////SpaceGame.cs////////////////////////////////////
using System;
using SplashKitSDK;
using System.Collections.Generic;

public class SpaceGame
{
    private Window _window;
    private Player _player;
    private Map _map;
    private List<AI> _ai = new List<AI>();
    private Timer _timer;
    private uint _doubleSpeedTime;
    private uint _invincibleTime;
    private int _score;
    private int _level;
    private int _basicSpeed;
    private bool _reward1;
    private bool _reward2;
    public bool Restart;
    private bool _addNew;
    private bool[] _lane = new bool[5];
    public delegate string GetBitmapName(Bitmap bitmap);
    public bool ESC
    {
        get
        {
            return _player.Quit;
        }
    }
    public void LoadResource()
    {
        SplashKit.LoadBitmap("Player1", "Spaceship1.png");
        SplashKit.LoadBitmap("Player2", "Spaceship2.png");
        SplashKit.LoadBitmap("Player1S", "Spaceship1S.png");
        SplashKit.LoadBitmap("Player2S", "Spaceship2S.png");
        SplashKit.LoadBitmap("Enemy1", "fireball.png");
        SplashKit.LoadBitmap("Enemy2", "Alien1.png");
        SplashKit.LoadBitmap("Enemy3", "Alien2.png");
        SplashKit.LoadBitmap("Reward1", "Fuel.png");
        SplashKit.LoadBitmap("Reward2", "Star.png");
        SplashKit.LoadBitmap("Space", "Space.png");
       
        SplashKit.LoadFont("FontC", "calibri.ttf");
        SplashKit.LoadFont("FontU", "unknown.ttf");
        SplashKit.LoadFont("FontJ", "jeebra.ttf");
    }

    public SpaceGame(Window w)
    {
        _window = w;
        LoadResource();
        _player = new Player(_window);
        _map = new Map(_window);
        _timer = new Timer("gameTimer");
        _timer.Start();
    }

    public void Update()
    {
        _player.Move();
        _map.Move();
        foreach (AI ai in _ai)
        {
            ai.Move();
        }
        Collision();
        CheckOverLine();
        CheckReward();
        AddNewEnemy();
        RemoveAI();
        LaneStatus();
        Level();
        SetSpeed();
    }

    public void Draw()
    {
        DrawUI();
        _map.Draw();
        PlayerTwinkle();
        foreach (AI ai in _ai)
        {
            ai.Draw();
        }
    }

    public void DrawUI()
    {
       
     
        _window.DrawBitmap(SplashKit.BitmapNamed("Reward1"), 600, 150);
       

        _window.DrawBitmap(SplashKit.BitmapNamed("Reward2"), 600, 300);
       
    }

    public void Level()
    {
        _basicSpeed = 1 + _level;
        _score += _basicSpeed;
        _level = Convert.ToInt32(_timer.Ticks) / 20000;
    }

    public void SetSpeed()
    {
        if (_reward1)
        {
            
            foreach (AI ai in _ai)
            {
                ai.Speed = _basicSpeed * 2;
            }
        }
        else
        {
           
            foreach (AI ai in _ai)
            {
                ai.Speed = _basicSpeed;
            }
        }
    }

    public void CheckReward()
    {
        _reward1 = (_timer.Ticks < _doubleSpeedTime);
        _reward2 = (_timer.Ticks < _invincibleTime);
        InvincibleBitmap(_player.SpaceShipBitmap, bitmap => SplashKit.BitmapName(bitmap));
    }

    public void InvincibleBitmap(Bitmap player, GetBitmapName getname)
    {
        if (_reward2)
        {
            if (getname(player) == "Player1")
            {
                _player.SpaceShipBitmap = SplashKit.BitmapNamed("Player1S");
            }
            if (getname(player) == "Player2")
            {
                _player.SpaceShipBitmap = SplashKit.BitmapNamed("Player2S");
            }
        }
        else
        {
            if (getname(player) == "Player1S")
            {
                _player.SpaceShipBitmap = SplashKit.BitmapNamed("Player1");
            }
            if (getname(player) == "Player2S")
            {
                _player.SpaceShipBitmap = SplashKit.BitmapNamed("Player2");
            }
        }
    }

    public void PlayerTwinkle()
    {
        if (_invincibleTime - _timer.Ticks > 700 && _invincibleTime - _timer.Ticks < 1000) { }
        else if (_invincibleTime - _timer.Ticks > 1700 && _invincibleTime - _timer.Ticks < 2000) { }
        else if (_invincibleTime - _timer.Ticks > 2700 && _invincibleTime - _timer.Ticks < 3000) { }
        else
        {
            _player.Draw();
        }
    }

    public void Collision()
    {
        foreach (AI ai in _ai)
        {
            if (ai.ColliedWith(_player))
            {
                if (SplashKit.BitmapName(ai.SpaceShipBitmap) == "Reward1")
                {
                    if (!_reward1)
                    {

                        _doubleSpeedTime = _timer.Ticks + 10000;
                    }
                    else
                        _doubleSpeedTime += 10000;
                }
                else if (SplashKit.BitmapName(ai.SpaceShipBitmap) == "Reward2")
                {
                    if (!_reward2)
                    {
                        _invincibleTime = _timer.Ticks + 10000;
                    }
                    else
                        _invincibleTime += 10000;
                }
                else if (!_reward2 && SplashKit.BitmapName(ai.SpaceShipBitmap) != "Reward1")
                {
                    SplashKit.DisplayDialog("GameOver", $"Your Score is: {_score} m", SplashKit.FontNamed("FontC"), 20);
                    Restart = true;
                }
            }
        }
    }

    public void LaneStatus()
    {
        foreach (AI ai in _ai)
        {
            _lane[ai.Lane - 1] = true;
        }
    }

    public void CheckOverLine()
    {
        foreach (AI ai in _ai)
        {
            if (ai.IsOverLine != true && ai.Y > _window.Height / 4)
            {
                _addNew = true;
                ai.IsOverLine = true;
            }
        }
    }

    public bool CheckLane(AI ai)
    {
        if (_lane[ai.Lane - 1])
        {
            ai = null;
            return false;
        }
        return true;
    }

    public void AddNewEnemy()
    {
        if (_addNew == true)
        {
            RandomAI();
            _addNew = false;
        }
        // else if (_lane[0] == false && _lane[1] == false && _lane[2] == false && _lane[3] == false && _lane[4] == false)
        else if (!(_lane[0] || _lane[1] || _lane[2] || _lane[3] || _lane[4]))
        {
            RandomAI();
        }
    }

    public void RandomAI()
    {
        bool rightLane = false;
        while (!rightLane)
        {
            double rnd = SplashKit.Rnd();
            if (rnd > 0.4)
            {
                AI newAI = new Enemy1();
                if (CheckLane(newAI))
                {
                    _ai.Add(newAI);
                    rightLane = true;
                }
            }
            if (rnd <= 0.4 && rnd > 0.2)
            {
                AI newAI = new Enemy2();
                if (CheckLane(newAI))
                {
                    _ai.Add(newAI);
                    rightLane = true;
                }
            }
            if (rnd <= 0.2 && rnd > 0.1)
            {
                AI newAI = new Enemy3();
                if (CheckLane(newAI))
                {
                    _ai.Add(newAI);
                    rightLane = true;
                }
            }
            if (rnd <= 0.1 && rnd > 0.03)
            {
                AI newAI = new Reward1();
                if (CheckLane(newAI))
                {
                    _ai.Add(newAI);
                    rightLane = true;
                }
            }
            if (rnd <= 0.03)
            {
                AI newAI = new Reward2();
                if (CheckLane(newAI))
                {
                    _ai.Add(newAI);
                    rightLane = true;
                }
            }
        }
    }

    public void RemoveAI()
    {
        List<AI> _uselessAI = new List<AI>();
        foreach (AI ai in _ai)
        {
            if (ai.Y > _window.Height || ai.ColliedWith(_player))
            {
                _uselessAI.Add(ai);
                _lane[ai.Lane - 1] = false;
            }
        }
        foreach (AI r in _uselessAI)
        {
            _ai.Remove(r);
        }
    }
}

/////////////////////////////////////////////////////////////////////////