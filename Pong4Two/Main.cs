using Godot;
using System;

public class Main : Node
{

    // Constant for ball speed (in pixels/second)
    [Export]
    int INITIAL_ballSpeed = 80;

    // Constant for pads speed
    [Export]
    int PAD_SPEED = 150;

    // Speed of the ball (also in pixels/second)
    float ballSpeed;
    Vector2 ballPosition;

    Vector2 _screenSize;
    //Shape2D padSize;
    Vector2 direction = new Vector2(1.0f, 0.0f);

    float lastDelta;

    static float NextFloat()
    {
        Random rand = new Random();
        float randomFloat = (float)rand.NextDouble();
        return randomFloat;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _screenSize = GetViewport().Size;
        ballSpeed = INITIAL_ballSpeed;
        GetNode<Area2D>("Ball").Position += direction;
        //padSize = GetNode<CollisionShape2D>("LeftPaddle/CollisionShape2D").Shape;
        SetProcess(true);
    }

    // public override void _Process(float delta)
    // {
    //     GetNode<Area2D>("Ball").Position += direction;
    // }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        // Get ball position
        ballPosition = GetNode<Area2D>("Ball").Position;

        // Check if the ball hits top or bottom of the screen
        if ((ballPosition.y < 0 && direction.y < 0) || (ballPosition.y > _screenSize.y && direction.y > 0))
        {
            direction.y = -direction.y; // Invert direction
        }

        // Check if ball area entered one of pad's area and if so, redirect it on x axis and set random direction on y axis
        if ((GetNode<Area2D>("LeftPaddle").OverlapsArea(GetNode<Area2D>("Ball"))) || (GetNode<Area2D>("RightPaddle").OverlapsArea(GetNode<Area2D>("Ball"))))
        {
            direction.x = -direction.x; // Invert x axis direction
            direction.y = NextFloat() * 2.0f - 1f; // Get random float for y axis
            direction = direction.Normalized(); // Normalize the direction
            ballSpeed *= 1.1f; // Multiply the speed (so it get's harder over time)
            GetNode<CollisionShape2D>("Ball/CollisionShape2D").SetDeferred("disabled", true);
        }

        // If ball get's outside of screen on x axis (goal), teleport it to the middle of the screen
        if (ballPosition.x < 0 || ballPosition.x > _screenSize.x)
        {
            ballPosition = _screenSize * 0.5f; // Set the ball's position to the middle of the screen
            ballSpeed = INITIAL_ballSpeed; // Set the speed to initial speed
            direction = new Vector2(-1, 0); // Set default direction
        }

        // Integrate new ball position
        ballPosition += direction * ballSpeed * delta;

        // Update ball's position
        GetNode<Area2D>("Ball").Position = ballPosition;

        // Move left pad
        Vector2 leftPosition = GetNode<Area2D>("LeftPaddle").Position;

        if (leftPosition.y > 0 && Input.IsActionPressed("LeftUp"))
        {
            leftPosition.y += -PAD_SPEED * delta;
        }
        if (leftPosition.y < _screenSize.y && Input.IsActionPressed("LeftDown"))
        {
            leftPosition.y += PAD_SPEED * delta;
        }

        GetNode<Area2D>("LeftPaddle").Position = leftPosition;

        // Move right pad
        Vector2 rightPosition = GetNode<Area2D>("RightPaddle").Position;

        if (rightPosition.y > 0 && Input.IsActionPressed("RightUp"))
        {
            rightPosition.y += -PAD_SPEED * delta;
        }
        if (rightPosition.y < _screenSize.y && Input.IsActionPressed("RightDown"))
        {
            rightPosition.y += PAD_SPEED * delta;
        }


        GetNode<Area2D>("RightPaddle").Position = rightPosition;
        GetNode<CollisionShape2D>("Ball/CollisionShape2D").Disabled = false;
    }
}
