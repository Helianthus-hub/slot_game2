using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Net;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
					for(int i = 0; i < MatchLength; i++){
						cells.Add(new Vector2I(row, col + i));
					}
					(HandType type , int payout) = MatchLength switch {
						5 => (HandType.HorizontalFive  , GameConfig.FiveOfAKindPayout),
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
			for(int i = 0; i < 5; i++){
				cells.Add(new Vector2I(0, i));
			}
			cells.Add(new Vector2I(1, 1));
			cells.Add(new Vector2I(2, 2));
			cells.Add(new Vector2I(1, 3));

			results.Add(new HandResult(HandType.PyramidUpper, GameConfig.PyramidPayout,  cells));
		}
		//CheckingLowerPyramid structure
		while(col < 4 && Symbols[4, col].Type == Symbols[4, col + 1].Type){
			MatchLength++;
			col++;
		}
		SymbolType type2 = Symbols[3, 1].Type;
		if(MatchLength == 5 && type2 == Symbols[2, 2].Type && type2 == Symbols[3, 3].Type){
			var cells = new List<Vector2I>();
			for(int i = 0; i < 5; i++){
				cells.Add(new Vector2I(0, i));
			}
			cells.Add(new Vector2I(3, 1));
			cells.Add(new Vector2I(2, 2));
			cells.Add(new Vector2I(3, 3));

			results.Add(new HandResult(HandType.PyramidLower, GameConfig.PyramidPayout,  cells));
		}
		return results;
	}
	public static List<HandResult> EvaluateDiagonal(Symbol[,] Symbols)
	{
		var results = new List<HandResult>();
		//Startpoints
		var StartPointsL = new List<Vector2I>();
		var StartPointsR = new List<Vector2I>();
		//Generate Startpoints
		for(int i = 2; i > 0; i--)
		{
			StartPointsL.Add(new Vector2I(i,0));
			StartPointsL.Add(new Vector2I(0,i));
		}
		for(int i = 4; i > -1; i--)
		{
			StartPointsR.Add(new Vector2I(4-i,4));
			StartPointsR.Add(new Vector2I(0,i));
		}
		
		//Definitions for the necessary components of the iterations
		var head = Symbols[0,0];
		var next = Symbols[1,1];
		var nextPos = new Vector2I(1,1);
		var headPos = new Vector2I(0,0);

		var LtoR = new Vector2I(1, 1);
		var RtoL = new Vector2I(1,-1);

		

		//Cheking necessary top left to right diagonals 
		foreach(var startPoint in StartPointsL)
		{
			int MatchLength = 0;
			headPos = startPoint;
			head = Symbols[headPos.Y,headPos.X];
			nextPos = startPoint + LtoR;
			next = Symbols[nextPos.Y ,nextPos.X];
			while(next != null || (nextPos.Y < 5 && nextPos.X < 5))
			{
				if(head.Type == next.Type)
				{
					next = Symbols[nextPos.Y, nextPos.X];
					nextPos += LtoR;
					MatchLength++;
				}
				else if(MatchLength == 1)
				{
					MatchLength = 0;
				}
				else if (MatchLength > 1)
				{
					break;
				}
				else
				{
					headPos += LtoR;
					//break contingency covered by the 2nd else statement 
					nextPos = headPos + LtoR;
					if(nextPos.Y < 5 && nextPos.X < 5)
					{
						head = Symbols[headPos.Y, headPos.X];
						next = Symbols[nextPos.Y, nextPos.X];
						
					}
					else break;
				}
			}
			if(MatchLength > 1)
			{
				//Calculate Cells
					var cells = new List<Vector2I>();
					var MatchedSymbols = new List<Symbol>();
					while(next != head)
				{
					cells.Add(new Vector2I(nextPos.Y,nextPos.X));
					MatchedSymbols.Add(next);
					nextPos -= LtoR;
				}
				//Calculate HandType
					(HandType type, int payout) = MatchLength switch {
					2 => (HandType.DiagonalFive, GameConfig.FiveOfAKindPayout),
					3 => (HandType.DiagonalFour, GameConfig.FourOfAKindPayout),
					_ => (HandType.DiagonalThree, GameConfig.ThreeOfAKindPayout),
				};
				results.Add(new HandResult(type, payout, cells, MatchedSymbols));
			}
		
		
	} 
	//Cheking necessary top right to left diagonals 
	foreach(var startPoint in StartPointsL)
		{
			int MatchLength = 0;
			headPos = startPoint;
			head = Symbols[headPos.Y,headPos.X];
			nextPos = startPoint + RtoL;
			next = Symbols[nextPos.Y ,nextPos.X];
			while(next != null || (nextPos.Y < 5 && nextPos.X > -1))
			{
				if(head.Type == next.Type)
				{
					next = Symbols[nextPos.Y, nextPos.X];
					nextPos += RtoL;
					MatchLength++;
				}
				else if(MatchLength == 1)
				{
					MatchLength = 0;
				}
				else if (MatchLength > 1)
				{
					break;
				}
				else
				{
					headPos += RtoL;
					//break contingency covered by the 2nd else statement 
					nextPos = headPos + RtoL;
					if(nextPos.Y < 5 && nextPos.X > -1)
					{
						head = Symbols[headPos.Y, headPos.X];
						next = Symbols[nextPos.Y, nextPos.X];
						
					}
					else break;
				}
			}
			if(MatchLength > 1)
			{
				//Calculate Cells
					var cells = new List<Vector2I>();
					var MatchedSymbols = new List<Symbol>();
					while(next != head)
				{
					cells.Add(new Vector2I(nextPos.Y,nextPos.X));
					MatchedSymbols.Add(next);
					nextPos -= RtoL;
				}
				//Calculate HandType
					(HandType type, int payout) = MatchLength switch {
					2 => (HandType.DiagonalFive, GameConfig.FiveOfAKindPayout),
					3 => (HandType.DiagonalFour, GameConfig.FourOfAKindPayout),
					_ => (HandType.DiagonalThree, GameConfig.ThreeOfAKindPayout),
				};
				results.Add(new HandResult(type, payout, cells, MatchedSymbols));
			}
		}
		return results;
	}
}
