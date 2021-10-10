using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAreaAttack
{
    AreaStats AreaStats { get; set; }

    void AreaAtack();
}
