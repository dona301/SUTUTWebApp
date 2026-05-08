using System.ComponentModel.DataAnnotations;

public class KategorijaRowVM
{
    public int KategorijaId { get; set; }
    public bool IsDeleted { get; set; }

    [Required(ErrorMessage = "Naziv kategorije je obavezan")]
    public string Naziv { get; set; } = "";

    [Required(ErrorMessage = "Duljina je obavezna")]
    [Range(0.1, 1000, ErrorMessage = "Duljina mora biti između 0.1 i 1000 km")]
    public double Duljina { get; set; }

    [Required(ErrorMessage = "Max broj trkača je obavezan")]
    [Range(1, 100000, ErrorMessage = "Mora biti najmanje 1 trkač")]
    public int MaxBrojTrkaca { get; set; }

    [Required(ErrorMessage = "Startnina je obavezna")]
    [Range(0, 100000, ErrorMessage = "Startnina ne može biti negativna")]
    public decimal Startnina { get; set; }

    [Required(ErrorMessage = "Datum početka je obavezan")]
    public DateOnly Pocetak { get; set; }

    [Required(ErrorMessage = "Tip kategorije je obavezan")]
    public int TipId { get; set; }
}