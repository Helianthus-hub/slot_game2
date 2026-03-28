using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class HandResult {
	public HandType Type;
	public int Payout;
	public int BaseBonus;
	public List<Vector2I> Cells;
	public List<Symbol> MatchedSymbols;
	
	public HandResult(HandType Type, int Payout,  List<Vector2I> Cells, List<Symbol> MatchedSymbols){
		this.Type = Type;
		this.Payout =  Payout;
		this.Cells = Cells;
		this.MatchedSymbols = MatchedSymbols;
	}
	public int CalculatePayout(){
		int chips = MatchedSymbols.Sum(symbol => symbol.BaseChips) + BaseBonus;
		float multi = MatchedSymbols.Aggregate(1.0f, (accumulated, symbol) => accumulated * symbol.Multi);
		return (int)(chips * multi);
	}

}
