using Microsoft.AspNetCore.Components.Forms;

namespace GreenVille.Portal.UserComponent
{

    /// <summary>
    /// Extensão do InputSelect que aceite número (int32) como chave.
    /// A versão padrão só aceita "string" ou "enum"
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
                    validationErrorMessage = "O valor selecionado não é um número válido.";
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