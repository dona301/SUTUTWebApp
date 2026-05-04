using System;
using System.Collections.Generic;

namespace SUTUTWebApp.Models.Entities;

public partial class Trkac
{
    public int TrkacId { get; set; }

    public string Ime { get; set; } = null!;

    public string Prezime { get; set; } = null!;

    public DateOnly? DatumRodenja { get; set; }

    public string? Spol { get; set; }

    public string? Nacionalnost { get; set; }

    public string Email { get; set; } = null!;

    public int? AklubId { get; set; }

    public virtual Atletskiklub? Aklub { get; set; }

    public virtual ICollection<Rezultat> Rezultats { get; set; } = new List<Rezultat>();

    public virtual ICollection<Trening> Trenings { get; set; } = new List<Trening>();
}
