using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class SlotMachineEngine : Node2D{
	[Signal]
	public delegate void SpinCompletedEventHandler(int payout, int chips, float multi);
	[Signal]
	public delegate void HandResolvedEventHandler(int type, int payout, int chips, float multi);
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
	public async void Spin(){
    SymbolPoolManager.UpdateSymbolPool(lists);
    HandDrawer.RandomizeSymbols(lists, Symbols, SymbolProbabilityManager.GetProbabilities());
    BuildGrid();

    List<HandResult> hands = PayoutCalculator.Evaluate(Symbols);
    int totalPayout = 0;

    foreach (HandResult hand in hands){
        int chips = hand.MatchedSymbols.Sum(s => s.BaseChips) + hand.BaseBonus;
        float multi = hand.MatchedSymbols.Aggregate(1.0f, (acc, s) => acc * s.Multi);
        int payout = hand.CalculatePayout();
        totalPayout += payout;

        AnimateMatchedCells(hand);
        EmitSignal(SignalName.HandResolved, (int)hand.Type, payout, chips, multi);

        await ToSignal(GetTree().CreateTimer(3.0f), SceneTreeTimer.SignalName.Timeout);
    }

    EmitSignal(SignalName.SpinCompleted, totalPayout, 0, 0f); // total at the end
}
    private (int payout, int chips, float multi) ApplyResults(List<HandResult> hands){
        int totalPayout = 0;
    	int totalChips = 0;
   	 	float totalMulti = 1.0f;
        foreach (HandResult hand in hands){
			totalPayout += hand.CalculatePayout();
            totalChips += hand.MatchedSymbols.Sum(s => s.BaseChips) + hand.BaseBonus;
        	totalMulti *= hand.MatchedSymbols.Aggregate(1.0f, (acc, s) => acc * s.Multi);
            GD.Print($"{hand.Type} → {hand.MatchedSymbols.Sum(symbol => symbol.BaseChips) + hand.BaseBonus} chips × {hand.MatchedSymbols.Aggregate(1.0f, (acc, s) => acc * s.Multi):F2} multi = {totalPayout}");
        }
        return (totalPayout, totalChips, totalMulti);
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
                var textureRect = new TextureRect{Texture = GD.Load<Texture2D>(symbolPath)};
                GridSlots[row, col] = textureRect;
				Grid.AddChild(textureRect);
			}
		}
	}

	private void AnimateMatchedCells(HandResult hand) {
    foreach (Vector2I cell in hand.Cells) {
        TextureRect slot = GridSlots[cell.X, cell.Y];
        Tween tween = CreateTween();
        tween.SetLoops(3);
        tween.TweenProperty(slot, "modulate", new Color(1.5f, 1.5f, 0.5f), 0.1f);
        tween.TweenProperty(slot, "modulate", Colors.White, 0.1f);
    	}
	}
}
