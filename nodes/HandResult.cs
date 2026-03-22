using Godot;
using System;
using System.Collections.Generic;

public class HandResult {
	public HandType Type;
	public int Payout;
	public List<Vector2I> Cells;
	public List<Symbol> MatchedSymbols;
	
	public HandResult(HandType Type, int Payout,  List<Vector2I> Cells, List<Symbol> MatchedSymbols){
		this.Type = Type;
		this.Payout =  Payout;
		this.Cells = Cells;
		this.MatchedSymbols = MatchedSymbols;
	}
}
