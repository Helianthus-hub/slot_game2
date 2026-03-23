using Godot;
using System;
using System.Collections.Generic;

public abstract partial class Symbol : Node
{
	public abstract string ImagePath {get;}
	public abstract SymbolType Type {get;}
	public bool InDeck = false;
	public float Multi = 1.0f;
	public int BaseChips = 1;
	public List<string> Modifiers = new();//Example: Cursed, Lucky etc basically modifiers
}
