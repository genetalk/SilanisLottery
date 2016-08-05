using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace eSignLive.Lottery.Domain
{
    public class UserInput
    {
        [Required(ErrorMessage = "Client's first name is required")]
        public string Name { get; set; }

        public bool IsAutoGeneratingBall { get; set; }

        [Range(1, 50, ErrorMessage = "Invalid ball number")]
        public int? NumberOfBallChosen { get; set; }
    }
}
