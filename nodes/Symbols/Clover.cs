using Godot;
using System;

public partial class Clover : Symbol{
	public override string ImagePath => "res://sprites/clover.png";
	public override SymbolType Type => SymbolType.Clover;
	public override double SymbolWeight => 1.0;
	public Clover(){}
}
