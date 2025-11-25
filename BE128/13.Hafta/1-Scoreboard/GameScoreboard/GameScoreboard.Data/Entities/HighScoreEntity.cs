using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace GameScoreboard.Data.Entities
{
    public class HighScoreEntity : BaseEntity
    {
        public int Score { get; set; }
        public long UserId  { get; set; }


        // navigation property
        [ForeignKey(nameof(UserId))]
        public UserEntity User { get; set; } = null!;
    }
}
