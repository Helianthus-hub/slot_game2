using Godot;
using System;
using System.Collections.Generic;

public partial class SlotMachineEngine : Node2D
{
	private TextureRect[,] GridSlots = new TextureRect[5, 5];
	private Symbol[,] Symbols = new Symbol[5, 5];
	
	
	[Export] private GridContainer Grid;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
			InitializeGrid();
			BuildGrid();
			GD.Print("hello");
			CalculateThreeOfAKind();

					//Sprite2D mySprite = new Sprite2D();
					/*over cl = new Clover();
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
						if(i % 2 == 0){
						Heart heart = new Heart();
						Symbols[i] = heart;
						}
						else{
							if(i % 3 == 0){
							Clover clover = new Clover();
							Symbols[i] = clover;
							}
							else{
								Ankh ankh = new Ankh();
								Symbols[i] = ankh;
							}
						}
					}*/
					
				//}
			//}

		
		
	}
	
	//Pabandysiu padarti kad random simbolius sudetu i grida
	//Saugom elementus mape su ju paths to sprite
	Dictionary<int, string> SymbolPaths = new Dictionary<int, string>() 
			{
				{0, "res://sprites/heart.png"},
				{1, "res://sprites/clover.png"},
				{2, "res://sprites/ankh.png"},
				{3, "res://sprites/pentagram.png"},
			};
	public void InitializeGrid(){
		Grid.Columns = 5;
		foreach(Node child in Grid.GetChildren()){
			child.Free();//istrina is atminties kad duplicates nebutu
		}
		Random rng = new Random();
		for(int row = 0; row < 5; row++){
			for(int col = 0; col < 5; col++){
				int randomPick = rng.Next(0, SymbolPaths.Count);
				Symbols[row, col] = randomPick switch {
					0 => new Heart(),
					1 => new Clover(),
					2 => new Ankh(),
					3 => new Pentagram(),
					_ => new Heart()
				};
			}
		}
	}
	// Called every frame. 'delta' is the elapsed time sincse the previous frame.
	public override void _Process(double delta)
	{
		
	}
	//Cia bus pagrindine logika kaip skaiciuojami laimejimai veliau bus kiti skirtingi laimejimo variantai dabar noriu padaryti 3 of a kind
	//kinda works tik reik pagalvot kaip antra for loop padaryti kad nebutu out of
	//bounds exception, dabar tikrina 3 simbolius is eiles bet reikia kad butu 5 simboliai
	public int CalculateThreeOfAKind(){
		int payout = 0;
		for(int row = 0; row < 5; row++){
			for(int col = 0; col <= 2; col++){
				Symbol a = Symbols[row, col];
				Symbol b = Symbols[row, col +1];
				Symbol c = Symbols[row, col +2];
				if(a.GetType() == b.GetType() && b.GetType() == c.GetType()){
					payout += GameConfig.ThreeOfAKindPayout;
				}
			}
		}
		GD.Print("Three of a kind payout: " + payout);
		return payout;
	}
	public int CalculateSpinResult(){
		
		return 0;
	}
	
	public void BuildGrid(){
		for(int row = 0; row < 5; row++){
			for(int col = 0; col < 5; col++){
				var textureRect = new TextureRect();
				textureRect.Texture = GD.Load<Texture2D>(Symbols[row, col].ImagePath);
				GridSlots[row, col] = textureRect;
				Grid.AddChild(textureRect);
			}
		}
	}
	public void UpdateGrid(int row, int col, Symbol symbol){
		GridSlots[row, col].Texture = GD.Load<Texture2D>(symbol.ImagePath);
	}
}
