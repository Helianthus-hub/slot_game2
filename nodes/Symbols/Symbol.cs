using Godot;
using System;
using System.Collections.Generic;

public abstract class Symbol{
	public abstract string ImagePath {get;}
	public abstract SymbolType Type {get;}
	public bool InDeck = true;
	public float Multi = 1.0f;
	public int BaseChips = 1;
	public float BaseProbability = 0.25f;
	public List<string> Modifiers = new();//Example: Cursed, Lucky etc basically modifiers
	public override string ToString(){
    	return $"[{Type}] Chips:{BaseChips} Multi:{Multi} InDeck:{InDeck} Modifiers:{string.Join(", ", Modifiers)}";
	}
}
