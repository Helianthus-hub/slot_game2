using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

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
		AnimateMatchedCells(hands);
        GD.Print($"Total payout: {totalPayout}");
    }

	private Symbol[] BuildSymbolPool(){
    var pool = new List<Symbol>();
    for (int i = 0; i < GameConfig.StartingPentagramAmount; i++)
        pool.Add(new Pentagram());
    for (int i = 0; i < GameConfig.StartingAnkhAmount; i++)
        pool.Add(new Ankh());
    for (int i = 0; i < GameConfig.StartingCloverAmount; i++)
        pool.Add(new Clover());
    for (int i = 0; i < GameConfig.StartingHeartAmount; i++)
        pool.Add(new Heart());

    var result = pool.ToArray();
    PrintPool(result);
    return result;
	}
	//private Symbol[] UpdateSymbolPool(){
		
		//return;
	//}

	private void PrintPool(Symbol[] pool)
{
    var counts = new Dictionary<SymbolType, int>();
    foreach (var s in pool)
    {
        if (!counts.ContainsKey(s.Type)) counts[s.Type] = 0;
        counts[s.Type]++;
    }

    GD.Print($"=== Symbol Pool (total: {pool.Length}) ===");
    foreach (var (type, count) in counts)
        GD.Print($"  {type}: {count} ({(float)count / pool.Length * 100:F1}%)");
}
    private int ApplyResults(List<HandResult> hands){
        int total = 0;
        foreach (HandResult hand in hands)
        {
            total += hand.Payout;
            GD.Print($"{hand.Type} +{hand.Payout}  cells: {string.Join(", ", hand.Cells)}");
        }
        return total;
    }

	private void AnimateMatchedCells(List<HandResult> hands){
    	foreach (HandResult hand in hands){
        	foreach (Vector2I cell in hand.Cells){
            TextureRect slot = GridSlots[cell.X, cell.Y];
            Tween tween = CreateTween();
            tween.SetLoops(3);
            tween.TweenProperty(slot, "modulate", new Color(1.5f, 1.5f, 0.5f), 0.1f); // flash yellow
            tween.TweenProperty(slot, "modulate", new Color(1, 1, 1), 0.1f);           // back to normal
    		}
   	   }
	}
}
