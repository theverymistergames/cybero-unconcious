using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using MisterGames.Blueprints;
using UnityEngine;

namespace MisterGames.BlueprintLib {

    [Serializable]
    [BlueprintNodeMeta(Name = "Trigger Enter (Obsolete)", Category = "Collision", Color = BlueprintColors.Node.Data)]
    public sealed class BlueprintNodeTriggerEnter : BlueprintNode, IBlueprintEnter {
        public override Port[] CreatePorts() => new[] {
            Port.Enter(),
            Port.Input<GameObject>("Game object"),
            Port.Exit("On trigger enter"),
        };

        public void OnEnterPort(int port) {
            if (port != 0)  return;
            
            var gameObject = Ports[1].Get<GameObject>();
            var triggerEnterAsync  = gameObject.GetComponent<BoxCollider>().GetAsyncTriggerEnterTrigger().OnTriggerEnterAsync();
            var awaiter = triggerEnterAsync.GetAwaiter();
            
            awaiter.OnCompleted(() => {
                Ports[2].Call();
            });
        }
    }

}
