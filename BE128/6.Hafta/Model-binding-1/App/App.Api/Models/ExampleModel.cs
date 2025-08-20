using Microsoft.AspNetCore.Mvc;

namespace App.Api.Models
{
    public class ExampleModel
    {
        [FromRoute(Name = "sayi1")]
        public int Sayi1 { get; set; }

        [FromQuery(Name = "sayi2")]
        public int Sayi2 { get; set; }

        [FromHeader(Name = "sayi3")]
        public int Sayi3 { get; set; }
    }
}
