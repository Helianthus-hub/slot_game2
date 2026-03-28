using Godot;
using System;

public partial class Clover : Symbol{
	public override string ImagePath => "res://sprites/clover.png";
	public override SymbolType Type => SymbolType.Clover;
	public Clover()
	{
		Multi = 1.0f;
		BaseChips = 10;
	}
}
