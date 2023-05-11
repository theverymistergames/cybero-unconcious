using System;
using System.Collections.Generic;
using System.Linq;
using MisterGames.Blueprints;
using UnityEngine;

public enum InventoryItems {
    Handle0,
    Handle1,
    Handle2,
}

public static class Inventory {
    private static readonly HashSet<InventoryItems> Items = new HashSet<InventoryItems>();

    public static void AddItem (InventoryItems item) {
        Items.Add(item);
    }
    
    public static void RemoveItem (InventoryItems item) {
        Items.Remove(item);
    }
    
    public static bool CheckItem (InventoryItems item) {
        return Items.Contains(item);
    }
}

[Serializable]
[BlueprintNodeMeta(Name = "Add Inventory Item", Category = "Inventory", Color = BlueprintColors.Node.Data)]
public sealed class BlueprintNodeAddInventoryItem : BlueprintNode, IBlueprintEnter {

    [SerializeField] private InventoryItems item;

    public override Port[] CreatePorts() => new[] {
        Port.Enter(),
    };
    
    public void OnEnterPort(int port) {
        if (port != 0) return;

        Inventory.AddItem(item);
    }
}

[Serializable]
[BlueprintNodeMeta(Name = "Remove Inventory Item", Category = "Inventory", Color = BlueprintColors.Node.Data)]
public sealed class BlueprintNodeRemoveInventoryItem : BlueprintNode, IBlueprintEnter {

    [SerializeField] private InventoryItems item;

    public override Port[] CreatePorts() => new[] {
        Port.Enter(),
    };
    
    public void OnEnterPort(int port) {
        if (port != 0) return;

        Inventory.RemoveItem(item);
    }
}

[Serializable]
[BlueprintNodeMeta(Name = "Check Inventory Item", Category = "Inventory", Color = BlueprintColors.Node.Data)]
public sealed class BlueprintNodeCheckInventoryItem : BlueprintNode, IBlueprintOutput<bool> {

    [SerializeField] private InventoryItems item;

    public override Port[] CreatePorts() => new[] {
        Port.Output<bool>(),
    };

    bool IBlueprintOutput<bool>.GetOutputPortValue(int port) => port switch {
        0 => Inventory.CheckItem(item)
    };
}