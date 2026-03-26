using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public partial class SlotMachineEngine : Node2D
{
	private TextureRect[,] GridSlots = new TextureRect[5, 5];
	private Symbol[,] Symbols = new Symbol[5, 5];

	SymbolPoolLists lists;
		
	[Export] private GridContainer Grid;

	public override void _Ready(){
		lists = SymbolPoolManager.InitializeSymbolPool();
		Spin();
	}

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
		SymbolPoolManager.UpdateSymbolPool(lists);
		HandDrawer.RandomizeSymbols(lists, Symbols, SymbolProbabilityManager.GetProbabilities());
        BuildGrid();
 
        List<HandResult> hands = PayoutCalculator.Evaluate(Symbols);
        int totalPayout = ApplyResults(hands);
		AnimateMatchedCells(hands);
        GD.Print($"Total payout: {totalPayout}");
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

	//For debugging hand evaluations etc
	private void FillHearts(){
		string symbolPath = "res://sprites/heart.png";
		Grid.Columns = GameConfig.GridColumns;
		foreach(Node child in Grid.GetChildren()){
			child.Free();//istrina is atminties kad duplicates nebutu
		}
		for(int row = 0; row < 5; row++){
			for(int col = 0; col < 5; col++){
				Symbols[row, col] = new Heart();
				var textureRect = new TextureRect();
				textureRect.Texture = GD.Load<Texture2D>(symbolPath);
				GridSlots[row, col] = textureRect;
				Grid.AddChild(textureRect);
			}
		}
	}

	private void AnimateMatchedCells(List<HandResult> hands){
    	foreach (HandResult hand in hands){
        	foreach (Vector2I cell in hand.Cells){
            TextureRect slot = GridSlots[cell.X, cell.Y];
            Tween tween = CreateTween();
            tween.SetLoops(3);
            tween.TweenProperty(slot, "modulate", new Color(1.5f, 1.5f, 0.5f), 0.1f); // flash yellow
            // tween.TweenProperty(slot, "modulate", new Color(1, 1, 1), 0.1f);           // back to normal
    		}
   	   }
	}
}
