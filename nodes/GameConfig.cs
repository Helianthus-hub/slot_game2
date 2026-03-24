//Cia rasom visus Final timpo variables kad galetume
//naudoti savo programoje ir reusinti keisti tik sitoje vietoje
using System.Collections.Generic;

public static class GameConfig{
//GridColumn amount
public static readonly int GridColumns = 5;
public static readonly int GridRows = 5;
//starting symbol amount
public static readonly int StartingPentagramAmount = 10;
public static readonly int StartingCloverAmount = 10;
public static readonly int StartingAnkhAmount = 10;
public static readonly int StartingHeartAmount = 10;

//Hand payouts    
public static readonly int ThreeOfAKindPayout = 100;
public static readonly int FourOfAKindPayout = 200;
public static readonly int FiveOfAKindPayout = 300;
public static readonly int PyramidPayout = 500;
public static readonly int FullHousePayout = 1000;

//Hand Probabilities
public static readonly Dictionary<HandType, float> HandProbabilities = new (){
        {HandType.HorizontalThree, 0.1f},
        {HandType.HorizontalFour, 0.1f},
        {HandType.HorizontalFive, 0.1f},
        {HandType.DiagonalThree, 0.1f},
        {HandType.DiagonalFour, 0.1f},
        {HandType.DiagonalFive, 0.1f},
        {HandType.ZigzagThree, 0.1f},
        {HandType.ZigzagFour, 0.1f},
        {HandType.ZigzagFive, 0.1f},
        {HandType.PyramidUpper, 0.1f},
        {HandType.PyramidLower, 0.1f},
        {HandType.FullHouse, 0.1f}
    };
}
