using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowPasswordToggle : MonoBehaviour
{
    public Toggle newUserToggle;
    public Toggle returningUserToggle;

    public InputField newUserPasswordField;
    public InputField returningUserPasswordField;

    public void NewUserToggleSwitch()
    {
        if (newUserToggle.isOn)
            newUserPasswordField.contentType = InputField.ContentType.Standard;
        else if (!newUserToggle.isOn)
            newUserPasswordField.contentType = InputField.ContentType.Password;
        newUserPasswordField.ActivateInputField();
    }

    public void ReturningUserToggleSwitch()
    {
        if (returningUserToggle.isOn)
            returningUserPasswordField.contentType = InputField.ContentType.Standard;
        else if (!returningUserToggle.isOn)
            returningUserPasswordField.contentType = InputField.ContentType.Password;
        returningUserPasswordField.ActivateInputField();
    }
}
