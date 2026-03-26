
//Cia rasom visus Final timpo variables kad galetume
//naudoti savo programoje ir reusinti keisti tik sitoje vietoje
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
public static int ThreeOfAKindPayout = 100;
public static int FourOfAKindPayout = 200;
public static int FiveOfAKindPayout = 300;
public static int PyramidPayout = FiveOfAKindPayout * 2;
public static int FullHousePayout = FiveOfAKindPayout * 4;

}
