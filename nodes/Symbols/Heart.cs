using Godot;
using System;

public partial class Heart : Symbol {
	public override string ImagePath => "res://sprites/heart.png";
	public override SymbolType Type => SymbolType.Heart;
	public override double SymbolWeight => 1.0;
	public Heart(){}
}
