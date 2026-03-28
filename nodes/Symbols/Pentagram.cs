using Godot;
using System;

public partial class Pentagram : Symbol {
	public override string ImagePath => "res://sprites/pentagram.png";
	public override SymbolType Type => SymbolType.Pentagram;
	public Pentagram()
	{
		Multi = 1.0f;
		BaseChips = 10;
	}
}
