using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Object that holds the tasks for a single round of the test, as well as an index for the current task
/// </summary>
public class Round {

    public string[] tasks;
    public int currentTask;

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="tasks"></param>
    /// <param name="currentTask"></param>
    public Round(string[] tasks, int currentTask)
    {
        this.tasks = tasks;
        this.currentTask = currentTask;
    }

}
