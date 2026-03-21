using Godot;
using System;

public abstract partial class Symbol : Node
{
	public abstract string ImagePath {get;}
	public abstract SymbolType Type {get;}
	public bool InDeck = false;
}
