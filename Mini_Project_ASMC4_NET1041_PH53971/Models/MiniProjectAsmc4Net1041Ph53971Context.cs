using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Mini_Project_ASMC4_NET1041_PH53971.Models;

public partial class MiniProjectAsmc4Net1041Ph53971Context : DbContext
{
    public MiniProjectAsmc4Net1041Ph53971Context()
    {
    }

    public MiniProjectAsmc4Net1041Ph53971Context(DbContextOptions<MiniProjectAsmc4Net1041Ph53971Context> options)
        : base(options)
    {
    }

    public virtual DbSet<ChiTietCombo> ChiTietCombos { get; set; }

    public virtual DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }

    public virtual DbSet<ChiTietGioHang> ChiTietGioHangs { get; set; }

    public virtual DbSet<Combo> Combos { get; set; }

    public virtual DbSet<DonHang> DonHangs { get; set; }

    public virtual DbSet<GioHang> GioHangs { get; set; }

    public virtual DbSet<MonAn> MonAns { get; set; }

    public virtual DbSet<NguoiDung> NguoiDungs { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=ADMIN\\SQLEXPRESS;Database=Mini_Project_ASMC4_NET1041_PH53971;Trusted_Connection=True;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChiTietCombo>(entity =>
        {
            entity.HasKey(e => new { e.MaCombo, e.MaMonAn }).HasName("PK__ChiTietC__88FCCD1AF6756E4F");

            entity.ToTable("ChiTietCombo");

            entity.HasOne(d => d.MaComboNavigation).WithMany(p => p.ChiTietCombos)
                .HasForeignKey(d => d.MaCombo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietCo__MaCom__3E52440B");

            entity.HasOne(d => d.MaMonAnNavigation).WithMany(p => p.ChiTietCombos)
                .HasForeignKey(d => d.MaMonAn)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietCo__MaMon__3F466844");
        });

        modelBuilder.Entity<ChiTietDonHang>(entity =>
        {
            entity.HasKey(e => e.MaChiTietDonHang).HasName("PK__ChiTietD__4B0B45DD1A75C737");

            entity.ToTable("ChiTietDonHang");

            entity.HasOne(d => d.MaComboNavigation).WithMany(p => p.ChiTietDonHangs)
                .HasForeignKey(d => d.MaCombo)
                .HasConstraintName("FK__ChiTietDo__MaCom__48CFD27E");

            entity.HasOne(d => d.MaDonHangNavigation).WithMany(p => p.ChiTietDonHangs)
                .HasForeignKey(d => d.MaDonHang)
                .HasConstraintName("FK__ChiTietDo__MaDon__46E78A0C");

            entity.HasOne(d => d.MaMonAnNavigation).WithMany(p => p.ChiTietDonHangs)
                .HasForeignKey(d => d.MaMonAn)
                .HasConstraintName("FK__ChiTietDo__MaMon__47DBAE45");
        });

        modelBuilder.Entity<ChiTietGioHang>(entity =>
        {
            entity.HasKey(e => e.MaChiTietGioHang).HasName("PK__ChiTietG__BBF4749862250370");

            entity.ToTable("ChiTietGioHang");

            entity.Property(e => e.Loai).HasMaxLength(50);

            entity.HasOne(d => d.MaComboNavigation).WithMany(p => p.ChiTietGioHangs)
                .HasForeignKey(d => d.MaCombo)
                .HasConstraintName("FK__ChiTietGi__MaCom__5070F446");

            entity.HasOne(d => d.MaGioHangNavigation).WithMany(p => p.ChiTietGioHangs)
                .HasForeignKey(d => d.MaGioHang)
                .HasConstraintName("FK__ChiTietGi__MaGio__4E88ABD4");

            entity.HasOne(d => d.MaMonAnNavigation).WithMany(p => p.ChiTietGioHangs)
                .HasForeignKey(d => d.MaMonAn)
                .HasConstraintName("FK__ChiTietGi__MaMon__4F7CD00D");
        });

        modelBuilder.Entity<Combo>(entity =>
        {
            entity.HasKey(e => e.MaCombo).HasName("PK__Combo__C3EDBC780AF5616F");

            entity.ToTable("Combo");

            entity.Property(e => e.DuongDanHinh).HasMaxLength(255);
            entity.Property(e => e.MoTa).HasMaxLength(255);
            entity.Property(e => e.TenCombo).HasMaxLength(255);
        });

        modelBuilder.Entity<DonHang>(entity =>
        {
            entity.HasKey(e => e.MaDonHang).HasName("PK__DonHang__129584AD7B744637");

            entity.ToTable("DonHang");

            entity.Property(e => e.ThoiGianDat).HasColumnType("datetime");
            entity.Property(e => e.TrangThai).HasMaxLength(50);

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.MaNguoiDung)
                .HasConstraintName("FK__DonHang__MaNguoi__440B1D61");
        });

        modelBuilder.Entity<GioHang>(entity =>
        {
            entity.HasKey(e => e.MaGioHang).HasName("PK__GioHang__F5001DA35FAD0E4D");

            entity.ToTable("GioHang");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.GioHangs)
                .HasForeignKey(d => d.MaNguoiDung)
                .HasConstraintName("FK__GioHang__MaNguoi__4BAC3F29");
        });

        modelBuilder.Entity<MonAn>(entity =>
        {
            entity.HasKey(e => e.MaMonAn).HasName("PK__MonAn__B1171625E776C924");

            entity.ToTable("MonAn");

            entity.Property(e => e.DuongDanHinh).HasMaxLength(255);
            entity.Property(e => e.MoTa).HasMaxLength(255);
            entity.Property(e => e.TenMonAn).HasMaxLength(255);

            entity.HasOne(d => d.MaTagNavigation).WithMany(p => p.MonAns)
                .HasForeignKey(d => d.MaTag)
                .HasConstraintName("FK__MonAn__MaTag__398D8EEE");
        });

        modelBuilder.Entity<NguoiDung>(entity =>
        {
            entity.HasKey(e => e.MaNguoiDung).HasName("PK__NguoiDun__C539D7623D3865F7");

            entity.ToTable("NguoiDung");

            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.MatKhau).HasMaxLength(255);
            entity.Property(e => e.SoDienThoai).HasMaxLength(20);
            entity.Property(e => e.Ten).HasMaxLength(255);
            entity.Property(e => e.VaiTro).HasMaxLength(50);
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.MaTag).HasName("PK__Tag__314EC2148ECC02F7");

            entity.ToTable("Tag");

            entity.Property(e => e.TenTag).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
