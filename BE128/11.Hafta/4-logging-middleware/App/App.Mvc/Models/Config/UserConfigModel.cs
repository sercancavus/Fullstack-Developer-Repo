namespace App.Mvc.Models.Config
{
    public class UserConfigModel
    {
        public string Name { get; set; } = null!;
        public int Age { get; set; }
        public UserAddressConfigModel? Address { get; set; }
    }

    public class UserAddressConfigModel
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
    }
}
