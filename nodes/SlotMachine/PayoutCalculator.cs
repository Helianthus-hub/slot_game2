using Godot;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
public static class PayoutCalculator {
	//ZigZag
	public static List<HandResult> Evaluate(Symbol[,] Symbols){
		var results = new List<HandResult>();
		results.AddRange(EvaluateHorizontal(Symbols));
		results.AddRange(EvaluatePyramid(Symbols));
		results.AddRange(EvaluateFullHouse(Symbols));
		results.AddRange(EvaluateDiagonal(Symbols));
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
	public static List<HandResult> EvaluateDiagonal(Symbol[,] Symbols)
	{
		var results = new List<HandResult>();
		//Startpoints
		var StartPointsL = new List<Vector2I>() {new(2,0), new(1,0), new(0,0), new(0,1), new(0,2)};
		var StartPointsR = new List<Vector2I>() {new(2,4), new(1,4), new(0,4), new(0,3), new(0,2)};
		//Generate Startpoints
		// for(int i = 2; i > 0; i--)
		// {
		// 	StartPointsL.Add(new Vector2I(i,0));
		// 	StartPointsL.Add(new Vector2I(0,i));
		// }
		// for(int i = 4; i > -1; i--)
		// {
			
		// 	StartPointsR.Add(new Vector2I(4-i,4));
		// 	StartPointsR.Add(new Vector2I(0,i));
		// }

		
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
			head = Symbols[headPos.X, headPos.Y];
			nextPos = startPoint + LtoR;
			next = Symbols[nextPos.X ,nextPos.Y];
			int iterations = 0;
			while(nextPos.X < 4 && nextPos.Y < 4)
			{
				GD.Print($"MatchLength is {MatchLength} with iterations {iterations}");
				GD.Print($"Comparing {head.Type} at {headPos} with {next.Type} at , where nextPos is {nextPos} with y {nextPos.Y} and x {nextPos.X}");	
				iterations++;
				if(head.Type == next.Type)
				{
					nextPos += LtoR;
					next = Symbols[nextPos.X, nextPos.Y];
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
					if(nextPos.X < 5 && nextPos.Y < 5)
					{
						head = Symbols[headPos.X, headPos.Y];
						next = Symbols[nextPos.X, nextPos.Y];
						
					}
					else break;
				}
				
			}
			
			if(MatchLength > 1)
			{
				
				//Calculate Cells
					var cells = new List<Vector2I>();
					var MatchedSymbols = new List<Symbol>();
					nextPos -= LtoR;
					while(MatchLength-- >= 0)
				{
					
					cells.Add(new Vector2I(nextPos.X,nextPos.Y));
					MatchedSymbols.Add(next);
					nextPos -= LtoR;
				}
				//Calculate HandType
					(HandType type, int payout) = MatchLength switch {
					4 => (HandType.DiagonalFive, GameConfig.FiveOfAKindPayout),
					3 => (HandType.DiagonalFour, GameConfig.FourOfAKindPayout),
					_ => (HandType.DiagonalThree, GameConfig.ThreeOfAKindPayout),
				};
				results.Add(new HandResult(type, payout, cells, MatchedSymbols));
			}
		
		
	} 
	//Cheking necessary top right to left diagonals 
	// foreach(var startPoint in StartPointsR)
	// 	{
	// 		int MatchLength = 0;
	// 		headPos = startPoint;
	// 		head = Symbols[headPos.X,headPos.Y];
	// 		nextPos = startPoint + RtoL;
	// 		next = Symbols[nextPos.X ,nextPos.Y];
	// 		while(nextPos.X < 5 && nextPos.Y > -1)
	// 		{
	// 			if(head.Type == next.Type)
	// 			{
	// 				nextPos += RtoL;
	// 				next = Symbols[nextPos.X, nextPos.Y];
	// 				MatchLength++;
	// 			}
	// 			else if(MatchLength == 1)
	// 			{
	// 				MatchLength = 0;
	// 			}
	// 			else if (MatchLength > 1)
	// 			{
	// 				break;
	// 			}
	// 			else
	// 			{
					
	// 				headPos += RtoL;
	// 				//break contingency covered by the 2nd else statement 
	// 				nextPos = headPos + RtoL;
					
	// 				if(nextPos.X < 5 && nextPos.Y > -1)
	// 				{
	// 					head = Symbols[headPos.X, headPos.Y];
	// 					next = Symbols[nextPos.X, nextPos.Y];
						
	// 				}
	// 				else break;
	// 			}
	// 		}
	// 		if(MatchLength > 1)
	// 		{
	// 			//Calculate Cells
	// 				var cells = new List<Vector2I>();
	// 				var MatchedSymbols = new List<Symbol>();
	// 				nextPos -= RtoL;
	// 				while(MatchLength-- >= 0)
	// 			{
	// 				cells.Add(new Vector2I(nextPos.X,nextPos.Y));
	// 				MatchedSymbols.Add(next);
	// 				nextPos -= RtoL;
	// 			}
	// 			//Calculate HandType
	// 				(HandType type, int payout) = MatchLength switch {
	// 				4 => (HandType.DiagonalFive, GameConfig.FiveOfAKindPayout),
	// 				3 => (HandType.DiagonalFour, GameConfig.FourOfAKindPayout),
	// 				_ => (HandType.DiagonalThree, GameConfig.ThreeOfAKindPayout),
	// 			};
	// 			results.Add(new HandResult(type, payout, cells, MatchedSymbols));
	// 		}
	// 	}
		return results;
	}
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

			results.Add(new HandResult(HandType.PyramidLower, GameConfig.PyramidPayout, cells, MatchedSymbols));
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
