using Godot;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Numerics;

public static class PayoutCalculator {
	//ZigZag
	public static List<HandResult> Evaluate(Symbol[,] Symbols){
        var results = new List<HandResult>();
		results.AddRange(EvaluateHorizontal(Symbols));
		results.AddRange(EvaluatePyramid(Symbols));
		results.AddRange(EvaluateFullHouse(Symbols));
		results.AddRange(EvaluateDiagonal(Symbols));
		results.AddRange(EvaluateZigZag(Symbols));
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
	public static List<HandResult> EvaluateDiagonal(Symbol[,] Symbols){
		var results = new List<HandResult>();
		//Startpoints
		var StartPointsL = new List<Vector2I>() {new(2,0), new(1,0), new(0,0), new(0,1), new(0,2)};
		var StartPointsR = new List<Vector2I>() {new(2,4), new(1,4), new(0,4), new(0,3), new(0,2)};

		//Definitions for the necessary components of the iterations
		var head = Symbols[0,0];
		var next = Symbols[1,1];
		var nextPos = new Vector2I(1,1);
		var headPos = new Vector2I(0,0);

		var LtoR = new Vector2I(1, 1);
		var RtoL = new Vector2I(1,-1);

		//Cheking necessary top left to right diagonals
		foreach(var startPoint in StartPointsL){
			int MatchLength = 0;
			headPos = startPoint;
			head = Symbols[headPos.X, headPos.Y];
			nextPos = startPoint + LtoR;
			next = Symbols[nextPos.X ,nextPos.Y];

			while(nextPos.X < 5 && nextPos.Y < 5){
				if(head.Type == next.Type){
					if(nextPos.X == 4 || nextPos.Y == 4){
						MatchLength++;
						break;
					}
					nextPos += LtoR;
					next = Symbols[nextPos.X, nextPos.Y];
					MatchLength++;
				}
				else if(MatchLength == 1){
					MatchLength = 0;
				}
				else if (MatchLength > 1){
					nextPos -= LtoR;
					next = Symbols[nextPos.X, nextPos.Y];
					break;
				}
				else{
					headPos += LtoR;
					//break contingency covered by the 2nd else statement
					nextPos = headPos + LtoR;
					if(nextPos.X < 5 && nextPos.Y < 5){
						head = Symbols[headPos.X, headPos.Y];
						next = Symbols[nextPos.X, nextPos.Y];
					}
					else break;
				}
			}
			if(MatchLength > 1){
				(HandType type, int payout) = MatchLength switch {
					4 => (HandType.DiagonalFive, GameConfig.FiveOfAKindPayout),
					3 => (HandType.DiagonalFour, GameConfig.FourOfAKindPayout),
					_ => (HandType.DiagonalThree, GameConfig.ThreeOfAKindPayout),
				};
				var cells = new List<Vector2I>();
				var MatchedSymbols = new List<Symbol>();
				var pos = nextPos;
				for (int i = 0; i <= MatchLength; i++){
					cells.Add(new Vector2I(pos.X, pos.Y));
					MatchedSymbols.Add(Symbols[pos.X, pos.Y]);
					pos -= LtoR;
				}
				results.Add(new HandResult(type, payout, cells, MatchedSymbols));
			}
		}
		foreach(var startPoint in StartPointsR){
			int MatchLength = 0;
			headPos = startPoint;
			head = Symbols[headPos.X, headPos.Y];
			nextPos = startPoint + RtoL;
			next = Symbols[nextPos.X ,nextPos.Y];

			while(nextPos.X < 5 && nextPos.Y > -1){
				if(head.Type == next.Type){
					if(nextPos.X == 4 || nextPos.Y == 0){
						MatchLength++;
						break;
					}
					nextPos += RtoL;
					next = Symbols[nextPos.X, nextPos.Y];
					MatchLength++;
				}
				else if(MatchLength == 1){
					MatchLength = 0;
				}
				else if (MatchLength > 1){
					nextPos -= RtoL;
					next = Symbols[nextPos.X, nextPos.Y];
					break;
				}
				else{
					headPos += RtoL;
					//break contingency covered by the 2nd else statement
					nextPos = headPos + RtoL;
					if(nextPos.X < 5 && nextPos.Y > -1){
						head = Symbols[headPos.X, headPos.Y];
						next = Symbols[nextPos.X, nextPos.Y];
					}
					else break;
				}
			}
			if(MatchLength > 1){
				(HandType type, int payout) = MatchLength switch {
					4 => (HandType.DiagonalFive, GameConfig.FiveOfAKindPayout),
					3 => (HandType.DiagonalFour, GameConfig.FourOfAKindPayout),
					_ => (HandType.DiagonalThree, GameConfig.ThreeOfAKindPayout),
				};
				var cells = new List<Vector2I>();
				var MatchedSymbols = new List<Symbol>();
				var pos = nextPos;
				for (int i = 0; i <= MatchLength; i++){
					cells.Add(new Vector2I(pos.X, pos.Y));
					MatchedSymbols.Add(Symbols[pos.X, pos.Y]);
					pos -= RtoL;
				}
				results.Add(new HandResult(type, payout, cells, MatchedSymbols));
			}
	}
		return results;
	}

	//ZigZag
	public static List<HandResult> EvaluateZigZag(Symbol[,] Symbols) {
		var results = new List<HandResult>();

		for (int startRow = 0; startRow < 5; startRow++) {
			foreach (bool startDown in new[] { true, false }) {
				var path = BuildZigzagPath(startRow, startDown);
				if (path.Count < 3) continue;

				int headIdx = 0;
				while (headIdx < path.Count) {
					int nextIdx = headIdx + 1;
					while (nextIdx < path.Count &&
						   Symbols[path[nextIdx].X, path[nextIdx].Y].Type == Symbols[path[headIdx].X, path[headIdx].Y].Type) {
						nextIdx++;
					}
					int runLength = nextIdx - headIdx;
					if (runLength >= 3) {
						var cells = new List<Vector2I>();
						var matchedSymbols = new List<Symbol>();
						for (int k = headIdx; k < nextIdx; k++) {
							cells.Add(path[k]);
							matchedSymbols.Add(Symbols[path[k].X, path[k].Y]);
						}
						(HandType type, int payout) = runLength switch {
							>= 5 => (HandType.ZigzagFive,  GameConfig.FiveOfAKindPayout),
							4    => (HandType.ZigzagFour,   GameConfig.FourOfAKindPayout),
							_    => (HandType.ZigzagThree,  GameConfig.ThreeOfAKindPayout),
						};
						results.Add(new HandResult(type, payout, cells, matchedSymbols));
					}
					headIdx = nextIdx;
				}
			}
		}
		return results;
	}

	private static List<Vector2I> BuildZigzagPath(int startRow, bool startDown) {
		var path = new List<Vector2I>();
		int row = startRow;
		bool goingDown = startDown;
		for (int col = 0; col < 5; col++) {
			if (row < 0 || row >= 5) break;
			path.Add(new Vector2I(row, col));
			row += goingDown ? 1 : -1;
			goingDown = !goingDown;
		}
		return path;
	}

	//Pyramid
	public static List<HandResult> EvaluatePyramid(Symbol[,] Symbols){
		var results = new List<HandResult>();
		int UpperMatchLength = 1;
		int LowerMatchLength = 1;
		int upperCol = 0;
		int lowerCol = 0;
		//Checking UpperPyramid structure
		while(upperCol < 4 && Symbols[0, upperCol].Type == Symbols[0, upperCol + 1].Type){
			UpperMatchLength++;
			upperCol++;
		}
		SymbolType type = Symbols[0, 0].Type;
		if(UpperMatchLength == 5 && type == Symbols[1, 1].Type && type == Symbols[2, 2].Type && type == Symbols[1, 3].Type){
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
		while(lowerCol < 4 && Symbols[4, lowerCol].Type == Symbols[4, lowerCol + 1].Type){
			LowerMatchLength++;
			lowerCol++;
		}
		SymbolType type2 = Symbols[4, 0].Type;
		if(LowerMatchLength == 5 && type2 == Symbols[3, 1].Type && type2 == Symbols[2, 2].Type && type2 == Symbols[3, 3].Type){
			var cells = new List<Vector2I>();
			var MatchedSymbols = new List<Symbol>();
			for(int i = 0; i < 5; i++){
				cells.Add(new Vector2I(4, i));
				MatchedSymbols.Add(Symbols[4, i]);
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

