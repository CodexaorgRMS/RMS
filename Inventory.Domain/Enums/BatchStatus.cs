using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Domain.Enums;

public enum BatchStatus
{
    Active = 1,  
    OnHold = 2, 
    Recalled = 3, 
    Expired = 4   
}