using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterComponent
{
    /// <summary>
    /// ‰Šú‰»ˆ—
    /// </summary>
    void OnCreate();

    /// <summary>
    /// XVˆ—
    /// </summary>
    void OnUpdate();

    /// <summary>
    /// I—¹ˆ—
    /// </summary>
    void OnEnd();

    Transform Parent { get; }
}
