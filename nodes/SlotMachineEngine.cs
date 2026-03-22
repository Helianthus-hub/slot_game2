using Godot;
using System;
using System.Collections.Generic;

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
	public override void _Process(double delta){}
	
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
 
        List<HandResult> hands = PayoutCalculator.Evaluate(Symbols);
        int totalPayout = ApplyResults(hands);
 
        GD.Print($"Total payout: {totalPayout}");
    }
 
    // Sums payout and prints every matched hand.
    private int ApplyResults(List<HandResult> hands)
    {
        int total = 0;
        foreach (HandResult hand in hands)
        {
            total += hand.Payout;
            GD.Print($"{hand.Type} +{hand.Payout}  cells: {string.Join(", ", hand.Cells)}");
        }
        return total;
    }
}
