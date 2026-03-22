using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public partial class SlotMachineEngine : Node2D
{
	private TextureRect[,] GridSlots = new TextureRect[5, 5];
	private Symbol[,] Symbols = new Symbol[5, 5];
	
	
	[Export] private GridContainer Grid;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready(){
		Spin();
	}
	
	public void InitializeGrid(){
		Grid.Columns = GameConfig.GridColumns;
	}
	public void RandomizeSymbols(){
		//reiktu exclusicity implement (Symbol.InDeck)
		Dictionary<int, string> SymbolPaths = new Dictionary<int, string>() 
			{
				{0, "res://sprites/heart.png"},
				{1, "res://sprites/clover.png"},
				{2, "res://sprites/ankh.png"},
				{3, "res://sprites/pentagram.png"},
			};
			
			Random rng = new Random();
		for(int row = 0; row < 5; row++){
			for(int col = 0; col < 5; col++){
				
					//tipo jei mes padedam tada skippinam
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
		 if (Input.IsKeyPressed(Key.A))
	{
		RandomizeSymbols();
		BuildGrid();
	}
	}
	//Cia bus pagrindine logika kaip skaiciuojami laimejimai veliau bus kiti skirtingi laimejimo variantai dabar noriu padaryti 3 of a kind
	//kinda works tik reiks diagonal skaiciavima prideti ir kad neskaiciuotu tu paciu simboliu 3 kartus jei yra 4 of a kind ar 5 of a kind
	//
	public int CalculateThreeOfAKind(){
		//Fix bug
		int payout = 0;
		for(int row = 0; row < 5; row++){
			for(int col = 0; col <= 2; col++){
				Symbol a = Symbols[row, col];
				Symbol b = Symbols[row, col +1];
				Symbol c = Symbols[row, col +2];
				if(a.Type == b.Type && b.Type == c.Type){
					payout += GameConfig.ThreeOfAKindPayout;
				}
			}
		}
		return payout;
	}
	public int CalculateSpinResult(){
		int payout= 0;
		payout+= CalculateThreeOfAKind();
		GD.Print("Spin Result: " + payout);
		return payout;
	}
	
	public void BuildGrid(){
		Grid.Columns = GameConfig.GridColumns;
		foreach(Node child in Grid.GetChildren()){
			child.Free();//istrina is atminties kad duplicates nebutu
		}
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
	public void Spin(){
		RandomizeSymbols();
		BuildGrid();
		CalculateSpinResult();
	}
}
