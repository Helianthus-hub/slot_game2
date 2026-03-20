using Godot;
using System;

public partial class SlotMachineEngine : Node2D
{
	[Export] private GridContainer _grid;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		foreach (Node child in _grid.GetChildren()){
		child.Free();}
		
		//for (int i = 0; i < 5; i++)
		//{
		   //for (int j = 0; j < 5; j++)
			//{
				String path = "res://sprites/sprite.png";
				Sprite2D mySprite = new Sprite2D();
				Clover cl = new Clover();
				Texture2D myTexture = GD.Load<Texture2D>(cl.ImagePath);
				mySprite.Texture = myTexture;
				var textureRect = new TextureRect();
				textureRect.Texture = GD.Load<Texture2D>(cl.ImagePath);
				_grid.AddChild(textureRect);
				
			//}
		//}
	}

	// Called every frame. 'delta' is the elapsed time sincse the previous frame.
	public override void _Process(double delta)
	{
		
	}
	
	public void SetSymbolAt(int index, Clover symbol)
{
	
}
}
