using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

// Indica si se encuentra amenzado por la explosion de alguna bomba
public class EndangeredCondition : Conditional
{
    public override void OnAwake()
    {
        
    }

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Success;
    }
}
