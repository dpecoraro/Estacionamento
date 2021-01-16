using Microsoft.AspNetCore.Components.Forms;

namespace GreenVille.Portal.UserComponent
{

    /// <summary>
    /// Extens�o do InputSelect que aceite n�mero (int32) como chave.
    /// A vers�o padr�o s� aceita "string" ou "enum"
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class InputSelectNumber<T> : InputSelect<T>
    {
        protected override bool TryParseValueFromString(string value, out T result, out string validationErrorMessage)
        {
            if (typeof(T) == typeof(int))
            {
                if (int.TryParse(value, out var resultInt))
                {
                    result = (T)(object)resultInt;
                    validationErrorMessage = null;
                    return true;
                }
                else
                {
                    result = default;
                    validationErrorMessage = "O valor selecionado n�o � um n�mero v�lido.";
                    return false;
                }
            }
            else
            {
                return base.TryParseValueFromString(value, out result, out validationErrorMessage);
            }
        }
    }

}