using Godot;
using System;

public class Main : Node
{
    [Export]
    // Constant for ball speed (in pixels/second)
    const int INITIAL_BALL_SPEED = 80;

    [Export]
    // Constant for pads speed
    const int PAD_SPEED = 150;

    // Speed of the ball (also in pixels/second)
    int ball_speed = INITIAL_BALL_SPEED;

    Vector2 _screenSize;
    //Shape2D padSize;
    Vector2 direction = new Vector2(1.0f, 0.0f);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _screenSize = GetViewport().Size;
        //padSize = GetNode<CollisionShape2D>("LeftPaddle/CollisionShape2D").Shape;
        //SetProcess(true);
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        //Position2D ball = GetNode<Ball>("Ball").Position;
    }
}
