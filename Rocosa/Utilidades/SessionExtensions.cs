using System.Text.Json;

namespace Rocosa.Utilidades
{
    public static class SessionExtensions
    {
        //Método para configurar la sesión
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        //Método para obtener el valor de la sesión
        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            //Es una condición si valor es igual a null (? => then) mande default (: => else), caso contrario lo que le sigue
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}
