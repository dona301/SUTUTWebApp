using System;
using System.Collections.Generic;

namespace SUTUTWebApp.Models.Entities;

public partial class Statusutrke
{
    public int StatusId { get; set; }

    public string Naziv { get; set; } = null!;

    public virtual ICollection<Utrka> Utrkas { get; set; } = new List<Utrka>();
}
