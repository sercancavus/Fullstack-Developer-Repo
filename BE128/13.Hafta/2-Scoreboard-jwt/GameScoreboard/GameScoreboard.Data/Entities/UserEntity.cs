using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameScoreboard.Data.Entities
{
    public class UserEntity : BaseEntity
    {
        [Required, StringLength(50, MinimumLength = 4)]
        public string Nickname { get; set; } = null!;
        [Required, MinLength(4)]
        public string Password { get; set; } = null!;
    }
}
