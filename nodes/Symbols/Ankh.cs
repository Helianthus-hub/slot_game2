using Godot;
using System;

public partial class Ankh : Symbol
{
	public override string ImagePath => "res://sprites/ankh.png";
	public override SymbolType Type => SymbolType.Ankh;
    public override double SymbolWeight => 1.0;

	public Ankh(){}
}
