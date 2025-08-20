namespace App.Api
{

    // Bir classın içinde olabilecek 3 yapı:
    // 1 - property({get; set;} şeklidne yazılır. İçinde değer tutmaz)
    // 2 - method (içinde işlem yapar)
    // 3 - constructor (classın ismiyle aynı olan, new ile çağrılan, classın ilk değerlerini atayan metot)

    public abstract class Hayvan
    {
        public string Renk { get; set; }

        public int AyakSayisi { get; set; }

        public void OzelIsim(string isim)
        {
            Console.WriteLine($"Bu hayvanın ismi: {isim}");
        }

        public abstract void SesCikar(); //contract

        public abstract void HareketEt(); //contract
    }

    public class Kedi : Hayvan
    {
        public override void SesCikar()
        {
            Console.WriteLine("Miyav!");
        }

        public override void HareketEt()
        {
            Console.WriteLine("Dört ayak üzerinde yürüdü.");
        }
    }

    public class Kus : Hayvan
    {
        public override void SesCikar()
        {
            Console.WriteLine("Cik cik!");
        }
        public override void HareketEt()
        {
            Console.WriteLine("Uçarak hareket etti.");
        }
    }

    public interface IUcan
    {
        void Uc();
    }

    public interface ISurungen
    {
        void Surun();
    }

    public interface ICanli
    {
        void NeredeYasar();
    }

    public class  Yilan : Hayvan
    {
        public override void SesCikar()
        {
            Console.WriteLine("Tsss!");
        }
        public override void HareketEt()
        {
            Console.WriteLine("Sürünerek hareket etti.");
        }

    }

}
