namespace App.Api
{
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
