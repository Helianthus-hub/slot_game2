using Godot;
using System;

public partial class Ankh : Symbol
{
	public override string ImagePath => "res://sprites/ankh.png";
	public override SymbolType Type => SymbolType.Ankh;
	public Ankh(){
		Multi = 1.0f;
		BaseChips = 10;
	}
}
