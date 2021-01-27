using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand
{
    /// <summary>
    /// ‰Šú‰»ˆ—
    /// </summary>
    void Initialize();

    /// <summary>
    /// Às
    /// </summary>
    void Execution();

    /// <summary>
    /// I—¹ŒŸ’m
    /// </summary>
    /// <returns></returns>
    bool IsEnd();
}
