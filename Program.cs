using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

class CifradoSimetrico
{
    static void Main()
    {
        string textaACifrar = "El profesor Juan Carlos Valencia\nposee una voz de locutor";

        // Generar clave y vector de inicialización
        using (Aes aes = Aes.Create())
        {
            aes.GenerateKey();
            aes.GenerateIV();

            byte[] clave = aes.Key;
            byte[] iv = aes.IV;

            Console.WriteLine("=== COMIENZA CIFRADO ===");
            byte[] textCifrado = Cifrar(textaACifrar, clave, iv);
            Console.WriteLine(Convert.ToBase64String(textCifrado));

            Console.WriteLine("\n=== DESCIFRANDO ===");
            string textoDescifrado = Descifrar(textCifrado, clave, iv);
            Console.WriteLine(textoDescifrado);
        }
    }

    static byte[] Cifrar(string texto, byte[] clave, byte[] iv)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = clave;
            aes.IV = iv;

            ICryptoTransform cifrador = aes.CreateEncryptor();
            using (MemoryStream ms = new MemoryStream())
            using (CryptoStream cs = new CryptoStream(ms, cifrador, CryptoStreamMode.Write))
            using (StreamWriter sw = new StreamWriter(cs))
            {
                sw.Write(texto);
                sw.Close();
                return ms.ToArray();
            }
        }
    }

    static string Descifrar(byte[] datosCifrados, byte[] clave, byte[] iv)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = clave;
            aes.IV = iv;

            ICryptoTransform descifrador = aes.CreateDecryptor();
            using (MemoryStream ms = new MemoryStream(datosCifrados))
            using (CryptoStream cs = new CryptoStream(ms, descifrador, CryptoStreamMode.Read))
            using (StreamReader sr = new StreamReader(cs))
            {
                return sr.ReadToEnd();
            }
        }
    }
}
