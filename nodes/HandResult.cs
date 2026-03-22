using Godot;
using System;
using System.Collections.Generic;

public class HandResult {
	public HandType Type;
	public int Payout;
	public List<Vector2I> Cells;
	
	public HandResult(HandType Type, int Payout,  List<Vector2I> Cells){
		this.Type = Type;
		this.Payout =  Payout;
		this.Cells = Cells;
	}
}
