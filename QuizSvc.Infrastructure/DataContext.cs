using QuizSvc.Domain.AuditableEntity;
namespace QuizSvc.Infrastructure;

public class DataContext : DbContext
{
    private readonly ICurrentUser _currentUser;
    public DataContext(DbContextOptions<DataContext> options, ICurrentUser currentUser) : base(options)
    {
        _currentUser = currentUser;
    }

    public DbSet<Campaign> Campaigns => Set<Campaign>();
    public DbSet<Domain.Entities.Quiz> Quizzes => Set<Domain.Entities.Quiz>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<Answer> Answers => Set<Answer>();
    public DbSet<Dimension> Dimensions => Set<Dimension>();
    public DbSet<AnswerDimensionScore> AnswerDimensionScores => Set<AnswerDimensionScore>();
    public DbSet<CampaignQuizSetting> CampaignQuizSettings => Set<CampaignQuizSetting>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Lead> Leads => Set<Lead>();
    public DbSet<QuizAttempt> QuizAttempts => Set<QuizAttempt>();
    public DbSet<QuizResult> QuizResults => Set<QuizResult>();
    public DbSet<InteractionLog> InteractionLogs => Set<InteractionLog>();
    public DbSet<ScoringRule> ScoringRules => Set<ScoringRule>();
    public DbSet<LeadEngagementProfile> LeadEngagementProfiles => Set<LeadEngagementProfile>();
    public DbSet<Recommendation> Recommendations => Set<Recommendation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasPostgresExtension("unaccent");

        modelBuilder.Entity<Campaign>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.HasIndex(p => p.Code).IsUnique();
            entity.Property(p => p.Name).IsRequired();
            entity.Property(p => p.Code).IsRequired();
            entity.Property(p => p.Status)
             .IsRequired()
             .HasSentinel(CampaignStatus.Unknown)
             .HasDefaultValue(CampaignStatus.Active);

            entity.Property(p => p.Tenant).IsRequired();
        });

        modelBuilder.Entity<Domain.Entities.Quiz>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Title).IsRequired();
            entity.Property(x => x.EstimateTime).IsRequired().HasPrecision(10, 2);

            entity.Property(p => p.Type)
             .IsRequired()
             .HasSentinel(QuizType.Unknown)
             .HasDefaultValue(QuizType.Graded);

            entity.Property(p => p.Source)
             .IsRequired()
             .HasSentinel(QuizSource.Unknown)
             .HasDefaultValue(QuizSource.Template);

        });

        modelBuilder.Entity<Dimension>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Title).IsRequired();

            // thiết lập quan hệ
            entity.HasOne(x => x.Quiz)  
                  .WithMany(p => p.Dimensions)
                  .HasForeignKey(p => p.QuizId) 
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Content).IsRequired();
            entity.Property(x => x.Order).IsRequired();
            entity.Property(p => p.Type)
                .IsRequired()
                .HasSentinel(QuizType.Unknown)
                .HasDefaultValue(QuizType.Graded);

            // thiết lập quan hệ
            entity.HasOne(x => x.Quiz)
                  .WithMany(p => p.Questions)
                  .HasForeignKey(p => p.QuizId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Answer>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Content).IsRequired();
            entity.Property(x => x.IsCorrect).IsRequired();
            entity.Property(x => x.Score).IsRequired().HasPrecision(10, 2);

            // thiết lập quan hệ
            entity.HasOne(x => x.Question)
                  .WithMany(p => p.Answers)
                  .HasForeignKey(p => p.QuestionId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<AnswerDimensionScore>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Score).IsRequired();
            entity.HasIndex(x => new { x.AnswerId, x.DimensionId }).IsUnique();
            entity.Property(x => x.Score).IsRequired().HasPrecision(10, 2);

            // thiết lập quan hệ
            entity.HasOne(x => x.Answer)
                  .WithMany(x => x.AnswerDimensionScores)
                  .HasForeignKey(p => p.AnswerId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Dimension)
                 .WithMany(x => x.AnswerDimensionScores)
                 .HasForeignKey(p => p.DimensionId)
                 .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<CampaignQuizSetting>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.IsActive).IsRequired();
            entity.HasIndex(x => new { x.QuizId, x.CampaignId }).IsUnique();
            entity.Property(e => e.LeadCollectionPolicy)
              .HasColumnType("jsonb");

            entity.Property(e => e.CheckpointConfig)
                  .HasColumnType("jsonb");

            // thiết lập quan hệ
            entity.HasOne(x => x.Campaign)
                  .WithMany(x => x.CampaignQuizSettings)
                  .HasForeignKey(p => p.CampaignId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Quiz)
                 .WithMany(x => x.CampaignQuizSettings)
                 .HasForeignKey(p => p.QuizId)
                 .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.UserId).IsRequired();
            entity.HasIndex(x => x.UserId).IsUnique();

            entity.Property(x => x.Username).IsRequired();
            entity.Property(x => x.Tenant).IsRequired();
        });

        modelBuilder.Entity<Lead>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.LeadId).IsRequired();
          
            // thiết lập quan hệ
            entity.HasOne(x => x.LinkedUser)
                  .WithOne(p => p.Lead)
                  .HasForeignKey<Lead>(p => p.LinkedUserId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<QuizAttempt>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.CampaignQuizSettingId).IsRequired();
            //entity.HasIndex(x => new { x.CampaignQuizSettingId, x.LeadId }).IsUnique();
            entity.Property(x => x.StartedAt).IsRequired();
            entity.Property(p => p.AttemptStatus)
               .IsRequired()
               .HasSentinel(AttemptStatus.Unknown)
               .HasDefaultValue(AttemptStatus.InProgress);
            entity.Property(x => x.UserAnswers)
            .HasColumnType("jsonb");

            //thiết lập quan hệ
            entity.HasOne(x => x.CampaignQuizSetting)
                  .WithMany(x => x.QuizAttempts)
                  .HasForeignKey(x => x.CampaignQuizSettingId)
                  .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(x => x.User)
                  .WithMany(x => x.QuizAttempts)
                  .HasForeignKey(x => x.UserId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Lead)
                 .WithMany(x => x.QuizAttempts)
                 .HasForeignKey(x => x.LeadId)
                 .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.CurrentQuestion)
                .WithMany(x => x.QuizAttempts)
                .HasForeignKey(x => x.CurrentQuestionId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<QuizResult>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.TotalScore)
            .IsRequired()
            .HasPrecision(10, 2);

            entity.Property(e => e.DimensionScoreResults)
                  .HasColumnType("jsonb");

            // thiết lập quan hệ
            entity.HasOne(x => x.QuizAttempt)
                  .WithOne(p => p.QuizResult)
                  .HasForeignKey<QuizResult>(p => p.QuizAttemptId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<InteractionLog>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.LeadId).IsRequired();
            entity.Property(x => x.Tenant).IsRequired();
            entity.Property(x => x.EventType).IsRequired();
            entity.Property(x => x.IsProcessed).HasDefaultValue(false);

            entity.HasOne(x => x.QuizAttempt)
                  .WithMany()
                  .HasForeignKey(x => x.QuizAttemptId)
                  .IsRequired(false)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ScoringRule>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Tenant).IsRequired();
            entity.Property(x => x.RuleKey).IsRequired();
            entity.HasIndex(x => new { x.Tenant, x.RuleKey }).IsUnique();
        });

        modelBuilder.Entity<LeadEngagementProfile>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.LeadId).IsRequired();
            entity.HasIndex(x => x.LeadId).IsUnique();
            entity.Property(e => e.InterestAffinities)
                  .HasColumnType("jsonb");

            entity.HasOne(x => x.Lead)
                  .WithMany()
                  .HasForeignKey(x => x.LeadId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Recommendation>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Tenant).IsRequired();
            entity.Property(x => x.Title).IsRequired();
            entity.Property(p => p.Type)
               .IsRequired()
               .HasSentinel(RecommendationType.Unknown)
               .HasDefaultValue(RecommendationType.Career);

            entity.Property(e => e.JsonContent)
                  .HasColumnType("jsonb")
                  .IsRequired();
        });
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is AuditableEntity<Guid> &&
                        (e.State == EntityState.Added || e.State == EntityState.Modified));

        var currentUser = _currentUser.UserName ?? "System";
        // PostgreSQL bắt buộc phải dùng UtcNow
        var now = DateTime.UtcNow;

        foreach (var entry in entries)
        {
            var entity = (AuditableEntity<Guid>)entry.Entity;

            if (entry.State == EntityState.Added)
            {
                // Nếu CreatedAt chưa gán thì dùng now, nếu gán rồi thì ép về UTC
                entity.CreatedAt = entity.CreatedAt == default ? now : EnsureUtc(entity.CreatedAt);
                entity.CreatedBy = currentUser;

                entity.LastModifiedAt = null;
                entity.LastModifiedBy = null;
            }
            else if (entry.State == EntityState.Modified)
            {
                // Không cho phép sửa thông tin người tạo
                entry.Property(nameof(AuditableEntity<Guid>.CreatedAt)).IsModified = false;
                entry.Property(nameof(AuditableEntity<Guid>.CreatedBy)).IsModified = false;

                // Luôn cập nhật thời gian chỉnh sửa mới nhất
                entity.LastModifiedAt = now;
                entity.LastModifiedBy = currentUser;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    private static DateTime EnsureUtc(DateTime dateTime)
    {
        if (dateTime.Kind == DateTimeKind.Utc) return dateTime;
        // Ép kiểu Kind thành Utc để Postgres không báo lỗi
        return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
    }

}
