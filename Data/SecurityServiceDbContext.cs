using Microsoft.AspNetCore.Mvc.ViewEngines;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.Generic;
using System.Reflection.Emit;
using SecurityService.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Secu.Data
{
    public class SecurityServiceDbContext : DbContext
    {
        /// <summary>
        /// Constructor - receives database connection configuration from DI container
        /// Options include: connection string, database provider (SQL Server), logging settings
        /// </summary>
        public SecurityServiceDbContext(DbContextOptions<SecurityServiceDbContext> options) : base(options) { }

        // DbSet<T> represents a table in the database
        // Each DbSet allows you to query and save instances of that entity type
        public DbSet<Company> Companies { get; set; } = null!;           // Maps to 'Companies' table
        public DbSet<Profile> Profiles { get; set; } = null!;            // Maps to 'Profiles' table  
        public DbSet<SecurityOfficer> SecurityOfficers { get; set; } = null!;  // Maps to 'SecurityOfficers' table
        public DbSet<ServiceRequest> ServiceRequests { get; set; } = null!;     // Maps to 'ServiceRequests' table
        public DbSet<RequestResponse> RequestResponses { get; set; } = null!;   // Maps to 'RequestResponses' table
        public DbSet<Review> Reviews { get; set; } = null!;              // Maps to 'Reviews' table

        /// <summary>
        /// OnModelCreating - Called when EF creates the database model
        /// This is where we configure:
        /// 1. How enums are stored (as strings vs integers)
        /// 2. Relationships between tables (foreign keys)
        /// 3. Database constraints and indexes
        /// 4. Custom table/column configurations
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ===== ENUM CONFIGURATIONS =====
            // Store enums as strings in database instead of integers (more readable)
            modelBuilder.Entity<Profile>()
                .Property(e => e.Role)
                .HasConversion<string>();  // UserRole.CompanyAdmin → "CompanyAdmin"

            modelBuilder.Entity<ServiceRequest>()
                .Property(e => e.ServiceType)
                .HasConversion<string>();  // ServiceType.ArmedResponse → "ArmedResponse"

            modelBuilder.Entity<ServiceRequest>()
                .Property(e => e.Status)
                .HasConversion<string>();  // RequestStatus.Pending → "Pending"

            modelBuilder.Entity<SecurityOfficer>()
                .Property(e => e.Status)
                .HasConversion<string>();  // OfficerStatus.Available → "Available"

            // ===== FOREIGN KEY RELATIONSHIPS =====
            // Define how tables relate to each other and what happens when records are deleted

            // Company → Profile (Owner relationship)
            modelBuilder.Entity<Company>()
                .HasOne(c => c.Owner)                    // One company has one owner
                .WithMany(p => p.OwnedCompanies)         // One profile can own many companies
                .HasForeignKey(c => c.OwnerId)           // Foreign key: Company.OwnerId → Profile.Id
                .OnDelete(DeleteBehavior.Restrict);      // Can't delete profile if they own companies

            // SecurityOfficer → Profile (User relationship) - One-to-One
            modelBuilder.Entity<SecurityOfficer>()
                .HasOne(so => so.User)                   // One officer has one user profile
                .WithOne(p => p.SecurityOfficer)         // One profile can have one officer record
                .HasForeignKey<SecurityOfficer>(so => so.UserId)  // Foreign key on SecurityOfficer table
                .OnDelete(DeleteBehavior.Restrict);      // Can't delete profile if they're an officer

            // SecurityOfficer → Company (Employment relationship)
            modelBuilder.Entity<SecurityOfficer>()
                .HasOne(so => so.Company)                // One officer works for one company
                .WithMany(c => c.SecurityOfficers)       // One company has many officers
                .HasForeignKey(so => so.CompanyId)       // Foreign key: SecurityOfficer.CompanyId → Company.Id
                .OnDelete(DeleteBehavior.Restrict);      // Can't delete company if officers work there

            // ServiceRequest → Profile (Customer relationship)
            modelBuilder.Entity<ServiceRequest>()
                .HasOne(sr => sr.User)                   // One request has one customer
                .WithMany(p => p.ServiceRequests)        // One customer can have many requests
                .HasForeignKey(sr => sr.UserId)          // Foreign key: ServiceRequest.UserId → Profile.Id
                .OnDelete(DeleteBehavior.Restrict);      // Can't delete customer if they have requests

            // ServiceRequest → SecurityOfficer (Assignment relationship) - NULLABLE
            modelBuilder.Entity<ServiceRequest>()
                .HasOne(sr => sr.AssignedOfficer)        // One request can have one assigned officer
                .WithMany(so => so.AssignedRequests)     // One officer can have many assigned requests
                .HasForeignKey(sr => sr.OfficerId)       // Foreign key: ServiceRequest.OfficerId → SecurityOfficer.Id
                .OnDelete(DeleteBehavior.SetNull);       // If officer deleted, set OfficerId to null (request remains)

            // RequestResponse → ServiceRequest (Response to request relationship)
            modelBuilder.Entity<RequestResponse>()
                .HasOne(rr => rr.Request)                // One response belongs to one request
                .WithMany(sr => sr.Responses)            // One request can have many responses
                .HasForeignKey(rr => rr.RequestId)       // Foreign key: RequestResponse.RequestId → ServiceRequest.Id
                .OnDelete(DeleteBehavior.Cascade);       // Delete responses when request is deleted

            // RequestResponse → SecurityOfficer (Officer who responded)
            modelBuilder.Entity<RequestResponse>()
                .HasOne(rr => rr.Officer)                // One response from one officer
                .WithMany(so => so.Responses)            // One officer can make many responses
                .HasForeignKey(rr => rr.OfficerId)       // Foreign key: RequestResponse.OfficerId → SecurityOfficer.Id
                .OnDelete(DeleteBehavior.Restrict);      // Can't delete officer if they have responses

            // Review → ServiceRequest (Review for completed request) - One-to-One
            modelBuilder.Entity<Review>()
            .HasOne(r => r.Request)                  // One review for one request
                .WithOne(sr => sr.Review)                // One request can have one review
                .HasForeignKey<Review>(r => r.RequestId) // Foreign key on Review table
                .OnDelete(DeleteBehavior.Cascade);       // Delete review when request is deleted

            // Review → Profile (Customer who wrote review)
            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)                     // One review by one customer
                .WithMany(p => p.Reviews)                // One customer can write many reviews
                .HasForeignKey(r => r.UserId)            // Foreign key: Review.UserId → Profile.Id
                .OnDelete(DeleteBehavior.Restrict);      // Can't delete customer if they have reviews

            // Review → SecurityOfficer (Officer being reviewed)
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Officer)                  // One review about one officer
                .WithMany(so => so.Reviews)              // One officer can have many reviews
                .HasForeignKey(r => r.OfficerId)         // Foreign key: Review.OfficerId → SecurityOfficer.Id
                .OnDelete(DeleteBehavior.Restrict);      // Can't delete officer if they have reviews

            // ===== DATABASE INDEXES =====
            // Create indexes on frequently searched columns for better query performance

            // Unique constraint: Each badge number must be unique across all officers
            modelBuilder.Entity<SecurityOfficer>()
                .HasIndex(so => so.BadgeNumber)
                .IsUnique();

            // Unique constraint: Each company registration number must be unique
            modelBuilder.Entity<Company>()
                .HasIndex(c => c.RegistrationNumber)
                .IsUnique();

            // Index for fast filtering by request status (Pending, Completed, etc.)
            modelBuilder.Entity<ServiceRequest>()
                .HasIndex(sr => sr.Status);

            // Index for fast filtering by officer availability (Available, Busy, Offline)
            modelBuilder.Entity<SecurityOfficer>()
                .HasIndex(so => so.Status);

            modelBuilder.Entity<SecurityOfficer>()
                .Property(e => e.Specializations)
                .HasConversion(
                  v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions)null) ?? new List<string>()
            );
        }
    }

}
