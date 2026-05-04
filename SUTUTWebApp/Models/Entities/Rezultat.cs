using System;
using System.Collections.Generic;

namespace SUTUTWebApp.Models.Entities;

public partial class Rezultat
{
    public int RezultatId { get; set; }

    public TimeOnly? FinalnoVrijeme { get; set; }

    public int StatusRezultataId { get; set; }

    public int TrkacId { get; set; }

    public int KategorijaId { get; set; }

    public virtual Kategorija Kategorija { get; set; } = null!;

    public virtual Statusrezultatum StatusRezultata { get; set; } = null!;

    public virtual Trkac Trkac { get; set; } = null!;
}
