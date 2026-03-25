using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

public static class SymbolProbabilityManager{
    public static int CountInPool(SymbolPoolLists lists, Type symbolType){
        return lists.PlayerSymbolPool.Count(symbol => symbol.GetType() == symbolType);
    }
}
