using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SUTUTWebApp.Models.Entities;

public partial class MasterContext : DbContext
{
    public MasterContext()
    {
    }

    public MasterContext(DbContextOptions<MasterContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Atletskiklub> Atletskiklubs { get; set; }

    public virtual DbSet<Kategorija> Kategorijas { get; set; }

    public virtual DbSet<Organizator> Organizators { get; set; }

    public virtual DbSet<Rezultat> Rezultats { get; set; }

    public virtual DbSet<Statusrezultatum> Statusrezultata { get; set; }

    public virtual DbSet<Statusutrke> Statusutrkes { get; set; }

    public virtual DbSet<Tipkategorije> Tipkategorijes { get; set; }

    public virtual DbSet<Trening> Trenings { get; set; }

    public virtual DbSet<Trkac> Trkacs { get; set; }

    public virtual DbSet<Utrka> Utrkas { get; set; }

    public virtual DbSet<Vrstaorganizator> Vrstaorganizators { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Atletskiklub>(entity =>
        {
            entity.HasKey(e => e.AklubId).HasName("PK__ATLETSKI__4E23C839BDF30C02");

            entity.ToTable("ATLETSKIKLUB");

            entity.Property(e => e.AklubId).HasColumnName("AKlubId");
            entity.Property(e => e.Drzava)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Grad)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Ime)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Kategorija>(entity =>
        {
            entity.HasKey(e => e.KategorijaId).HasName("PK__KATEGORI__6C3B8FEEA9386602");

            entity.ToTable("KATEGORIJA");

            entity.Property(e => e.MaxBrojTrkaca).HasColumnName("Max_broj_trkaca");
            entity.Property(e => e.Naziv)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Startnina).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Tip).WithMany(p => p.Kategorijas)
                .HasForeignKey(d => d.TipId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__KATEGORIJ__TipId__4BCC3ABA");

            entity.HasOne(d => d.Utrka).WithMany(p => p.Kategorijas)
                .HasForeignKey(d => d.UtrkaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__KATEGORIJ__Utrka__4AD81681");
        });

        modelBuilder.Entity<Organizator>(entity =>
        {
            entity.HasKey(e => e.OrganizatorId).HasName("PK__ORGANIZA__7464266AB917FA53");

            entity.ToTable("ORGANIZATOR");

            entity.Property(e => e.BrojMobitela)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Broj_mobitela");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Ime)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Oib)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("OIB");
            entity.Property(e => e.Opis)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.WebStranica)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("Web_stranica");

            entity.HasOne(d => d.VrstaOrganizatora).WithMany(p => p.Organizators)
                .HasForeignKey(d => d.VrstaOrganizatoraId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ORGANIZAT__Vrsta__3B95D2F1");
        });

        modelBuilder.Entity<Rezultat>(entity =>
        {
            entity.HasKey(e => e.RezultatId).HasName("PK__REZULTAT__447B09FB9915679D");

            entity.ToTable("REZULTAT");

            entity.HasIndex(e => new { e.TrkacId, e.KategorijaId }, "UQ_Trkac_Kategorija").IsUnique();

            entity.Property(e => e.FinalnoVrijeme).HasColumnName("Finalno_vrijeme");

            entity.HasOne(d => d.Kategorija).WithMany(p => p.Rezultats)
                .HasForeignKey(d => d.KategorijaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__REZULTAT__Katego__536D5C82");

            entity.HasOne(d => d.StatusRezultata).WithMany(p => p.Rezultats)
                .HasForeignKey(d => d.StatusRezultataId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__REZULTAT__Status__51851410");

            entity.HasOne(d => d.Trkac).WithMany(p => p.Rezultats)
                .HasForeignKey(d => d.TrkacId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__REZULTAT__TrkacI__52793849");
        });

        modelBuilder.Entity<Statusrezultatum>(entity =>
        {
            entity.HasKey(e => e.StatusRezultataId).HasName("PK__STATUSRE__E1B46CB0331E663B");

            entity.ToTable("STATUSREZULTATA");

            entity.Property(e => e.Naziv)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Statusutrke>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__STATUSUT__C8EE20632978266A");

            entity.ToTable("STATUSUTRKE");

            entity.Property(e => e.Naziv)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Tipkategorije>(entity =>
        {
            entity.HasKey(e => e.TipId).HasName("PK__TIPKATEG__2DB1A1C81952CDB9");

            entity.ToTable("TIPKATEGORIJE");

            entity.Property(e => e.Naziv)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Trening>(entity =>
        {
            entity.HasKey(e => e.TreningId).HasName("PK__TRENING__3B04A8D3444F6F93");

            entity.ToTable("TRENING");

            entity.Property(e => e.Lokacija)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Trkac).WithMany(p => p.Trenings)
                .HasForeignKey(d => d.TrkacId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TRENING__TrkacId__5649C92D");
        });

        modelBuilder.Entity<Trkac>(entity =>
        {
            entity.HasKey(e => e.TrkacId).HasName("PK__TRKAC__1B133CCFD36A1F7E");

            entity.ToTable("TRKAC");

            entity.Property(e => e.AklubId).HasColumnName("AKlubId");
            entity.Property(e => e.DatumRodenja).HasColumnName("Datum_rodenja");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Ime)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nacionalnost)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Prezime)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Spol)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.Aklub).WithMany(p => p.Trkacs)
                .HasForeignKey(d => d.AklubId)
                .HasConstraintName("FK__TRKAC__AKlubId__46136164");
        });

        modelBuilder.Entity<Utrka>(entity =>
        {
            entity.HasKey(e => e.UtrkaId).HasName("PK__UTRKA__038172C35314E55A");

            entity.ToTable("UTRKA");

            entity.Property(e => e.Drzava)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Grad)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Naziv)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Organizator).WithMany(p => p.Utrkas)
                .HasForeignKey(d => d.OrganizatorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UTRKA__Organizat__405A880E");

            entity.HasOne(d => d.Status).WithMany(p => p.Utrkas)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UTRKA__StatusId__414EAC47");
        });

        modelBuilder.Entity<Vrstaorganizator>(entity =>
        {
            entity.HasKey(e => e.VrstaOrganizatoraId).HasName("PK__VRSTAORG__6B1325DB1F74A794");

            entity.ToTable("VRSTAORGANIZATOR");

            entity.Property(e => e.Naziv)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
