using Godot;
using System;

public class Main : Node
{
    [Export]
    public PackedScene Mob; // Load Mob scene - selected in editor

    private int _score; // Store the score
    private Random _random = new Random(); // Random numbers

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        NewGame();
    }

    ///<summary>Get random floats</summary>
    private float RandRange(float min, float max)
    {
        return (float)_random.NextDouble() * (max - min) + min;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    /*public override void _Process(float delta)
    {

    }*/

    ///<summary>Stops game timers - runs when player's hit - literally game over</summary>
    public void GameOver()
    {
        GetNode<Timer>("MobTimer").Stop();
        GetNode<Timer>("ScoreTimer").Stop();
    }

    ///<summary>Initializes new Game</summary>
    public void NewGame(){
        _score = 0; // Reset the score
        Player player = GetNode<Player>("Player"); // Get the player node
        Position2D startPosition = GetNode<Position2D>("StartPosition"); // Get starting postion

        player.Start(startPosition.Position); // Move the player to the starting position

        GetNode<Timer>("StartTimer").Start(); // Start StartTimer (start countdown) -- start the game
    }

    ///<summary>Start timeout - starts MobTimer and ScoreTimer - new game begins</summary>
    public void OnStartTimerTimeout(){
        GetNode<Timer>("MobTimer").Start();
        GetNode<Timer>("ScoreTimer").Start();
    }

    ///<summary>Increases score by 1 each timeout (1s)</summary>
    public void OnScoreTimerTimeout(){
        _score++;
    }

    public void OnMobTimerTimeout(){
        // Get random spawnlocation for mobs
        PathFollow2D mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
        mobSpawnLocation.Offset = _random.Next();

        // Create new Mob instance and add it to the scene
        var mobInstance = (RigidBody2D)Mob.Instance();
        AddChild(mobInstance);

        // Set the mob's direction perpendicular to the path direction.
        float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;

        // Set's the mob position to a random location
        mobInstance.Position = mobSpawnLocation.Position;

        // Add some randomness to the Mob direction
        direction +=  RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
        mobInstance.Rotation = direction;

        // Choose Mob speed
        mobInstance.LinearVelocity =  new Vector2(RandRange(150f, 250f), 0).Rotated(direction);
    }
}
