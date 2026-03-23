using Godot;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
public static class PayoutCalculator {
    
	
	
	// FullHouse
	//ZigZag
	//Triangle
	public static List<HandResult> Evaluate(Symbol[,] Symbols){
        var results = new List<HandResult>();
		results.AddRange(EvaluateHorizontal(Symbols));
		results.AddRange(EvaluatePyramid(Symbols));
		return results;
	}
	// Horizontal
	public static List<HandResult> EvaluateHorizontal(Symbol[,] Symbols){
        var results = new List<HandResult>();
		for(int row = 0; row < 5; row++){
			int col = 0;
			while(col < 5){
				int MatchLength = 1;
				while(col + MatchLength < 5 && Symbols[row, col].Type == Symbols[row, col + MatchLength].Type){
					MatchLength++;
				}
				if(MatchLength >= 3){
					var cells = new List<Vector2I>();
					var MatchedSymbols = new List<Symbol>();
					for(int i = 0; i < MatchLength; i++){
						cells.Add(new Vector2I(row, col + i));
						MatchedSymbols.Add(Symbols[row, col + i]);
					}
					(HandType type , int payout) = MatchLength switch {
						5 => (HandType.HorizontalFive, GameConfig.FiveOfAKindPayout),
						4 => (HandType.HorizontalFour,GameConfig.FourOfAKindPayout),
						_ => (HandType.HorizontalThree, GameConfig.ThreeOfAKindPayout),
					}; 
					results.Add(new HandResult(type, payout, cells, MatchedSymbols));
				}
				col += MatchLength;
			}
		}
		return results;
	}
	//Diagonal

	//Pyramid
	public static List<HandResult> EvaluatePyramid(Symbol[,] Symbols){
		var results = new List<HandResult>();
		int MatchLength = 1;
		int col = 0;
		//Checking UpperPyramid structure
		while(col < 4 && Symbols[0, col].Type == Symbols[0, col + 1].Type){
			MatchLength++;
			col++;
		}
		SymbolType type = Symbols[1, 1].Type;
		if(MatchLength == 5 && type == Symbols[2, 2].Type && type == Symbols[0, 4].Type){
			var cells = new List<Vector2I>();
			var MatchedSymbols = new List<Symbol>();
			for(int i = 0; i < 5; i++){
				cells.Add(new Vector2I(0, i));
				MatchedSymbols.Add(Symbols[0, i]);
			}
			cells.Add(new Vector2I(1, 1));
			cells.Add(new Vector2I(2, 2));
			cells.Add(new Vector2I(1, 3));
			MatchedSymbols.Add(Symbols[1, 1]);
			MatchedSymbols.Add(Symbols[2, 2]);
			MatchedSymbols.Add(Symbols[1, 3]);

			results.Add(new HandResult(HandType.PyramidUpper, GameConfig.PyramidPayout,  cells, MatchedSymbols));
		}
		//CheckingLowerPyramid structure
		while(col < 4 && Symbols[4, col].Type == Symbols[4, col + 1].Type){
			MatchLength++;
			col++;
		}
		SymbolType type2 = Symbols[3, 1].Type;
		if(MatchLength == 5 && type2 == Symbols[2, 2].Type && type2 == Symbols[3, 3].Type){
			var cells = new List<Vector2I>();
			var MatchedSymbols = new List<Symbol>();
			for(int i = 0; i < 5; i++){
				cells.Add(new Vector2I(0, i));
				MatchedSymbols.Add(Symbols[0, i]);
			}
			cells.Add(new Vector2I(3, 1));
			cells.Add(new Vector2I(2, 2));
			cells.Add(new Vector2I(3, 3));
			MatchedSymbols.Add(Symbols[3, 1]);
			MatchedSymbols.Add(Symbols[2, 2]);
			MatchedSymbols.Add(Symbols[3, 3]);

			results.Add(new HandResult(HandType.PyramidLower, GameConfig.PyramidPayout,  cells, MatchedSymbols));
		}
		return results;
	}

	// FullHouse
	public static List<HandResult> EvaluateFullHouse(Symbol[,] Symbols){
		var results = new List<HandResult>();
		int MatchLength = 1;
		int col = 0;
		//Checking UpperPyramid structure
		while(col < 4 && Symbols[0, col].Type == Symbols[0, col + 1].Type 
		&& Symbols[4, col].Type == Symbols[4, col + 1].Type){
			MatchLength++;
			col++;
		}
		SymbolType type = Symbols[1, 1].Type;
		if(MatchLength == 5 && type == Symbols[2, 2].Type && type == Symbols[0, 4].Type 
		&& type == Symbols[2, 2].Type && type == Symbols[3, 3].Type){
			var cells = new List<Vector2I>();
			var MatchedSymbols = new List<Symbol>();
			for(int i = 0; i < 5; i++){
				cells.Add(new Vector2I(0, i));
				MatchedSymbols.Add(Symbols[0, i]);
			}
			for(int i = 0; i < 5; i++){
				cells.Add(new Vector2I(4, i));
				MatchedSymbols.Add(Symbols[4, i]);
			}
			cells.Add(new Vector2I(1, 1));
			cells.Add(new Vector2I(2, 2));
			cells.Add(new Vector2I(1, 3));
			cells.Add(new Vector2I(3, 1));
			cells.Add(new Vector2I(3, 3));
			MatchedSymbols.Add(Symbols[1, 1]);
			MatchedSymbols.Add(Symbols[2, 2]);
			MatchedSymbols.Add(Symbols[1, 3]);
			MatchedSymbols.Add(Symbols[3, 1]);
			MatchedSymbols.Add(Symbols[3, 3]);

			results.Add(new HandResult(HandType.FullHouse, GameConfig.FullHousePayout,  cells, MatchedSymbols));
		}
		return results;
	}
}
