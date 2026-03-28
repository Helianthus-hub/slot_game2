using Godot;
using System;

public partial class Heart : Symbol {
	public override string ImagePath => "res://sprites/heart.png";
	public override SymbolType Type => SymbolType.Heart;
	public Heart()
	{
		Multi = 2.0f;
		BaseChips = 10;
	}
}
