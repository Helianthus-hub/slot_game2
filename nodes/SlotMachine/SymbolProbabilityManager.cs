using Godot;
using System.Collections.Generic;
using System.Linq;

public static class SymbolProbabilityManager{
    public static Dictionary<SymbolType, float> BaseProbabilities = new(){
        {SymbolType.Heart, 0.25f},
        {SymbolType.Ankh, 0.25f},
        {SymbolType.Clover, 0.25f},
        {SymbolType.Pentagram, 0.25f}
    };

    public static Dictionary<SymbolType, float> GetProbabilities(){
        return BaseProbabilities;
    }

    public static void ModifySymbolProbabilities(SymbolType type, float targetProbability){
        if(targetProbability < 0 || targetProbability > 1){
            GD.PrintErr("Impossible probability: {targetProbability}");
        }
        float probabilitySum = 0.0f;
        foreach(var symbol in BaseProbabilities){
            if(symbol.Key != type){
                probabilitySum += symbol.Value;
            }
        }
        //Paima key is dictionary ir jam priskiria value
        BaseProbabilities[type] = targetProbability;
        float remainingProbability = 1f - probabilitySum;
        foreach(SymbolType otherSymbols in BaseProbabilities.Keys.ToList()){
            if(otherSymbols == type){
                continue;
            }
            BaseProbabilities[otherSymbols] = (BaseProbabilities[otherSymbols] / probabilitySum) * remainingProbability;
        }
        GD.Print($"Boosted {type} to {targetProbability * 100}%. New probabilities:");
        foreach (var entry in BaseProbabilities)
            GD.Print($"  {entry.Key}: {entry.Value * 100:F1}%");
    }
}
