using System;
using System.Collections.Generic;

namespace SUTUTWebApp.Models.Entities;

public partial class Trening
{
    public int TreningId { get; set; }

    public string Lokacija { get; set; } = null!;

    public TimeOnly Trajanje { get; set; }

    public double Duljina { get; set; }

    public int TrkacId { get; set; }

    public virtual Trkac Trkac { get; set; } = null!;
}
