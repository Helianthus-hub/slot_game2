using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public static class HandDrawer{
	
	public static Symbol DrawSymbol(SymbolPoolLists lists, Dictionary<SymbolType, float> probabilities, Random rng){
		float roll = (float)rng.NextDouble();
		float probabilitySum = 0.0f;
		SymbolType rolledType = SymbolType.Heart;//fallback jei type negautum bet neturetu taip buti
		foreach(var symbolEntry in probabilities){
			probabilitySum += symbolEntry.Value;
			if(roll < probabilitySum){
				rolledType = symbolEntry.Key;
				break;
			}
		}
		
		//atrusiuojam candidates for the Draw pagal tipa is active symbol pool ir sudedam i lista
		List<Symbol> symbolCandidates = lists.ActiveSymbolPool.Where(symbol => symbol.Type == rolledType && symbol.InDeck).ToList();

		//Jei pvz turi 0 hearts bet DrawSymbol isrinko heart tipa
		if (symbolCandidates.Count == 0){
        	GD.PrintErr($"DrawSymbol: no instances of {rolledType}, picking random fallback.");
        	return lists.PlayerSymbolPool.First(s => s.InDeck);
    	}
		//Paimam random instance of the specific symbol type
		return symbolCandidates[rng.Next(0, symbolCandidates.Count)];
	}
	public static void RandomizeSymbols(SymbolPoolLists lists, Symbol[,] symbol, Dictionary<SymbolType, float> probabilities){
		Random rng = new Random();
		for(int row = 0; row < GameConfig.GridRows; row++){
			for(int col = 0; col < GameConfig.GridColumns; col++){
				symbol[row, col] = DrawSymbol(lists, probabilities, rng);
			}
		}
	}
}
