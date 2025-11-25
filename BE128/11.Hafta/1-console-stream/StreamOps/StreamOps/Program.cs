using System.Text;

namespace StreamOps
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            using (var stream = new MemoryStream())
            {
                var writer = new StreamWriter(stream);
                writer.Write("Hello, World!");
                writer.Flush(); // Flush metodu karakter karakter bu veriyi stream'e yazar.

                // Position = 0 diyerek cursor'ı en başa alıyoruz 
                stream.Position = 0;

                // 1)

                byte[] bytes = stream.ToArray();

                var byteStrings = string.Join(",", bytes);

                Console.WriteLine(byteStrings);

                // 2)

                char[] chars = Encoding.UTF8.GetChars(bytes);

                var charString = string.Join(",", chars);

                Console.WriteLine(charString);

                var charString2 = string.Join("", chars);

                Console.WriteLine(charString2);
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            //var fileContents = ReadAllFileContents("Students.txt");

            //Console.WriteLine(fileContents);

            Console.WriteLine("Dosya içeriğini giriniz : ");

            string fileContents = Console.ReadLine();

            Console.WriteLine($"{fileContents.Length} karakter girdiniz.");

            CreateTextFile("input.txt", fileContents);

            Console.WriteLine("dosya oluşturuldu, içeriğini görmek için enter'a basınız");

            var key = Console.ReadKey();

            if (key.Key != ConsoleKey.Enter)
            {
                return;
            }

            var fileContents2 = ReadAllFileContents("input.txt");

            Console.WriteLine(fileContents2);

        }

        static string ReadAllFileContents(string path)
        {
            // verdiğimiz path'deki dosyayı aç, sadece okuma yetkisi var.
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                List<byte> bytes = new();

                stream.Position = 0;

                while (stream.Position < stream.Length)
                {
                    bytes.Add((byte)stream.ReadByte());
                }

                string fileContents = Encoding.UTF8.GetString(bytes.ToArray());

                return fileContents;
            }
        }

        static void CreateTextFile(string path, string fileContents)
        {
            using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(fileContents);

                stream.Write(bytes);
                stream.Flush();
            }
        }

    }
}
