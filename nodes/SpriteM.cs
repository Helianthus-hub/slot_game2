using Godot;
using System;

public partial class SpriteM : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		for (int i = 0; i <= 5; i++)
		{
		   for (int j = 0; j <= 5; j++)
			{
				String path = "res://sprites/sprite.png";
				Sprite2D mySprite = new Sprite2D();
				Texture2D myTexture = GD.Load<Texture2D>(path);
				mySprite.Texture = myTexture;
				AddChild(mySprite);
				mySprite.Position = new Vector2(100*i, 200*j);
				mySprite.Scale = new Vector2(0.5f, 0.5f);
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
