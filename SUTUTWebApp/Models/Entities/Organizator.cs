using System;
using System.Collections.Generic;

namespace SUTUTWebApp.Models.Entities;

public partial class Organizator
{
    public int OrganizatorId { get; set; }

    public string Ime { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string BrojMobitela { get; set; } = null!;

    public string Oib { get; set; } = null!;

    public string? WebStranica { get; set; }

    public string Opis { get; set; } = null!;

    public int VrstaOrganizatoraId { get; set; }

    public virtual ICollection<Utrka> Utrkas { get; set; } = new List<Utrka>();

    public virtual Vrstaorganizator VrstaOrganizatora { get; set; } = null!;
}
