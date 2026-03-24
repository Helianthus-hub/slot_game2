using Godot;
using System;

public partial class Pentagram : Symbol {
	public override string ImagePath => "res://sprites/pentagram.png";
	public override SymbolType Type => SymbolType.Pentagram;
	public override int SymbolWeight => 1;
	public Pentagram(){}
}
