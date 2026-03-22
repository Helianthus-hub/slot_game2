using Godot;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
public static class PayoutCalculator {
    
	//Diagonal
	//Pyramid
	// FullHouse
	//ZigZag
	//Triangle
	public static List<HandResult> Evaluate(Symbol[,] Symbols){
        var results = new List<HandResult>();
		results.AddRange(EvaluateHorizontal(Symbols));
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
					for(int i = 0; i < MatchLength; i++){
						cells.Add(new Vector2I(row, col + i));
					}
					(HandType type , int payout) = MatchLength switch {
						5 => (HandType.HorizontalFive, GameConfig.FiveOfAKindPayout),
						4 => (HandType.HorizontalFour,GameConfig.FourOfAKindPayout),
						_ => (HandType.HorizontalThree, GameConfig.ThreeOfAKindPayout),
					}; 
					results.Add(new HandResult(type, payout, cells));
				}
				col += MatchLength;
			}
		}
		return results;
	}
}
