using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ContainerTrackingWebApi.Models
{
    public partial class containertracking_dk_db_containertrackingContext : DbContext
    {
        public containertracking_dk_db_containertrackingContext()
        {
        }

        public containertracking_dk_db_containertrackingContext(DbContextOptions<containertracking_dk_db_containertrackingContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AvailableContainers> AvailableContainers { get; set; }
        public virtual DbSet<ColumnsShow> ColumnsShow { get; set; }
        public virtual DbSet<CompanyAlias> CompanyAlias { get; set; }
        public virtual DbSet<ContainerApidata> ContainerApidata { get; set; }
        public virtual DbSet<CreateLog> CreateLog { get; set; }
        public virtual DbSet<EmailLogs> EmailLogs { get; set; }
        public virtual DbSet<NewsLetter> NewsLetter { get; set; }
        public virtual DbSet<NotificationsEmails> NotificationsEmails { get; set; }
        public virtual DbSet<Payment> Payment { get; set; }
        public virtual DbSet<Plan> Plan { get; set; }
        public virtual DbSet<Sealine> Sealine { get; set; }
        public virtual DbSet<SheetData> SheetData { get; set; }
        public virtual DbSet<SheetRowEmail> SheetRowEmail { get; set; }
        public virtual DbSet<ShipsgoContainer> ShipsgoContainer { get; set; }
        public virtual DbSet<UserNotifications> UserNotifications { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseSqlServer("Data Source=KINDLEBIT;Initial Catalog=containertracking_dk_db_containertracking;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");             
                optionsBuilder.UseSqlServer(Startup.GetConnectionString());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<AvailableContainers>(entity =>
            {
                entity.ToTable("available_containers");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Containers).HasColumnName("containers");

                entity.Property(e => e.PaymentId).HasColumnName("payment_id");

                entity.HasOne(d => d.Payment)
                    .WithMany(p => p.AvailableContainers)
                    .HasForeignKey(d => d.PaymentId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_available_containers_payment");
            });

            modelBuilder.Entity<ColumnsShow>(entity =>
            {
                entity.ToTable("columns_show");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Arrival)
                    .HasColumnName("arrival")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.BlReferenceNo).HasColumnName("bl_reference_no");

                entity.Property(e => e.ContainerNo)
                    .HasColumnName("container_no")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ContainerType)
                    .HasColumnName("container_type")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DaysBeforeArrival).HasColumnName("days_before_arrival");

                entity.Property(e => e.Departure)
                    .HasColumnName("departure")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Destination)
                    .HasColumnName("destination")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.EarlyDelay)
                    .HasColumnName("early_delay")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.EmptyReturnDate).HasColumnName("empty_return_date");

                entity.Property(e => e.FirstArrival)
                    .HasColumnName("first_arrival")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.FirstEta).HasColumnName("first_eta");

                entity.Property(e => e.FromCountry).HasColumnName("from_country");

                entity.Property(e => e.GetoutDate).HasColumnName("getout_date");

                entity.Property(e => e.Origin)
                    .HasColumnName("origin")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ShipmentBy).HasColumnName("shipment_by");

                entity.Property(e => e.ShipmentRef)
                    .HasColumnName("shipment_ref")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ShippingLine)
                    .HasColumnName("shipping_line")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ToCountry).HasColumnName("to_country");

                entity.Property(e => e.TransitPorts).HasColumnName("transit_ports");

                entity.Property(e => e.TransitTime).HasColumnName("transit_time");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.Vesselname).HasColumnName("vesselname");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ColumnsShow)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_columns_show_users");
            });

            modelBuilder.Entity<CompanyAlias>(entity =>
            {
                entity.ToTable("company_alias");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedBy).HasColumnName("createdBy");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("createdDate")
                    .HasColumnType("date");

                entity.Property(e => e.DeletedDate)
                    .HasColumnName("deletedDate")
                    .HasColumnType("date");

                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");

                entity.Property(e => e.ModifiedBy).HasColumnName("modifiedBy");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modifiedDate")
                    .HasColumnType("date");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(500);

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CompanyAlias)
                    .HasForeignKey(d => d.Userid)
                    .HasConstraintName("FK_company_alias_users");
            });

            modelBuilder.Entity<ContainerApidata>(entity =>
            {
                entity.ToTable("container_apidata");

                entity.Property(e => e.ApiData).HasColumnName("apiData");

                entity.Property(e => e.ContainerNo)
                    .HasColumnName("container_no")
                    .HasMaxLength(255);

                entity.Property(e => e.Day)
                    .HasColumnName("day")
                    .HasMaxLength(50);

                entity.Property(e => e.EmailStatus)
                    .HasColumnName("email_status")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Time)
                    .HasColumnName("time")
                    .HasMaxLength(50);

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ContainerApidata)
                    .HasForeignKey(d => d.Userid)
                    .HasConstraintName("FK_container_apidata_users");
            });

            modelBuilder.Entity<CreateLog>(entity =>
            {
                entity.ToTable("create_log");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LoginResult)
                    .IsRequired()
                    .HasColumnName("login_result")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LoginTime)
                    .HasColumnName("login_time")
                    .HasColumnType("datetime2(0)");

                entity.Property(e => e.LogoutTime)
                    .HasColumnName("logout_time")
                    .HasColumnType("datetime2(0)");

                entity.Property(e => e.SourceIp)
                    .IsRequired()
                    .HasColumnName("source_ip")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserAgent)
                    .IsRequired()
                    .HasColumnName("user_agent")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EmailLogs>(entity =>
            {
                entity.ToTable("email_logs");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ArrivalMail).HasColumnName("arrival_mail");

                entity.Property(e => e.ConAddMail).HasColumnName("con_add_mail");

                entity.Property(e => e.ContainerId).HasColumnName("container_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("datetime2(0)");

                entity.Property(e => e.DepartureMail).HasColumnName("departure_mail");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.EmailLogs)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_email_logs_users");
            });

            modelBuilder.Entity<NewsLetter>(entity =>
            {
                entity.ToTable("news_letter");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("datetime2(0)");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Status).HasColumnName("status");
            });

            modelBuilder.Entity<NotificationsEmails>(entity =>
            {
                entity.ToTable("notifications_emails");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ArrChangeEmails)
                    .IsRequired()
                    .HasColumnName("arr_change_emails")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ConAddEmails)
                    .IsRequired()
                    .HasColumnName("con_add_emails")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ConDelEmails)
                    .IsRequired()
                    .HasColumnName("con_del_emails")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ConTimeoutEmails)
                    .HasColumnName("con_timeout_emails")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("datetime2(0)");

                entity.Property(e => e.DepChangeEmails)
                    .IsRequired()
                    .HasColumnName("dep_change_emails")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UntilArrivalEmails)
                    .HasColumnName("until_arrival_emails")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("datetime2(0)");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.NotificationsEmails)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_notifications_emails_users");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("payment");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("datetime2(0)");

                entity.Property(e => e.ExpiryDate)
                    .HasColumnName("expiry_date")
                    .HasColumnType("datetime2(0)");

                entity.Property(e => e.PlanId).HasColumnName("plan_id");

                entity.Property(e => e.RowData)
                    .IsRequired()
                    .HasColumnName("row_data")
                    .IsUnicode(false);

                entity.Property(e => e.StartDate)
                    .HasColumnName("start_date")
                    .HasColumnType("datetime2(0)");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.SubAmount).HasColumnName("sub_amount");

                entity.Property(e => e.TotalAmount).HasColumnName("total_amount");

                entity.Property(e => e.TransactionId)
                    .IsRequired()
                    .HasColumnName("transaction_id")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.VatAmount).HasColumnName("vat_amount");

                entity.HasOne(d => d.Plan)
                    .WithMany(p => p.Payment)
                    .HasForeignKey(d => d.PlanId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_payment_plan");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Payment)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_payment_users");
            });

            modelBuilder.Entity<Plan>(entity =>
            {
                entity.ToTable("plan");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("datetime2(0)");

                entity.Property(e => e.MonthlyPrice)
                    .IsRequired()
                    .HasColumnName("monthly_price")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.OrgYearlyPrice)
                    .IsRequired()
                    .HasColumnName("org_yearly_price")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TrackingShipment)
                    .IsRequired()
                    .HasColumnName("tracking_shipment")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Vat).HasColumnName("vat");

                entity.Property(e => e.YearlyPrice)
                    .IsRequired()
                    .HasColumnName("yearly_price")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.YearlyTrackingShipment).HasColumnName("yearly_tracking_shipment");
            });

            modelBuilder.Entity<Sealine>(entity =>
            {
                entity.ToTable("sealine");

                entity.Property(e => e.Link)
                    .HasColumnName("link")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<SheetData>(entity =>
            {
                entity.ToTable("sheet_data");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Arrival)
                    .HasColumnName("arrival")
                    .HasColumnType("date");

                entity.Property(e => e.BlReferenceNo)
                    .IsRequired()
                    .HasColumnName("bl_reference_no")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ContainerId).HasColumnName("container_id");

                entity.Property(e => e.ContainerNumber)
                    .IsRequired()
                    .HasColumnName("container_number")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ContainerType)
                    .IsRequired()
                    .HasColumnName("container_type")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("datetime2(0)");

                entity.Property(e => e.Departure)
                    .HasColumnName("departure")
                    .HasColumnType("date");

                entity.Property(e => e.Destination)
                    .IsRequired()
                    .HasColumnName("destination")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.EarlyDelay)
                    .IsRequired()
                    .HasColumnName("early_delay")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.EmptyReturnDate)
                    .HasColumnName("empty_return_date")
                    .HasColumnType("date");

                entity.Property(e => e.FirstEta)
                    .IsRequired()
                    .HasColumnName("first_eta")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FromCountry)
                    .IsRequired()
                    .HasColumnName("from_country")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.GetoutDate)
                    .HasColumnName("getout_date")
                    .HasColumnType("date");

                entity.Property(e => e.Origin)
                    .IsRequired()
                    .HasColumnName("origin")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PoNo)
                    .IsRequired()
                    .HasColumnName("po_no")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.RowId)
                    .IsRequired()
                    .HasColumnName("row_id")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.SheetId)
                    .IsRequired()
                    .HasColumnName("sheet_id")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ShipmentBy)
                    .IsRequired()
                    .HasColumnName("shipment_by")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ShippingLine)
                    .IsRequired()
                    .HasColumnName("shipping_line")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ToCountry)
                    .IsRequired()
                    .HasColumnName("to_country")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TransitPorts)
                    .IsRequired()
                    .HasColumnName("transit_ports")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TransitTime)
                    .IsRequired()
                    .HasColumnName("transit_time")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SheetRowEmail>(entity =>
            {
                entity.ToTable("sheet_row_email");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ContainerId).HasColumnName("container_id");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.RowId)
                    .IsRequired()
                    .HasColumnName("row_id")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.SheetId)
                    .IsRequired()
                    .HasColumnName("sheet_id")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ShipsgoContainer>(entity =>
            {
                entity.ToTable("shipsgo_container");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Arrival)
                    .HasColumnName("arrival")
                    .HasColumnType("datetime2(0)");

                entity.Property(e => e.BlReferenceNo)
                    .IsRequired()
                    .HasColumnName("bl_reference_no")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CompanyName)
                    .HasColumnName("company_name")
                    .HasMaxLength(250);

                entity.Property(e => e.ContainerNo)
                    .IsRequired()
                    .HasColumnName("container_no")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ContainerType)
                    .IsRequired()
                    .HasColumnName("container_type")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("datetime2(0)");

                entity.Property(e => e.Day)
                    .HasColumnName("day")
                    .HasMaxLength(250);

                entity.Property(e => e.DaysBeforeArrival)
                    .HasColumnName("days_before_arrival")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Departure)
                    .HasColumnName("departure")
                    .HasColumnType("datetime2(0)");

                entity.Property(e => e.Destination)
                    .IsRequired()
                    .HasColumnName("destination")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.EarlyDelay)
                    .IsRequired()
                    .HasColumnName("early_delay")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.EmptyReturnDate)
                    .HasColumnName("empty_return_date")
                    .HasColumnType("date");

                entity.Property(e => e.Eta)
                    .IsRequired()
                    .HasColumnName("eta")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FirstArrival)
                    .HasColumnName("first_arrival")
                    .HasColumnType("datetime2(0)");

                entity.Property(e => e.FirstEta)
                    .HasColumnName("first_eta")
                    .HasColumnType("date");

                entity.Property(e => e.FromCountry)
                    .IsRequired()
                    .HasColumnName("from_country")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.GetoutDate)
                    .HasColumnName("getout_date")
                    .HasColumnType("date");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.Latfinal)
                    .HasColumnName("latfinal")
                    .HasMaxLength(250);

                entity.Property(e => e.Lngfinal)
                    .HasColumnName("lngfinal")
                    .HasMaxLength(250);

                entity.Property(e => e.Origin)
                    .IsRequired()
                    .HasColumnName("origin")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PoNo)
                    .IsRequired()
                    .HasColumnName("po_no")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ShipmentBy)
                    .IsRequired()
                    .HasColumnName("shipment_by")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ShippingLine)
                    .IsRequired()
                    .HasColumnName("shipping_line")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ShippingLink)
                    .HasColumnName("shipping_link")
                    .HasMaxLength(500);

                entity.Property(e => e.ShipsgoId)
                    .IsRequired()
                    .HasColumnName("shipsgo_id")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Time)
                    .HasColumnName("time")
                    .HasMaxLength(250);

                entity.Property(e => e.ToCountry)
                    .IsRequired()
                    .HasColumnName("to_country")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TransitPorts)
                    .IsRequired()
                    .HasColumnName("transit_ports")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TransitTime)
                    .IsRequired()
                    .HasColumnName("transit_time")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Type56)
                    .HasColumnName("type56")
                    .HasMaxLength(50);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("datetime2(0)");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ShipsgoContainer)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_shipsgo_container_users");
            });

            modelBuilder.Entity<UserNotifications>(entity =>
            {
                entity.ToTable("user_notifications");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ArrChangeSts).HasColumnName("arr_change_sts");

                entity.Property(e => e.ArrChangeTime).HasColumnName("arr_change_time");

                entity.Property(e => e.ConAddSts).HasColumnName("con_add_sts");

                entity.Property(e => e.ConAddTime).HasColumnName("con_add_time");

                entity.Property(e => e.ConDelSts).HasColumnName("con_del_sts");

                entity.Property(e => e.ConDelTime).HasColumnName("con_del_time");

                entity.Property(e => e.ConTimeoutSendsts).HasColumnName("con_timeout_sendsts");

                entity.Property(e => e.ConTimeoutSts).HasColumnName("con_timeout_sts");

                entity.Property(e => e.ConTimeoutTime).HasColumnName("con_timeout_time");

                entity.Property(e => e.ConUntilarrivalByEmail).HasColumnName("con_untilarrival_by_email");

                entity.Property(e => e.ConUntilarrivalDays).HasColumnName("con_untilarrival_days");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("datetime2(0)");

                entity.Property(e => e.DepChangeSts).HasColumnName("dep_change_sts");

                entity.Property(e => e.DepChangeTime).HasColumnName("dep_change_time");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("datetime2(0)");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserNotifications)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_user_notifications_users");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasColumnName("address")
                    .IsUnicode(false);

                entity.Property(e => e.CityName)
                    .IsRequired()
                    .HasColumnName("city_name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CompanyName)
                    .IsRequired()
                    .HasColumnName("company_name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CountryCode)
                    .IsRequired()
                    .HasColumnName("country_code")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CountryName)
                    .IsRequired()
                    .HasColumnName("country_name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("datetime2(0)");

                entity.Property(e => e.CvrNo)
                    .IsRequired()
                    .HasColumnName("cvr_no")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ExpiryDate)
                    .HasColumnName("expiry_date")
                    .HasColumnType("datetime2(0)");

                entity.Property(e => e.FName)
                    .IsRequired()
                    .HasColumnName("f_name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.InvitedId).HasColumnName("invited_id");

                entity.Property(e => e.LName)
                    .IsRequired()
                    .HasColumnName("l_name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNo)
                    .IsRequired()
                    .HasColumnName("phone_no")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ProfilePic)
                    .IsRequired()
                    .HasColumnName("profile_pic")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.SignupStatus).HasColumnName("signup_status");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.TotalContainer).HasColumnName("total_container");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("datetime2(0)");

                entity.Property(e => e.UserRole).HasColumnName("user_role");

                entity.Property(e => e.ZipCode)
                    .IsRequired()
                    .HasColumnName("zip_code")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });
        }
    }
}
