using System.Diagnostics;
using lab1api.Models;
using Microsoft.EntityFrameworkCore;
using lab1api.Models;

namespace lab3api.Infrastructure.Repositories;

public partial class ApplicationContext : DbContext
{
	public ApplicationContext()
	{
	}

	public ApplicationContext(DbContextOptions<ApplicationContext> options)
		: base(options)
	{
	}

	public virtual DbSet<Course> Courses { get; set; }

	public virtual DbSet<Department> Departments { get; set; }

	public virtual DbSet<Group> Groups { get; set; }

	public virtual DbSet<Lecture> Lectures { get; set; }

	public virtual DbSet<LectureMaterial> LectureMaterials { get; set; }

	public virtual DbSet<LectureType> LectureTypes { get; set; }

	public virtual DbSet<Specialty> Specialties { get; set; }

	public virtual DbSet<Student> Students { get; set; }

	public virtual DbSet<Timetable> Timetables { get; set; }
	public virtual DbSet<Visits> Visits { get; set; }


	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https: //go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
	{
		optionsBuilder.UseNpgsql("Host=postgre;Database=university;Port=5432;Username=fokypoky;Password=toor");
		optionsBuilder.LogTo(m => Debug.WriteLine(m));
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Course>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("courses_pkey");

			entity.ToTable("courses");

			entity.HasIndex(e => e.Title, "courses_title_key").IsUnique();

			entity.Property(e => e.Id).HasColumnName("id");
			entity.Property(e => e.DescriptionId)
				.HasMaxLength(50)
				.HasColumnName("description_id");
			entity.Property(e => e.Duration).HasColumnName("duration");
			entity.Property(e => e.Title)
				.HasMaxLength(300)
				.HasColumnName("title");
		});

		modelBuilder.Entity<Department>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("departments_pkey");

			entity.ToTable("departments");

			entity.HasIndex(e => e.Title, "departments_title_key").IsUnique();

			entity.Property(e => e.Id).HasColumnName("id");
			entity.Property(e => e.InstituteId).HasColumnName("institute_id");
			entity.Property(e => e.MainSpecialityId).HasColumnName("main_speciality_id");
			entity.Property(e => e.Title)
				.HasMaxLength(200)
				.HasColumnName("title");

			entity.HasOne(d => d.MainSpeciality).WithMany(p => p.Departments)
				.HasForeignKey(d => d.MainSpecialityId)
				.HasConstraintName("departments_main_speciality_id_fkey");
		});

		modelBuilder.Entity<Group>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("groups_pkey");

			entity.ToTable("groups");

			entity.HasIndex(e => e.Number, "groups_number_key").IsUnique();

			entity.Property(e => e.Id).HasColumnName("id");
			entity.Property(e => e.DepartmentId).HasColumnName("department_id");
			entity.Property(e => e.Number)
				.HasMaxLength(50)
				.HasColumnName("number");
			entity.Property(e => e.SpecialityId).HasColumnName("speciality_id");

			entity.HasOne(d => d.Department).WithMany(p => p.Groups)
				.HasForeignKey(d => d.DepartmentId)
				.HasConstraintName("groups_department_id_fkey");

			entity.HasOne(d => d.Speciality).WithMany(p => p.Groups)
				.HasForeignKey(d => d.SpecialityId)
				.HasConstraintName("groups_speciality_id_fkey");
		});

		modelBuilder.Entity<Lecture>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("lectures_pkey");

			entity.ToTable("lectures");

			entity.Property(e => e.Id).HasColumnName("id");
			entity.Property(e => e.Annotation)
				.HasMaxLength(200)
				.HasColumnName("annotation");
			entity.Property(e => e.CourseId).HasColumnName("course_id");
			entity.Property(e => e.Requirements)
				.HasMaxLength(350)
				.HasColumnName("requirements");
			entity.Property(e => e.TypeId).HasColumnName("type_id");

			entity.HasOne(d => d.Course).WithMany(p => p.Lectures)
				.HasForeignKey(d => d.CourseId)
				.HasConstraintName("lectures_course_id_fkey");

			entity.HasOne(d => d.Type).WithMany(p => p.Lectures)
				.HasForeignKey(d => d.TypeId)
				.HasConstraintName("lectures_type_id_fkey");
		});

		modelBuilder.Entity<LectureMaterial>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("lecture_materials_pkey");

			entity.ToTable("lecture_materials");

			entity.Property(e => e.Id).HasColumnName("id");
			entity.Property(e => e.LectureId).HasColumnName("lecture_id");
			entity.Property(e => e.MaterialsId)
				.HasMaxLength(200)
				.HasColumnName("materials_id");

			entity.HasOne(d => d.Lecture).WithMany(p => p.LectureMaterials)
				.HasForeignKey(d => d.LectureId)
				.HasConstraintName("lecture_materials_lecture_id_fkey");
		});

		modelBuilder.Entity<LectureType>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("lecture_types_pkey");

			entity.ToTable("lecture_types");

			entity.Property(e => e.Id).HasColumnName("id");
			entity.Property(e => e.Type)
				.HasMaxLength(50)
				.HasColumnName("type");
		});

		modelBuilder.Entity<Specialty>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("specialties_pkey");

			entity.ToTable("specialties");

			entity.HasIndex(e => e.Code, "specialties_code_key").IsUnique();

			entity.HasIndex(e => e.Title, "specialties_title_key").IsUnique();

			entity.HasIndex(e => e.Code, "unique_code").IsUnique();

			entity.HasIndex(e => e.Title, "unique_title").IsUnique();

			entity.Property(e => e.Id).HasColumnName("id");
			entity.Property(e => e.Code)
				.HasMaxLength(50)
				.HasColumnName("code");
			entity.Property(e => e.StudyDuration).HasColumnName("study_duration");
			entity.Property(e => e.Title)
				.HasMaxLength(200)
				.HasColumnName("title");
		});

		modelBuilder.Entity<Student>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("students_pkey");

			entity.ToTable("students");

			entity.HasIndex(e => e.PassbookNumber, "students_passbook_number_key").IsUnique();

			entity.Property(e => e.Id).HasColumnName("id");
			entity.Property(e => e.GroupId).HasColumnName("group_id");
			entity.Property(e => e.Name)
				.HasMaxLength(200)
				.HasColumnName("name");
			entity.Property(e => e.PassbookNumber)
				.HasMaxLength(50)
				.HasColumnName("passbook_number");

			entity.HasOne(d => d.Group).WithMany(p => p.Students)
				.HasForeignKey(d => d.GroupId)
				.HasConstraintName("students_group_id_fkey");
		});

		modelBuilder.Entity<Timetable>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("timetable_pkey");

			entity.ToTable("timetable");

			entity.Property(e => e.Id).HasColumnName("id");
			entity.Property(e => e.Date)
				.HasColumnType("timestamp without time zone")
				.HasColumnName("date");
			entity.Property(e => e.GroupId).HasColumnName("group_id");
			entity.Property(e => e.LectureId).HasColumnName("lecture_id");

			entity.HasOne(d => d.Group).WithMany(p => p.Timetables)
				.HasForeignKey(d => d.GroupId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("timetable_group_id_fkey");

			entity.HasOne(d => d.Lecture).WithMany(p => p.Timetables)
				.HasForeignKey(d => d.LectureId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("timetable_lecture_id_fkey");
		});

		modelBuilder.Entity<Visits>(entity =>
		{
			entity.HasNoKey()
				.ToTable("visits");
			entity.HasIndex(e => e.Date);

			entity.Property(e => e.Date)
				.HasColumnType("timestamp without time zone")
				.HasColumnName("date");

			entity.Property(e => e.LectureId).HasColumnName("lecture_id");
			entity.Property(e => e.StudentId).HasColumnName("student_id");

			entity.HasOne(d => d.Lecture).WithMany()
				.HasForeignKey(d => d.LectureId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("visits_lecture_id_fkey");
			entity.HasOne(d => d.Student).WithMany()
				.HasForeignKey(d => d.StudentId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("visits_student_id_fkey");
		});

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
