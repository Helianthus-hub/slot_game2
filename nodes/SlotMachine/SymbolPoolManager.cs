using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

public class SymbolPoolLists{
    public List<Symbol> ActiveSymbolPool {get; set;} = new();
    public List<Symbol> PlayerSymbolPool {get; set;} = new();
}
public static class SymbolPoolManager{
    private static Symbol[,] Symbols = new Symbol[5, 5];
    public static SymbolPoolLists InitializeSymbolPool(){
        SymbolPoolLists lists = new SymbolPoolLists();
        for (int i = 0; i < GameConfig.StartingPentagramAmount; i++)
            lists.PlayerSymbolPool.Add(new Pentagram());
        for (int i = 0; i < GameConfig.StartingAnkhAmount; i++)
            lists.PlayerSymbolPool.Add(new Ankh());
        for (int i = 0; i < GameConfig.StartingCloverAmount; i++)
            lists.PlayerSymbolPool.Add(new Clover());
        for (int i = 0; i < GameConfig.StartingHeartAmount; i++)
            lists.PlayerSymbolPool.Add(new Heart());

        lists.ActiveSymbolPool = new List<Symbol>(lists.PlayerSymbolPool);

        return lists;
	}

    //Panaikina visus simbolius kurie nera decke
    //Prideda visus simbolius is PlayerSymbolPool i ActivePool
    public static void UpdateSymbolPool(SymbolPoolLists lists){
        lists.ActiveSymbolPool.RemoveAll(symbol => !symbol.InDeck);
        foreach(Symbol symbol in lists.PlayerSymbolPool){
            if(!lists.ActiveSymbolPool.Contains(symbol) && symbol.InDeck){
                lists.ActiveSymbolPool.Add(symbol);
            }
        }
    }

    //You can pass either ActivePool or PlayerPool here
    //Todel tas List<SymbolPool> SymbolPool
    public static void AddSymbolToPool(List<Symbol> SymbolPool, Symbol symbol){
        if(!SymbolPool.Contains(symbol)){
            SymbolPool.Add(symbol);
        }
    }

    public static void RemoveSymbolFromPool(List<Symbol> SymbolPool, Symbol symbol){
        SymbolPool.Remove(symbol);
    }

    public static void ModifySymbolProbabilities(){
        
    }

    
    public static void RandomizeSymbols(SymbolPoolLists lists, Symbol[,] symbol){
        if(lists.ActiveSymbolPool.Count == 0){
            GD.PrintErr($" list is empty: {lists.ActiveSymbolPool.Count}");
        }
        var RandomizedSymbols = new List<Symbol>(lists.ActiveSymbolPool);
        Random rng = new Random();
        for(int i = RandomizedSymbols.Count - 1; i > 0; i--){
            int j = rng.Next(0, i - 1);
            (RandomizedSymbols[i], RandomizedSymbols[j]) = (RandomizedSymbols[j], RandomizedSymbols[i]);
        }
        int SymbolIndex = 0;
        for(int rows = 0; rows < GameConfig.GridRows; rows++){
            for(int col = 0; col < GameConfig.GridColumns; col++){
                symbol[rows, col] = RandomizedSymbols[SymbolIndex];
                SymbolIndex++;
            }
        }
			
	}
}
