using Godot;
using System;

public class Player : Area2D
{
    [Signal]
    public delegate void Hit();
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    [Export]
    public int Speed = 400; // How fast the player will move (pixels/sec).

    private Vector2 _screenSize; // Size of the game window.

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _screenSize = GetViewport().Size; // Get screen resolution
        Hide(); // Hide the player when the game starts
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Vector2 velocity = new Vector2(); // For storing velocity

        if (Input.IsActionPressed("ui_right")) // If key right is pressed, add velocity to move right
        {
            velocity.x += 1;
        }
        if (Input.IsActionPressed("ui_left")) // If key left...
        {
            velocity.x -= 1;
        }
        if (Input.IsActionPressed("ui_up")) // If key up...
        {
            velocity.y -= 1;
        }
        if (Input.IsActionPressed("ui_down")) // If key down...
        {
            velocity.y += 1;
        }

        AnimatedSprite adnimatedSprite = GetNode<AnimatedSprite>("AnimatedSprite"); // Create instance of the player AnimatedSprite

        if (velocity.Length() > 0) // If the player moves, normalize his vector (he would be faster diagonally) and multiply it by speed, start the animation
        {
            velocity = velocity.Normalized() * Speed;
            adnimatedSprite.Play();
        }
        else
        { // Else stop the animation
            adnimatedSprite.Stop();
        }
        Position += velocity * delta; // Adjust player position according to his velocity and multiply it by delta (so he moves the same speed no matter the fps)
        Position = new Vector2(  // Clamp the player position, so he doesn't go outside the screen
            x: Mathf.Clamp(Position.x, 0, _screenSize.x),
            y: Mathf.Clamp(Position.y, 0, _screenSize.y)
        );

        if (velocity.x != 0)
        { // Adjust animations according to player movement
            adnimatedSprite.Animation = "walk";
            adnimatedSprite.FlipV = false;

            adnimatedSprite.FlipH = velocity.x < 0;
        }
        if (velocity.y != 0)
        {
            adnimatedSprite.Animation = "up";
            adnimatedSprite.FlipV = velocity.y > 0;
        }
    }

    ///<summary>Runs when player collides with Mob
    ///<para>Hides the player, emits hit signal and disables collisions</para></summary>
    public void OnPlayerBodyEntered(PhysicsBody2D body)
    {
        Hide(); // Player disappears after being hit.
        EmitSignal("Hit"); // Call hit - connected to GameOver in MainScene
        GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", true); // Disable colisions - so things don't get messed up
    }


    ///<summary>Starts the player
    ///<para>Shows the player, enables collisions, populates player position</para></summary>
    public void Start(Vector2 pos)
    {
        Position = pos;
        Show();
        GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
    }
}
