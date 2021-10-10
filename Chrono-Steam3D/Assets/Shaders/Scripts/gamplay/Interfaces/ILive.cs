using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILive 
{
    float MaxLife { get; }
    Life_Controller Life_Controller { get;}
}
