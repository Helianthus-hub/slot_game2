using Godot;
using System;

public abstract class SymbolModifiers{
    public abstract string Name {get;}
    //Implementuoti metodus kaip OnMatched() etc
    public virtual void OnMatched(Symbol symbol, Symbol[,] grid){}
    public virtual void OnSpinEnd(Symbol symbol, Symbol[,] grid) {}
}
