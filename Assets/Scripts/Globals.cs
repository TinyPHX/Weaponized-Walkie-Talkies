using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// none of the methods and variables in here control anything in Unity, only scripts written to use these variables and methods will use them
public class Globals {

    public static bool LOG_MISSING_REFERENCES = false; // whether or not missing assignments are logged

    /// <summary>
    /// Logs missing references based on referenceName, scriptName, objectName, and LOG_MISSING_REFERENCES boolean
    /// </summary>
    /// <param name="referenceName">Name of missing reference.</param>
    /// <param name="scriptName">Name of script (e.g. Script.cs).</param>
    /// <param name="objectName">Name of GameObject that holds script component (use this.name).</param>
    /// <
    public static void logMissingReference(string referenceName, string scriptName, string objectName)
    {
        if (LOG_MISSING_REFERENCES)
        {
            Debug.Log("Reference '" + referenceName + "' not assigned in '" + scriptName + "' attached to '" + objectName + "'");
        }
    }

}