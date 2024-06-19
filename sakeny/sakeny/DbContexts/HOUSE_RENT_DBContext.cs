using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using sakeny.Entities;

namespace sakeny.DbContexts
{
    public partial class HOUSE_RENT_DBContext : DbContext
    {
        public HOUSE_RENT_DBContext()
        {
        }

        public HOUSE_RENT_DBContext(DbContextOptions<HOUSE_RENT_DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<FeaturesTbl> FeaturesTbls { get; set; } = null!;
        public virtual DbSet<PostFeaturesTbl> PostFeaturesTbls { get; set; } = null!;
        public virtual DbSet<PostFeedbackTbl> PostFeedbackTbls { get; set; } = null!;
        public virtual DbSet<PostPicTbl> PostPicTbls { get; set; } = null!;
        public virtual DbSet<PostsTbl> PostsTbls { get; set; } = null!;
        public virtual DbSet<UserBanTbl> UserBanTbls { get; set; } = null!;
        public virtual DbSet<UserChatTbl> UserChatTbls { get; set; } = null!;
        public virtual DbSet<UserFeedbackTbl> UserFeedbackTbls { get; set; } = null!;
        public virtual DbSet<UsersTbl> UsersTbls { get; set; } = null!;
        public virtual DbSet<NotificationTbl> Notifications { get; set; } = null!;
        public virtual DbSet<PostFaviourateTbl> PostFaviourateTbls { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source = . ; Initial Catalog = HOUSE_RENT_DB; Integrated Security = SSPI ; TrustServerCertificate = True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("Arabic_CI_AS");

            modelBuilder.Entity<FeaturesTbl>(entity =>
            {
                entity.Property(e => e.FeaturesId).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<PostFeaturesTbl>(entity =>
            {
                entity.HasKey(pf => new { pf.FeaturesId, pf.PostId });

                entity.HasOne(d => d.Features)
                    .WithMany()
                    .HasForeignKey(d => d.FeaturesId)
                    .HasConstraintName("FK_POST_FEATURES_TBL_FEATURES_TBL");

                entity.HasOne(d => d.Post)
                    .WithMany()
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK_POST_FEATURES_TBL_POSTS_TBL");
            });

            modelBuilder.Entity<PostFeedbackTbl>(entity =>
            {
                entity.HasKey(e => e.PostFeedId)
                    .HasName("PK_POST_FEED_TBL");

                entity.Property(e => e.PostFeedId).ValueGeneratedOnAdd();

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostFeedbackTbls)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK_POST_FEEDBACK_TBL_POSTS_TBL");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PostFeedbackTbls)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_POST_FEEDBACK_TBL_USERS_TBL");
            });

            modelBuilder.Entity<PostPicTbl>(entity =>
            {
                entity.Property(e => e.PostPicId).ValueGeneratedOnAdd();

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostPicTbls)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK_POST_PIC_TBL_POSTS_TBL");
            });

            modelBuilder.Entity<PostsTbl>(entity =>
            {
                entity.Property(e => e.PostId).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<UserBanTbl>(entity =>
            {
                entity.Property(e => e.UserBanId).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<UserChatTbl>(entity =>
            {
                entity.Property(e => e.UserChatId).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<UserFeedbackTbl>(entity =>
            {
                entity.HasKey(e => e.FeedbackId)
                    .HasName("PK_FEEDBACKS_TBL");

                entity.Property(e => e.FeedbackId).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<UsersTbl>(entity =>
            {
                entity.Property(e => e.UserId).ValueGeneratedOnAdd();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
