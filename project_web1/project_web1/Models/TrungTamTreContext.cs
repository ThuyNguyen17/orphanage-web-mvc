using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace project_web1.Models;

public partial class TrungTamTreContext : IdentityDbContext<ApplicationUser>
{
    public TrungTamTreContext()
    {
    }

    public TrungTamTreContext(DbContextOptions<TrungTamTreContext> options)
        : base(options)
    {
    }
    public DbSet<TodoItem> TodoItems { get; set; }

    public virtual DbSet<CtchiTieu> CtchiTieus { get; set; }

    public virtual DbSet<Ctdonthuoc> Ctdonthuocs { get; set; }

    public virtual DbSet<CtphieuMua> CtphieuMuas { get; set; }

    public virtual DbSet<DonThuoc> DonThuocs { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<HoSoSucKhoe> HoSoSucKhoes { get; set; }

    public virtual DbSet<HoaDonTaiTro> HoaDonTaiTros { get; set; }

    public virtual DbSet<LoaiChiTieu> LoaiChiTieus { get; set; }

    public virtual DbSet<NguoiChamSoc> NguoiChamSocs { get; set; }

    public virtual DbSet<NguoiNhanNuoi> NguoiNhanNuois { get; set; }

    public virtual DbSet<NhaTaiTro> NhaTaiTros { get; set; }

    public virtual DbSet<NhanVienYte> NhanVienYtes { get; set; }

    public virtual DbSet<PhieuChiTieu> PhieuChiTieus { get; set; }

    public virtual DbSet<PhieuKhamBenh> PhieuKhamBenhs { get; set; }

    public virtual DbSet<PhieuTiemChung> PhieuTiemChungs { get; set; }

    public virtual DbSet<Thuoc> Thuocs { get; set; }

    public virtual DbSet<ThuocMuaNgoai> ThuocMuaNgoais { get; set; }

    public virtual DbSet<Tre> Tres { get; set; }

    public virtual DbSet<Vaccine> Vaccines { get; set; }

  
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
        {
            entity.HasKey(e => e.UserId); 
        });

        modelBuilder.Entity<CtchiTieu>(entity =>
        {
            entity.HasKey(e => new { e.MaPhieuChiTieu, e.ChiTieuId }).HasName("PK__CTChiTie__98E08775690AB346");

            entity.ToTable("CTChiTieu");

            entity.Property(e => e.ChiTieuId).HasColumnName("ChiTieuID");
            entity.Property(e => e.SoTien).HasColumnType("decimal(15, 2)");

            entity.HasOne(d => d.ChiTieu).WithMany(p => p.CtchiTieus)
                .HasForeignKey(d => d.ChiTieuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CTChiTieu__ChiTi__6C190EBB");

            entity.HasOne(d => d.MaPhieuChiTieuNavigation).WithMany(p => p.CtchiTieus)
                .HasForeignKey(d => d.MaPhieuChiTieu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CTChiTieu__MaPhi__6B24EA82");
        });

        modelBuilder.Entity<Ctdonthuoc>(entity =>
        {
            entity.HasKey(e => new { e.MaDonThuoc, e.MaSoThuoc }).HasName("PK__CTDonthu__211B1A7D825BA313");

            entity.ToTable("CTDonthuoc");

            entity.HasOne(d => d.MaDonThuocNavigation).WithMany(p => p.Ctdonthuocs)
                .HasForeignKey(d => d.MaDonThuoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CTDonthuo__MaDon__6A30C649");

            entity.HasOne(d => d.MaSoThuocNavigation).WithMany(p => p.Ctdonthuocs)
                .HasForeignKey(d => d.MaSoThuoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CTDonthuo__MaSoT__5CD6CB2B");
        });

        modelBuilder.Entity<CtphieuMua>(entity =>
        {
            entity.HasKey(e => new { e.MaSoThuoc, e.MaSoMuaThuoc }).HasName("PK__CTPhieuM__B00654CE229F77BA");

            entity.ToTable("CTPhieuMua");

            entity.Property(e => e.Gia).HasColumnType("decimal(15, 2)");

            entity.HasOne(d => d.MaSoMuaThuocNavigation).WithMany(p => p.CtphieuMuas)
                .HasForeignKey(d => d.MaSoMuaThuoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CTPhieuMu__MaSoM__693CA210");

            entity.HasOne(d => d.MaSoThuocNavigation).WithMany(p => p.CtphieuMuas)
                .HasForeignKey(d => d.MaSoThuoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CTPhieuMu__MaSoT__5DCAEF64");
        });

        modelBuilder.Entity<DonThuoc>(entity =>
        {
            entity.HasKey(e => e.MaDonThuoc).HasName("PK__DonThuoc__3EF99EE1EE7B2347");

            entity.ToTable("DonThuoc");

            entity.Property(e => e.GhiChu).HasColumnType("text");
            entity.Property(e => e.ThoiGianDung)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TrangThai)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.MaSoTreNavigation).WithMany(p => p.DonThuocs)
                .HasForeignKey(d => d.MaSoTre)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DonThuoc__MaSoTr__6477ECF3");
        });

        modelBuilder.Entity<HoSoSucKhoe>(entity =>
        {
            entity.HasKey(e => e.MaHoSo).HasName("PK__HoSoSucK__1666423CE1228521");

            entity.ToTable("HoSoSucKhoe");

            entity.Property(e => e.DiUng).HasColumnType("text");
            entity.Property(e => e.GhiChu).HasColumnType("text");
            entity.Property(e => e.Idnvyte).HasColumnName("IDNVYTe");
            entity.Property(e => e.KetLuan).HasColumnType("text");
            entity.Property(e => e.TienSuBenh).HasColumnType("text");
            entity.Property(e => e.TinhTrangSucKhoe).HasColumnType("text");

            entity.HasOne(d => d.IdnvyteNavigation).WithMany(p => p.HoSoSucKhoes)
                .HasForeignKey(d => d.Idnvyte)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HoSoSucKh__IDNVY__68487DD7");

            entity.HasOne(d => d.MaSoTreNavigation).WithMany(p => p.HoSoSucKhoes)
                .HasForeignKey(d => d.MaSoTre)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HoSoSucKh__MaSoT__656C112C");
        });

        modelBuilder.Entity<HoaDonTaiTro>(entity =>
        {
            entity.HasKey(e => e.MaSoPhieuHoaDon).HasName("PK__HoaDonTa__A3C734FB6D0262A1");

            entity.ToTable("HoaDonTaiTro");

            entity.Property(e => e.MaSoNtt).HasColumnName("MaSoNTT");
            entity.Property(e => e.NgayLap).HasColumnType("datetime");
            entity.Property(e => e.SoTien).HasColumnType("decimal(15, 2)");

            entity.HasOne(d => d.MaSoNttNavigation).WithMany(p => p.HoaDonTaiTros)
                .HasForeignKey(d => d.MaSoNtt)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HoaDonTai__MaSoN__66603565");
        });

        modelBuilder.Entity<LoaiChiTieu>(entity =>
        {
            entity.HasKey(e => e.ChiTieuId).HasName("PK__LoaiChiT__BF79296D3B799B76");

            entity.ToTable("LoaiChiTieu");

            entity.Property(e => e.ChiTieuId).HasColumnName("ChiTieuID");
            entity.Property(e => e.TenChiTieu).HasColumnType("text");
        });

        modelBuilder.Entity<NguoiChamSoc>(entity =>
        {
            entity.HasKey(e => e.MaNhanVien).HasName("PK__NguoiCha__77B2CA47B0F11B58");

            entity.ToTable("NguoiChamSoc");

            entity.HasIndex(e => e.Email, "UQ__NguoiCha__A9D105343FA2E38B").IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.HoTen)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ImageUrl).HasDefaultValue("");
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<NguoiNhanNuoi>(entity =>
        {
            entity.HasKey(e => e.MaSoNnn).HasName("PK__NguoiNha__7CDA1DBCBC4BEC9C");

            entity.ToTable("NguoiNhanNuoi");

            entity.HasIndex(e => e.Email, "UQ__NguoiNha__A9D1053421154CDA").IsUnique();

            entity.Property(e => e.MaSoNnn).HasColumnName("MaSoNNN");
            entity.Property(e => e.DiaChi).HasColumnType("text");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.GhiChu).HasColumnType("text");
            entity.Property(e => e.HoTen)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.NgaySinh).HasColumnType("datetime");
            entity.Property(e => e.Sdt)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("SDT");

            entity.HasOne(d => d.MaSoTreNavigation).WithMany(p => p.NguoiNhanNuois)
                .HasForeignKey(d => d.MaSoTre)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__NguoiNhan__MaSoT__6383C8BA");
        });

        modelBuilder.Entity<NhaTaiTro>(entity =>
        {
            entity.HasKey(e => e.MaSoNtt).HasName("PK__NhaTaiTr__7CDA6C8EAFF0EB15");

            entity.ToTable("NhaTaiTro");

            entity.Property(e => e.MaSoNtt).HasColumnName("MaSoNTT");
            entity.Property(e => e.DiaChi).HasColumnType("text");
            entity.Property(e => e.GhiChu).HasColumnType("text");
            entity.Property(e => e.NgaySinh).HasColumnType("datetime");
            entity.Property(e => e.Sdt)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("SDT");
            entity.Property(e => e.Ten)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.ToChuc)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<NhanVienYte>(entity =>
        {
            entity.HasKey(e => e.Idnvyte).HasName("PK__NhanVien__CAEFD41E4F0E7121");

            entity.ToTable("NhanVienYTe");

            entity.Property(e => e.Idnvyte).HasColumnName("IDNVYTe");
            entity.Property(e => e.ChucVu)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.DonVi)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.HoTen)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Loai)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PhieuChiTieu>(entity =>
        {
            entity.HasKey(e => e.MaPhieuChiTieu).HasName("PK__PhieuChi__531715E38175541F");

            entity.ToTable("PhieuChiTieu");

            entity.Property(e => e.Mota).HasColumnType("text");
            entity.Property(e => e.NgayGhi).HasColumnType("datetime");
            entity.Property(e => e.TongTien).HasColumnType("decimal(15, 2)");

            entity.HasOne(d => d.MaNhanVienNavigation).WithMany(p => p.PhieuChiTieus)
                .HasForeignKey(d => d.MaNhanVien)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PhieuChiT__MaNha__60A75C0F");
        });

        modelBuilder.Entity<PhieuKhamBenh>(entity =>
        {
            entity.HasKey(e => e.MaPhieuKham).HasName("PK__PhieuKha__FACA55DF6919B3EC");

            entity.ToTable("PhieuKhamBenh");

            entity.Property(e => e.Idnvyte).HasColumnName("IDNVYTe");
            entity.Property(e => e.NgayGhi).HasColumnType("datetime");
            entity.Property(e => e.TinhTrangSucKhoe).HasColumnType("text");

            entity.HasOne(d => d.IdnvyteNavigation).WithMany(p => p.PhieuKhamBenhs)
                .HasForeignKey(d => d.Idnvyte)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PhieuKham__IDNVY__6754599E");

            entity.HasOne(d => d.MaSoTreNavigation).WithMany(p => p.PhieuKhamBenhs)
                .HasForeignKey(d => d.MaSoTre)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PhieuKham__MaSoT__619B8048");
        });

        modelBuilder.Entity<PhieuTiemChung>(entity =>
        {
            entity.HasKey(e => e.MaSoTiem).HasName("PK__PhieuTie__1E41A5178A59F172");

            entity.ToTable("PhieuTiemChung");

            entity.HasIndex(e => e.MuiThu, "UQ__PhieuTie__02D60CDD47617401").IsUnique();

            entity.HasIndex(e => e.MaSoTre, "UQ__PhieuTie__712406C9CEAADAB5").IsUnique();

            entity.HasIndex(e => e.MaSoVaccine, "UQ__PhieuTie__B7D083B06E18FB0F").IsUnique();

            entity.Property(e => e.GhiChu).HasColumnType("text");
            entity.Property(e => e.NgayTiem).HasColumnType("datetime");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.MaSoTreNavigation).WithOne(p => p.PhieuTiemChung)
                .HasForeignKey<PhieuTiemChung>(d => d.MaSoTre)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PhieuTiem__MaSoT__628FA481");

            entity.HasOne(d => d.MaSoVaccineNavigation).WithOne(p => p.PhieuTiemChung)
                .HasForeignKey<PhieuTiemChung>(d => d.MaSoVaccine)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PhieuTiem__MaSoV__5EBF139D");
        });

        modelBuilder.Entity<Thuoc>(entity =>
        {
            entity.HasKey(e => e.MaSoThuoc).HasName("PK__Thuoc__FE2849C0D63FCA19");

            entity.ToTable("Thuoc");

            entity.Property(e => e.CongDung).HasColumnType("text");
            entity.Property(e => e.GhiChu).HasColumnType("text");
            entity.Property(e => e.HoatChat)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.LieuLuongDeXuat)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NgayHetHan).HasColumnType("datetime");
            entity.Property(e => e.TenThuoc)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ThuocMuaNgoai>(entity =>
        {
            entity.HasKey(e => e.MaSoMuaThuoc).HasName("PK__ThuocMua__E2E1D0E582AD8A7D");

            entity.ToTable("ThuocMuaNgoai");

            entity.Property(e => e.GhiChu).HasColumnType("text");
            entity.Property(e => e.NgayMua).HasColumnType("datetime");
            entity.Property(e => e.NoiMua)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.TongTien).HasColumnType("decimal(15, 2)");
        });

        modelBuilder.Entity<Tre>(entity =>
        {
            entity.HasKey(e => e.MaSoTre).HasName("PK__Tre__712406C85A2E070B");

            entity.ToTable("Tre");

            entity.Property(e => e.AnhDaiDien)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.GioiTinh)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.HoTen)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.NgayNhapTrungTam).HasColumnType("datetime");
            entity.Property(e => e.NgaySinh).HasColumnType("datetime");
            entity.Property(e => e.Quote).HasColumnName("quote");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.MaNhanVienNavigation).WithMany(p => p.Tres)
                .HasForeignKey(d => d.MaNhanVien)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tre__MaNhanVien__5FB337D6");
        });

        modelBuilder.Entity<Vaccine>(entity =>
        {
            entity.HasKey(e => e.MaSoVaccine).HasName("PK__Vaccine__B7D083B14B33D524");

            entity.ToTable("Vaccine");

            entity.Property(e => e.GhiChu).HasColumnType("text");
            entity.Property(e => e.NgayHetHan).HasColumnType("datetime");
            entity.Property(e => e.PhongBenh).HasColumnType("text");
            entity.Property(e => e.TenVaccine)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
