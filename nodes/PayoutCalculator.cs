using Godot;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
public static class PayoutCalculator {
    
	//Diagonal
	//Pyramid
	// FullHouse
	//ZigZag
	//Triangle
	public static List<HandResult> Evaluate(Symbol[,] Symbols){
        var results = new List<HandResult>();
	}
	// Horizontal
	public static List<HandResult> EvaluateHorizontal(Symbol[,] Symbols, int length){
        var results = new List<HandResult>();
		for(int row = 0; row < 5; row++){
			Symbol a = Symbols[row, 0];
			Symbol b = Symbols[row, 1];
			int j = 0;
			int i = 0;
			while(a.Type == b.Type || i  < 3){
                a = Symbols[row, i];
                b = Symbols[row, i + 1];
                i++;
				if(a.Type == b.Type){
					j++;
				}
				if(i == 5){
				    break;
				}
				}
				switch (j){
					case 2: results.Add(new HandResult(b.Type, GameConfig.ThreeOfAKindPayout,  ));  
						break;
                    case 3: results.Add(new HandResult());
					break;  
					case 4: results.Add(new HandResult());
					break;  
				}
			
		}
		return(results);
			}
}
