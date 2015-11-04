using UnityEngine;
using System;
using System.Collections;

public static class ValidateHelper {
    public static Validation Begin() {
        return null;
    }
}
//no one can inherit from it.
public sealed class Validation {
    public bool IsValid { get; set; }
}
public static class ValidationExtensions {
    //implement the function you want to do for validation here....

    private static Validation Check<T>(this Validation validation, Func<bool> filterMethod, T exception) where T : Exception {
        if (filterMethod()) {
            return validation ?? new Validation() { IsValid = true };
        }
        else {
            throw exception;
        }
    }
    public static Validation Check(this Validation validation, Func<bool> filterMethod) {
        return Check<Exception>(validation, filterMethod, new Exception("Parameter InValid!"));
    }
    //public static Validation NotNull(this Validation validation, UnityEngine.Object obj) {
    //    return Check<ArgumentNullException>(
    //        validation,
    //        () => obj != null,
    //        new ArgumentNullException(string.Format("The object {0} can't be null", obj))
    //    );
    //}
    public static Validation NotNullObj(this Validation validation, object obj) {
        return Check<ArgumentNullException>(
            validation,
            () => obj != null,
            new ArgumentNullException(string.Format("The object {0} can't be null", obj))
        );
    }
}