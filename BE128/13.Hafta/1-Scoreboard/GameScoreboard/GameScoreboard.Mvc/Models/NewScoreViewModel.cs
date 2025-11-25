using System.ComponentModel.DataAnnotations;

namespace GameScoreboard.Mvc.Models
{
    public class NewScoreViewModel
    {
        [Required, Range(0,int.MaxValue)]
        public int Score { get; set; }
    }
}
