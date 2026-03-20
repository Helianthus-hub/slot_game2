using Godot;
using System;

public partial class SlotMachineEngine : Node2D
{
	private TextureRect[] GridSlots = new TextureRect[25];
	private Symbol[] symbols = new Symbol[25];
	
	
	[Export] private GridContainer Grid;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		InitializeGrid();
				Sprite2D mySprite = new Sprite2D();
				Clover cl = new Clover();
				Heart he = new Heart();
				Texture2D myTexture = GD.Load<Texture2D>(cl.ImagePath);
				mySprite.Texture = myTexture;
				//var textureRect = new TextureRect();
				//textureRect.Texture = GD.Load<Texture2D>(cl.ImagePath);
				//var textureRect2 = new TextureRect();
				//Grid.AddChild(textureRect);
				//GridSlots[0] = textureRect;
				//textureRect2.Texture = GD.Load<Texture2D>(he.ImagePath);
				//GridSlots[1] = textureRect2;
				for(int i = 0; i < 25; i++){
					var textureRect = new TextureRect();
					textureRect.Texture = GD.Load<Texture2D>(he.ImagePath);
					GridSlots[i] = textureRect;
				}
				BuildGrid(GridSlots);
				
				
				
				
			//}
		//}
	}

	// Called every frame. 'delta' is the elapsed time sincse the previous frame.
	public override void _Process(double delta)
	{
		
	}
	public void InitializeGrid(){
		Grid.Columns = 5;
		foreach (Node child in Grid.GetChildren()){
		child.Free();}
	}
	public void BuildGrid(params TextureRect[] textures){
		foreach(var texture in textures){
			Grid.AddChild(texture);
		}
	}
}
