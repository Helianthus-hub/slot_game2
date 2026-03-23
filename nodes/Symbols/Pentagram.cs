using Godot;
using System;

public partial class Pentagram : Symbol {
	public override string ImagePath => "res://sprites/pentagram.png";
	public override SymbolType Type => SymbolType.Pentagram;
	public override double SymbolWeight => 1.0;
	public Pentagram(){}
}
