using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using MOI.Patrol;

namespace MOI.Patrol
{
    public partial class PatrolsContext : DbContext
    {
        public PatrolsContext()
        {
        }

        public PatrolsContext(DbContextOptions<PatrolsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Ahwal> Ahwal { get; set; }
        public virtual DbSet<Ahwalmapping> Ahwalmapping { get; set; }
        public virtual DbSet<Checkinoutstates> Checkinoutstates { get; set; }
        public virtual DbSet<Citygroups> Citygroups { get; set; }
        public virtual DbSet<Handhelds> Handhelds { get; set; }
        public virtual DbSet<Handheldscheckinout> Handheldscheckinout { get; set; }
        public virtual DbSet<Incidents> Incidents { get; set; }
        public virtual DbSet<Incidentscomments> Incidentscomments { get; set; }
        public virtual DbSet<Incidentsources> Incidentsources { get; set; }
        public virtual DbSet<Incidentstates> Incidentstates { get; set; }
        public virtual DbSet<Incidentstypes> Incidentstypes { get; set; }
        public virtual DbSet<Incidentsview> Incidentsview { get; set; }
        public virtual DbSet<Livecallers> Livecallers { get; set; }
        public virtual DbSet<Livecallersunknown> Livecallersunknown { get; set; }
        public virtual DbSet<OpenIddictApplications> OpenIddictApplications { get; set; }
        public virtual DbSet<OpenIddictAuthorizations> OpenIddictAuthorizations { get; set; }
        public virtual DbSet<OpenIddictScopes> OpenIddictScopes { get; set; }
        public virtual DbSet<OpenIddictTokens> OpenIddictTokens { get; set; }
        public virtual DbSet<Operationlogs> Operationlogs { get; set; }
        public virtual DbSet<Operations> Operations { get; set; }
        public virtual DbSet<Operationsstatus> Operationsstatus { get; set; }
        public virtual DbSet<Patrolcars> Patrolcars { get; set; }
        public virtual DbSet<Patrolcheckinout> Patrolcheckinout { get; set; }
        public virtual DbSet<Patrolpersonstatelog> Patrolpersonstatelog { get; set; }
        public virtual DbSet<Patrolpersonstates> Patrolpersonstates { get; set; }
        public virtual DbSet<Patrolroles> Patrolroles { get; set; }
        public virtual DbSet<Persons> Persons { get; set; }
        public virtual DbSet<Ranks> Ranks { get; set; }
        public virtual DbSet<Reservedcallers> Reservedcallers { get; set; }
        public virtual DbSet<Sectors> Sectors { get; set; }
        public virtual DbSet<Shifts> Shifts { get; set; }
        public virtual DbSet<Sysdiagrams> Sysdiagrams { get; set; }
        public virtual DbSet<Userpreference> Userpreference { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Usersroles> Usersroles { get; set; }
        public virtual DbSet<Usersrolesmap> Usersrolesmap { get; set; }

        // Unable to generate entity type for table 'public.devicescheckinout'. Please see the warning messages.
        // Unable to generate entity type for table 'public.devices'. Please see the warning messages.
        // Unable to generate entity type for table 'public.codemaster'. Please see the warning messages.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql("Host=localhost;Database=Patrols;Username=postgres;Password=12345");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ahwal>(entity =>
            {
                entity.ToTable("ahwal");

                entity.Property(e => e.Ahwalid)
                    .HasColumnName("ahwalid")
                    .HasDefaultValueSql("nextval('ahwal_seq'::regclass)");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<Ahwalmapping>(entity =>
            {
                entity.ToTable("ahwalmapping");

                entity.Property(e => e.Ahwalmappingid)
                    .HasColumnName("ahwalmappingid")
                    .HasDefaultValueSql("nextval('ahwalmapping_seq'::regclass)");

                entity.Property(e => e.Ahwalid).HasColumnName("ahwalid");

                entity.Property(e => e.Associtatepersonid).HasColumnName("associtatepersonid");

                entity.Property(e => e.Callerid)
                    .HasColumnName("callerid")
                    .HasMaxLength(50);

                entity.Property(e => e.Citygroupid).HasColumnName("citygroupid");

                entity.Property(e => e.Handheldid).HasColumnName("handheldid");

                entity.Property(e => e.Hasdevices).HasColumnName("hasdevices");

                entity.Property(e => e.Hasfixedcallerid).HasColumnName("hasfixedcallerid");

                entity.Property(e => e.Incidentid).HasColumnName("incidentid");

                entity.Property(e => e.Lastawaytimestamp).HasColumnName("lastawaytimestamp");

                entity.Property(e => e.Lastcomebacktimestamp).HasColumnName("lastcomebacktimestamp");

                entity.Property(e => e.Lastlandtimestamp).HasColumnName("lastlandtimestamp");

                entity.Property(e => e.Lastseatimestamp).HasColumnName("lastseatimestamp");

                entity.Property(e => e.Laststatechangetimestamp).HasColumnName("laststatechangetimestamp");

                entity.Property(e => e.Patrolid).HasColumnName("patrolid");

                entity.Property(e => e.Patrolpersonstateid).HasColumnName("patrolpersonstateid");

                entity.Property(e => e.Patrolroleid).HasColumnName("patrolroleid");

                entity.Property(e => e.Personid).HasColumnName("personid");

                entity.Property(e => e.Sectorid).HasColumnName("sectorid");

                entity.Property(e => e.Shiftid).HasColumnName("shiftid");

                entity.Property(e => e.Sortingindex)
                    .HasColumnName("sortingindex")
                    .HasDefaultValueSql("10000");

                entity.Property(e => e.Sunrisetimestamp).HasColumnName("sunrisetimestamp");

                entity.Property(e => e.Sunsettimestamp).HasColumnName("sunsettimestamp");

                entity.HasOne(d => d.Citygroup)
                    .WithMany(p => p.Ahwalmapping)
                    .HasForeignKey(d => d.Citygroupid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ahwalmapping_citygroups");

                entity.HasOne(d => d.Handheld)
                    .WithMany(p => p.Ahwalmapping)
                    .HasForeignKey(d => d.Handheldid)
                    .HasConstraintName("fk_ahwalmapping_handhelds");

                entity.HasOne(d => d.Incident)
                    .WithMany(p => p.Ahwalmapping)
                    .HasForeignKey(d => d.Incidentid)
                    .HasConstraintName("fk_ahwalmapping_incidents");

                entity.HasOne(d => d.Patrol)
                    .WithMany(p => p.Ahwalmapping)
                    .HasForeignKey(d => d.Patrolid)
                    .HasConstraintName("fk_ahwalmapping_patrolcars");

                entity.HasOne(d => d.Patrolpersonstate)
                    .WithMany(p => p.Ahwalmapping)
                    .HasForeignKey(d => d.Patrolpersonstateid)
                    .HasConstraintName("fk_ahwalmapping_patrolpersonstates");

                entity.HasOne(d => d.Patrolrole)
                    .WithMany(p => p.Ahwalmapping)
                    .HasForeignKey(d => d.Patrolroleid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ahwalmapping_patrolroles");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.Ahwalmapping)
                    .HasForeignKey(d => d.Personid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ahwalmapping_persons");

                entity.HasOne(d => d.Sector)
                    .WithMany(p => p.Ahwalmapping)
                    .HasForeignKey(d => d.Sectorid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ahwalmapping_sectors");

                entity.HasOne(d => d.Shift)
                    .WithMany(p => p.Ahwalmapping)
                    .HasForeignKey(d => d.Shiftid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ahwalmapping_shifts");
            });

            modelBuilder.Entity<Checkinoutstates>(entity =>
            {
                entity.HasKey(e => e.Checkinoutstateid);

                entity.ToTable("checkinoutstates");

                entity.Property(e => e.Checkinoutstateid)
                    .HasColumnName("checkinoutstateid")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<Citygroups>(entity =>
            {
                entity.HasKey(e => e.Citygroupid);

                entity.ToTable("citygroups");

                entity.Property(e => e.Citygroupid)
                    .HasColumnName("citygroupid")
                    .ValueGeneratedNever();

                entity.Property(e => e.Ahwalid).HasColumnName("ahwalid");

                entity.Property(e => e.Callerprefix)
                    .HasColumnName("callerprefix")
                    .HasMaxLength(50);

                entity.Property(e => e.Disabled).HasColumnName("disabled");

                entity.Property(e => e.Sectorid).HasColumnName("sectorid");

                entity.Property(e => e.Shortname)
                    .HasColumnName("shortname")
                    .HasMaxLength(50);

                entity.Property(e => e.Text)
                    .HasColumnName("text")
                    .HasMaxLength(4000);

                entity.HasOne(d => d.Ahwal)
                    .WithMany(p => p.Citygroups)
                    .HasForeignKey(d => d.Ahwalid)
                    .HasConstraintName("fk_citygroups_ahwal");

                entity.HasOne(d => d.Sector)
                    .WithMany(p => p.Citygroups)
                    .HasForeignKey(d => d.Sectorid)
                    .HasConstraintName("fk_citygroups_sectors");
            });

            modelBuilder.Entity<Handhelds>(entity =>
            {
                entity.HasKey(e => e.Handheldid);

                entity.ToTable("handhelds");

                entity.Property(e => e.Handheldid)
                    .HasColumnName("handheldid")
                    .HasDefaultValueSql("nextval('handhelds_seq'::regclass)");

                entity.Property(e => e.Ahwalid).HasColumnName("ahwalid");

                entity.Property(e => e.Barcode)
                    .IsRequired()
                    .HasColumnName("barcode")
                    .HasMaxLength(50);

                entity.Property(e => e.Defective).HasColumnName("defective");

                entity.Property(e => e.Serial)
                    .IsRequired()
                    .HasColumnName("serial")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Ahwal)
                    .WithMany(p => p.Handhelds)
                    .HasForeignKey(d => d.Ahwalid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_handhelds_ahwal");
            });

            modelBuilder.Entity<Handheldscheckinout>(entity =>
            {
                entity.HasKey(e => e.Handheldcheckinoutid);

                entity.ToTable("handheldscheckinout");

                entity.Property(e => e.Handheldcheckinoutid)
                    .HasColumnName("handheldcheckinoutid")
                    .HasDefaultValueSql("nextval('handheldscheckinout_seq'::regclass)");

                entity.Property(e => e.Checkinoutstateid).HasColumnName("checkinoutstateid");

                entity.Property(e => e.Handheldid).HasColumnName("handheldid");

                entity.Property(e => e.Personid).HasColumnName("personid");

                entity.Property(e => e.Timestamp).HasColumnName("timestamp");

                entity.HasOne(d => d.Checkinoutstate)
                    .WithMany(p => p.Handheldscheckinout)
                    .HasForeignKey(d => d.Checkinoutstateid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_handheldscheckinout_checkinoutstates");

                entity.HasOne(d => d.Handheld)
                    .WithMany(p => p.Handheldscheckinout)
                    .HasForeignKey(d => d.Handheldid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_handheldscheckinout_handhelds");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.Handheldscheckinout)
                    .HasForeignKey(d => d.Personid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_handheldscheckinout_persons");
            });

            modelBuilder.Entity<Incidents>(entity =>
            {
                entity.HasKey(e => e.Incidentid);

                entity.ToTable("incidents");

                entity.Property(e => e.Incidentid)
                    .HasColumnName("incidentid")
                    .HasDefaultValueSql("nextval('incidents_seq'::regclass)");

                entity.Property(e => e.Incidentsourceextrainfo1)
                    .HasColumnName("incidentsourceextrainfo1")
                    .HasMaxLength(4000);

                entity.Property(e => e.Incidentsourceextrainfo2)
                    .HasColumnName("incidentsourceextrainfo2")
                    .HasMaxLength(4000);

                entity.Property(e => e.Incidentsourceextrainfo3)
                    .HasColumnName("incidentsourceextrainfo3")
                    .HasMaxLength(4000);

                entity.Property(e => e.Incidentsourceid).HasColumnName("incidentsourceid");

                entity.Property(e => e.Incidentstateid).HasColumnName("incidentstateid");

                entity.Property(e => e.Incidenttypeid).HasColumnName("incidenttypeid");

                entity.Property(e => e.Lastupdate).HasColumnName("lastupdate");

                entity.Property(e => e.Place)
                    .HasColumnName("place")
                    .HasMaxLength(4000);

                entity.Property(e => e.Timestamp).HasColumnName("timestamp");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.HasOne(d => d.Incidentsource)
                    .WithMany(p => p.Incidents)
                    .HasForeignKey(d => d.Incidentsourceid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_incidents_incidentsources");

                entity.HasOne(d => d.Incidentstate)
                    .WithMany(p => p.Incidents)
                    .HasForeignKey(d => d.Incidentstateid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_incidents_incidentstates");

                entity.HasOne(d => d.Incidenttype)
                    .WithMany(p => p.Incidents)
                    .HasForeignKey(d => d.Incidenttypeid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_incidents_incidentstypes");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Incidents)
                    .HasForeignKey(d => d.Userid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_incidents_users");
            });

            modelBuilder.Entity<Incidentscomments>(entity =>
            {
                entity.HasKey(e => e.Incidentcommentid);

                entity.ToTable("incidentscomments");

                entity.Property(e => e.Incidentcommentid)
                    .HasColumnName("incidentcommentid")
                    .HasDefaultValueSql("nextval('incidentscomments_seq'::regclass)");

                entity.Property(e => e.Incidentid).HasColumnName("incidentid");

                entity.Property(e => e.Text)
                    .HasColumnName("text")
                    .HasMaxLength(4000);

                entity.Property(e => e.Timestamp).HasColumnName("timestamp");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.HasOne(d => d.Incident)
                    .WithMany(p => p.Incidentscomments)
                    .HasForeignKey(d => d.Incidentid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_incidentscomments_incidents");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Incidentscomments)
                    .HasForeignKey(d => d.Userid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_incidentscomments_users");
            });

            modelBuilder.Entity<Incidentsources>(entity =>
            {
                entity.HasKey(e => e.Incidentsourceid);

                entity.ToTable("incidentsources");

                entity.Property(e => e.Incidentsourceid)
                    .HasColumnName("incidentsourceid")
                    .ValueGeneratedNever();

                entity.Property(e => e.Extrainfo1)
                    .HasColumnName("extrainfo1")
                    .HasMaxLength(400);

                entity.Property(e => e.Extrainfo2)
                    .HasColumnName("extrainfo2")
                    .HasMaxLength(400);

                entity.Property(e => e.Extrainfo3)
                    .HasColumnName("extrainfo3")
                    .HasMaxLength(400);

                entity.Property(e => e.Mainextrainfonumber).HasColumnName("mainextrainfonumber");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(400);

                entity.Property(e => e.Requiresextrainfo1).HasColumnName("requiresextrainfo1");

                entity.Property(e => e.Requiresextrainfo2).HasColumnName("requiresextrainfo2");

                entity.Property(e => e.Requiresextrainfo3).HasColumnName("requiresextrainfo3");
            });

            modelBuilder.Entity<Incidentstates>(entity =>
            {
                entity.HasKey(e => e.Incidentstateid);

                entity.ToTable("incidentstates");

                entity.Property(e => e.Incidentstateid)
                    .HasColumnName("incidentstateid")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Incidentstypes>(entity =>
            {
                entity.HasKey(e => e.Incidenttypeid);

                entity.ToTable("incidentstypes");

                entity.Property(e => e.Incidenttypeid)
                    .HasColumnName("incidenttypeid")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(400);

                entity.Property(e => e.Priority).HasColumnName("priority");
            });

            modelBuilder.Entity<Incidentsview>(entity =>
            {
                entity.HasKey(e => new { e.Incidentid, e.Userid });

                entity.ToTable("incidentsview");

                entity.Property(e => e.Incidentid).HasColumnName("incidentid");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.Property(e => e.Timestamp).HasColumnName("timestamp");

                entity.HasOne(d => d.Incident)
                    .WithMany(p => p.Incidentsview)
                    .HasForeignKey(d => d.Incidentid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_incidentsview_incidents");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Incidentsview)
                    .HasForeignKey(d => d.Userid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_incidentsview_users");
            });

            modelBuilder.Entity<Livecallers>(entity =>
            {
                entity.HasKey(e => e.Livecallerid);

                entity.ToTable("livecallers");

                entity.Property(e => e.Livecallerid)
                    .HasColumnName("livecallerid")
                    .HasDefaultValueSql("nextval('livecallers_seq'::regclass)");

                entity.Property(e => e.Handheldid).HasColumnName("handheldid");

                entity.Property(e => e.Processed).HasColumnName("processed");

                entity.Property(e => e.Timestamp).HasColumnName("timestamp");

                entity.HasOne(d => d.Handheld)
                    .WithMany(p => p.Livecallers)
                    .HasForeignKey(d => d.Handheldid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_livecallers_handhelds");
            });

            modelBuilder.Entity<Livecallersunknown>(entity =>
            {
                entity.HasKey(e => e.Livecallerunknownid);

                entity.ToTable("livecallersunknown");

                entity.Property(e => e.Livecallerunknownid)
                    .HasColumnName("livecallerunknownid")
                    .HasDefaultValueSql("nextval('livecallersunknown_seq'::regclass)");

                entity.Property(e => e.Handheldnumbername)
                    .IsRequired()
                    .HasColumnName("handheldnumbername")
                    .HasMaxLength(500);

                entity.Property(e => e.Processed).HasColumnName("processed");

                entity.Property(e => e.Timestamp).HasColumnName("timestamp");
            });

            //modelBuilder.Entity<OpenIddictApplications>(entity =>
            //{
            //    entity.Property(e => e.Id)
            //        .HasMaxLength(450)
            //        .ValueGeneratedNever();

            //    entity.Property(e => e.ClientId)
            //        .IsRequired()
            //        .HasMaxLength(450);

            //    entity.Property(e => e.Type).IsRequired();
            //});

            //modelBuilder.Entity<OpenIddictAuthorizations>(entity =>
            //{
            //    entity.Property(e => e.Id)
            //        .HasMaxLength(450)
            //        .ValueGeneratedNever();

            //    entity.Property(e => e.ApplicationId).HasMaxLength(450);

            //    entity.Property(e => e.Status).IsRequired();

            //    entity.Property(e => e.Subject).IsRequired();

            //    entity.Property(e => e.Type).IsRequired();

            //    entity.HasOne(d => d.Application)
            //        .WithMany(p => p.OpenIddictAuthorizations)
            //        .HasForeignKey(d => d.ApplicationId)
            //        .HasConstraintName("FK_OpenIddictAuthorizations_OpenIddictApplications_ApplicationI");
            //});

            //modelBuilder.Entity<OpenIddictScopes>(entity =>
            //{
            //    entity.Property(e => e.Id)
            //        .HasMaxLength(450)
            //        .ValueGeneratedNever();

            //    entity.Property(e => e.Name)
            //        .IsRequired()
            //        .HasMaxLength(450);
            //});


            //modelBuilder.Entity<OpenIddictTokens>(entity =>
            //{
            //    entity.Property(e => e.Id)
            //        .HasMaxLength(450)
            //        .ValueGeneratedNever();

            //    entity.Property(e => e.ApplicationId).HasMaxLength(450);

            //    entity.Property(e => e.AuthorizationId).HasMaxLength(450);

            //    entity.Property(e => e.CreationDate).HasColumnType("timestamp(6) with time zone");

            //    entity.Property(e => e.ExpirationDate).HasColumnType("timestamp(6) with time zone");

            //    entity.Property(e => e.ReferenceId).HasMaxLength(450);

            //    entity.Property(e => e.Subject).IsRequired();

            //    entity.Property(e => e.Type).IsRequired();

            //    entity.HasOne(d => d.Application)
            //        .WithMany(p => p.OpenIddictTokens)
            //        .HasForeignKey(d => d.ApplicationId);

            //    entity.HasOne(d => d.Authorization)
            //        .WithMany(p => p.OpenIddictTokens)
            //        .HasForeignKey(d => d.AuthorizationId);
            //});


            modelBuilder.Entity<OpenIddictApplications>(entity =>
            {
                entity.ToTable("openiddictapplications");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(450)
                    .ValueGeneratedNever();

                entity.Property(e => e.ClientId)
                    .IsRequired()
                    .HasColumnName("clientid")
                    .HasMaxLength(450);

                entity.Property(e => e.ClientSecret).HasColumnName("clientsecret");

                entity.Property(e => e.ConcurrencyToken).HasColumnName("concurrencytoken");

                entity.Property(e => e.ConsentType).HasColumnName("consenttype");

                entity.Property(e => e.DisplayName).HasColumnName("displayname");

                entity.Property(e => e.Permissions).HasColumnName("permissions");

                entity.Property(e => e.PostLogoutRedirectUris).HasColumnName("postlogoutredirecturis");

                entity.Property(e => e.Properties).HasColumnName("properties");

                entity.Property(e => e.RedirectUris).HasColumnName("redirecturis");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnName("type");
            });

            modelBuilder.Entity<OpenIddictAuthorizations>(entity =>
            {
                entity.ToTable("openiddictauthorizations");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(450)
                    .ValueGeneratedNever();

                entity.Property(e => e.ApplicationId)
                    .HasColumnName("applicationid")
                    .HasMaxLength(450);

                entity.Property(e => e.ConcurrencyToken).HasColumnName("concurrencytoken");

                entity.Property(e => e.Properties).HasColumnName("properties");

                entity.Property(e => e.Scopes).HasColumnName("scopes");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status");

                entity.Property(e => e.Subject)
                    .IsRequired()
                    .HasColumnName("subject");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnName("type");

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.OpenIddictAuthorizations)
                    .HasForeignKey(d => d.ApplicationId)
                    .HasConstraintName("fk_openiddictauthorizations_openiddictapplications_applicationi");
            });

            modelBuilder.Entity<OpenIddictScopes>(entity =>
            {
                entity.ToTable("openiddictscopes");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(450)
                    .ValueGeneratedNever();

                entity.Property(e => e.ConcurrencyToken).HasColumnName("concurrencytoken");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.DisplayName).HasColumnName("displayname");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(450);

                entity.Property(e => e.Properties).HasColumnName("properties");

                entity.Property(e => e.Resources).HasColumnName("resources");
            });

            modelBuilder.Entity<OpenIddictTokens>(entity =>
            {
                entity.ToTable("openiddicttokens");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(450)
                    .ValueGeneratedNever();

                entity.Property(e => e.ApplicationId)
                    .HasColumnName("applicationid")
                    .HasMaxLength(450);

                entity.Property(e => e.AuthorizationId)
                    .HasColumnName("authorizationid")
                    .HasMaxLength(450);

                entity.Property(e => e.ConcurrencyToken).HasColumnName("concurrencytoken");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("creationdate")
                    .HasColumnType("timestamp(6) with time zone");

                entity.Property(e => e.ExpirationDate)
                    .HasColumnName("expirationdate")
                    .HasColumnType("timestamp(6) with time zone");

                entity.Property(e => e.Payload).HasColumnName("payload");

                entity.Property(e => e.Properties).HasColumnName("properties");

                entity.Property(e => e.ReferenceId)
                    .HasColumnName("referenceid")
                    .HasMaxLength(450);

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Subject)
                    .IsRequired()
                    .HasColumnName("subject");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnName("type");

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.OpenIddictTokens)
                    .HasForeignKey(d => d.ApplicationId)
                    .HasConstraintName("fk_openiddicttokens_openiddictapplications_applicationid");

                entity.HasOne(d => d.Authorization)
                    .WithMany(p => p.OpenIddictTokens)
                    .HasForeignKey(d => d.AuthorizationId)
                    .HasConstraintName("fk_openiddicttokens_openiddictauthorizations_authorizationid");
            });

            modelBuilder.Entity<Operationlogs>(entity =>
            {
                entity.HasKey(e => e.Logid);

                entity.ToTable("operationlogs");

                entity.Property(e => e.Logid)
                    .HasColumnName("logid")
                    .HasDefaultValueSql("nextval('operationlogs_seq'::regclass)");

                entity.Property(e => e.Operationid).HasColumnName("operationid");

                entity.Property(e => e.Statusid).HasColumnName("statusid");

                entity.Property(e => e.Text)
                    .HasColumnName("text")
                    .HasMaxLength(500);

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasDefaultValueSql("'2018-09-20 10:34:19.359033'::timestamp without time zone");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.HasOne(d => d.Operation)
                    .WithMany(p => p.Operationlogs)
                    .HasForeignKey(d => d.Operationid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_operationlogs_operations");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Operationlogs)
                    .HasForeignKey(d => d.Statusid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_operationlogs_operationsstatus");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Operationlogs)
                    .HasForeignKey(d => d.Userid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_operationlogs_users");
            });

            modelBuilder.Entity<Operations>(entity =>
            {
                entity.HasKey(e => e.Opeartionid);

                entity.ToTable("operations");

                entity.Property(e => e.Opeartionid)
                    .HasColumnName("opeartionid")
                    .HasDefaultValueSql("nextval('operations_seq'::regclass)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<Operationsstatus>(entity =>
            {
                entity.HasKey(e => e.Statusid);

                entity.ToTable("operationsstatus");

                entity.Property(e => e.Statusid)
                    .HasColumnName("statusid")
                    .HasDefaultValueSql("nextval('operationsstatus_seq'::regclass)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<Patrolcars>(entity =>
            {
                entity.HasKey(e => e.Patrolid);

                entity.ToTable("patrolcars");

                entity.Property(e => e.Patrolid)
                    .HasColumnName("patrolid")
                    .HasDefaultValueSql("nextval('patrolcars_seq'::regclass)");

                entity.Property(e => e.Ahwalid).HasColumnName("ahwalid");

                entity.Property(e => e.Barcode)
                    .IsRequired()
                    .HasColumnName("barcode")
                    .HasMaxLength(50);

                entity.Property(e => e.Defective).HasColumnName("defective");

                entity.Property(e => e.Delflag).HasColumnName("delflag");

                entity.Property(e => e.Model)
                    .HasColumnName("model")
                    .HasMaxLength(50);

                entity.Property(e => e.Platenumber)
                    .IsRequired()
                    .HasColumnName("platenumber")
                    .HasMaxLength(50);

                entity.Property(e => e.Rental).HasColumnName("rental");

                entity.Property(e => e.Typecode)
                    .HasColumnName("typecode")
                    .HasColumnType("character(3)");

                entity.Property(e => e.Vinnumber)
                    .HasColumnName("vinnumber")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Ahwal)
                    .WithMany(p => p.Patrolcars)
                    .HasForeignKey(d => d.Ahwalid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_patrolcars_ahwal");
            });

            modelBuilder.Entity<Patrolcheckinout>(entity =>
            {
                entity.ToTable("patrolcheckinout");

                entity.Property(e => e.Patrolcheckinoutid)
                    .HasColumnName("patrolcheckinoutid")
                    .HasDefaultValueSql("nextval('patrolcheckinout_seq'::regclass)");

                entity.Property(e => e.Checkinoutstateid).HasColumnName("checkinoutstateid");

                entity.Property(e => e.Patrolid).HasColumnName("patrolid");

                entity.Property(e => e.Personid).HasColumnName("personid");

                entity.Property(e => e.Timestamp).HasColumnName("timestamp");

                entity.HasOne(d => d.Checkinoutstate)
                    .WithMany(p => p.Patrolcheckinout)
                    .HasForeignKey(d => d.Checkinoutstateid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_patrolcheckinout_checkinoutstates");

                entity.HasOne(d => d.Patrol)
                    .WithMany(p => p.Patrolcheckinout)
                    .HasForeignKey(d => d.Patrolid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_patrolcheckinout_patrolcars");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.Patrolcheckinout)
                    .HasForeignKey(d => d.Personid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_patrolcheckinout_persons");
            });

            modelBuilder.Entity<Patrolpersonstatelog>(entity =>
            {
                entity.ToTable("patrolpersonstatelog");

                entity.Property(e => e.Patrolpersonstatelogid)
                    .HasColumnName("patrolpersonstatelogid")
                    .HasDefaultValueSql("nextval('patrolpersonstatelog_seq'::regclass)");

                entity.Property(e => e.Patrolpersonstateid).HasColumnName("patrolpersonstateid");

                entity.Property(e => e.Personid).HasColumnName("personid");

                entity.Property(e => e.Timestamp).HasColumnName("timestamp");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.HasOne(d => d.Patrolpersonstate)
                    .WithMany(p => p.Patrolpersonstatelog)
                    .HasForeignKey(d => d.Patrolpersonstateid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_patrolpersonstatelog_patrolpersonstates");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.Patrolpersonstatelog)
                    .HasForeignKey(d => d.Personid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_patrolpersonstatelog_persons");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Patrolpersonstatelog)
                    .HasForeignKey(d => d.Userid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_patrolpersonstatelog_users");
            });

            modelBuilder.Entity<Patrolpersonstates>(entity =>
            {
                entity.HasKey(e => e.Patrolpersonstateid);

                entity.ToTable("patrolpersonstates");

                entity.Property(e => e.Patrolpersonstateid)
                    .HasColumnName("patrolpersonstateid")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<Patrolroles>(entity =>
            {
                entity.HasKey(e => e.Patrolroleid);

                entity.ToTable("patrolroles");

                entity.Property(e => e.Patrolroleid)
                    .HasColumnName("patrolroleid")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Persons>(entity =>
            {
                entity.HasKey(e => e.Personid);

                entity.ToTable("persons");

                entity.Property(e => e.Personid)
                    .HasColumnName("personid")
                    .HasDefaultValueSql("nextval('persons_seq'::regclass)");

                entity.Property(e => e.Ahwalid).HasColumnName("ahwalid");

                entity.Property(e => e.Fixedcallerid)
                    .HasColumnName("fixedcallerid")
                    .HasMaxLength(50);

                entity.Property(e => e.Milnumber).HasColumnName("milnumber");

                entity.Property(e => e.Mobile)
                    .HasColumnName("mobile")
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(500);

                entity.Property(e => e.Rankid).HasColumnName("rankid");

                entity.HasOne(d => d.Ahwal)
                    .WithMany(p => p.Persons)
                    .HasForeignKey(d => d.Ahwalid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_persons_ahwal");

                entity.HasOne(d => d.Rank)
                    .WithMany(p => p.Persons)
                    .HasForeignKey(d => d.Rankid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_persons_ranks");
            });

            modelBuilder.Entity<Ranks>(entity =>
            {
                entity.HasKey(e => e.Rankid);

                entity.ToTable("ranks");

                entity.Property(e => e.Rankid)
                    .HasColumnName("rankid")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Reservedcallers>(entity =>
            {
                entity.HasKey(e => e.Reservedid);

                entity.ToTable("reservedcallers");

                entity.Property(e => e.Reservedid)
                    .HasColumnName("reservedid")
                    .HasDefaultValueSql("nextval('reservedcallers_seq'::regclass)");

                entity.Property(e => e.Reservedcallerid)
                    .HasColumnName("reservedcallerid")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Sectors>(entity =>
            {
                entity.HasKey(e => e.Sectorid);

                entity.ToTable("sectors");

                entity.Property(e => e.Sectorid)
                    .HasColumnName("sectorid")
                    .ValueGeneratedNever();

                entity.Property(e => e.Ahwalid).HasColumnName("ahwalid");

                entity.Property(e => e.Callerprefix)
                    .HasColumnName("callerprefix")
                    .HasColumnType("character(2)");

                entity.Property(e => e.Disabled).HasColumnName("disabled");

                entity.Property(e => e.Shortname)
                    .IsRequired()
                    .HasColumnName("shortname")
                    .HasMaxLength(500);

                entity.HasOne(d => d.Ahwal)
                    .WithMany(p => p.Sectors)
                    .HasForeignKey(d => d.Ahwalid)
                    .HasConstraintName("fk_sectors_ahwal");
            });

            modelBuilder.Entity<Shifts>(entity =>
            {
                entity.HasKey(e => e.Shiftid);

                entity.ToTable("shifts");

                entity.Property(e => e.Shiftid)
                    .HasColumnName("shiftid")
                    .HasDefaultValueSql("nextval('shifts_seq'::regclass)");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.Property(e => e.Numberofhours).HasColumnName("numberofhours");

                entity.Property(e => e.Startinghour).HasColumnName("startinghour");
            });

            modelBuilder.Entity<Sysdiagrams>(entity =>
            {
                entity.HasKey(e => e.DiagramId);

                entity.ToTable("sysdiagrams");

                entity.HasIndex(e => new { e.PrincipalId, e.Name })
                    .HasName("uk_principal_name")
                    .IsUnique();

                entity.Property(e => e.DiagramId)
                    .HasColumnName("diagram_id")
                    .HasDefaultValueSql("nextval('sysdiagrams_seq'::regclass)");

                entity.Property(e => e.Definition).HasColumnName("definition");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(128);

                entity.Property(e => e.PrincipalId).HasColumnName("principal_id");

                entity.Property(e => e.Version).HasColumnName("version");
            });

            modelBuilder.Entity<Userpreference>(entity =>
            {
                entity.ToTable("userpreference");

                entity.Property(e => e.Userpreferenceid)
                    .HasColumnName("userpreferenceid")
                    .ValueGeneratedNever();

                entity.Property(e => e.Defaulturl)
                    .HasColumnName("defaulturl")
                    .HasMaxLength(50);

                entity.Property(e => e.Theme)
                    .HasColumnName("theme")
                    .HasMaxLength(10);

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Userpreference)
                    .HasForeignKey(d => d.Userid)
                    .HasConstraintName("fk_userpreferenceid_users");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.Userid);

                entity.ToTable("users");

                entity.Property(e => e.Userid)
                    .HasColumnName("userid")
                    .HasDefaultValueSql("nextval('users_seq'::regclass)");

                entity.Property(e => e.Accountlocked).HasColumnName("accountlocked");

                entity.Property(e => e.Failedlogins).HasColumnName("failedlogins");

                entity.Property(e => e.Lastfailedlogin).HasColumnName("lastfailedlogin");

                entity.Property(e => e.Lastipaddress)
                    .HasColumnName("lastipaddress")
                    .HasMaxLength(50);

                entity.Property(e => e.Lastsuccesslogin).HasColumnName("lastsuccesslogin");

                entity.Property(e => e.LayoutAhwalmapping)
                    .HasColumnName("layout_ahwalmapping")
                    .HasMaxLength(4000);

                entity.Property(e => e.LayoutGroupsAhawalmapping)
                    .HasColumnName("layout_groups_ahawalmapping")
                    .HasMaxLength(4000);

                entity.Property(e => e.LayoutGroupsOpslivegrid)
                    .HasColumnName("layout_groups_opslivegrid")
                    .HasMaxLength(4000);

                entity.Property(e => e.LayoutOpslive)
                    .HasColumnName("layout_opslive")
                    .HasMaxLength(4000);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(500);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(500);

                entity.Property(e => e.Salt)
                    .IsRequired()
                    .HasColumnName("salt")
                    .HasMaxLength(50);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Usersroles>(entity =>
            {
                entity.HasKey(e => e.Userroleid);

                entity.ToTable("usersroles");

                entity.Property(e => e.Userroleid)
                    .HasColumnName("userroleid")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Usersrolesmap>(entity =>
            {
                entity.HasKey(e => new { e.Userid, e.Ahwalid, e.Userroleid });

                entity.ToTable("usersrolesmap");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.Property(e => e.Ahwalid).HasColumnName("ahwalid");

                entity.Property(e => e.Userroleid).HasColumnName("userroleid");

                entity.HasOne(d => d.Ahwal)
                    .WithMany(p => p.Usersrolesmap)
                    .HasForeignKey(d => d.Ahwalid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_usersrolesmap_ahwal");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Usersrolesmap)
                    .HasForeignKey(d => d.Userid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_usersrolesmap_users");

                entity.HasOne(d => d.Userrole)
                    .WithMany(p => p.Usersrolesmap)
                    .HasForeignKey(d => d.Userroleid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_usersrolesmap_usersroles");
            });

            modelBuilder.HasSequence("ahwal_seq").StartsAt(5);

            modelBuilder.HasSequence("ahwalmapping_seq").StartsAt(10145);

            modelBuilder.HasSequence("handhelds_seq").StartsAt(10010);

            modelBuilder.HasSequence("handheldscheckinout_seq").StartsAt(10020);

            modelBuilder.HasSequence("incidents_seq").StartsAt(24);

            modelBuilder.HasSequence("incidentscomments_seq").StartsAt(171);

            modelBuilder.HasSequence("livecallers_seq");

            modelBuilder.HasSequence("livecallersunknown_seq");

            modelBuilder.HasSequence("operationlogs_seq").StartsAt(21820);

            modelBuilder.HasSequence("operations_seq").StartsAt(35);

            modelBuilder.HasSequence("operationsstatus_seq").StartsAt(7);

            modelBuilder.HasSequence("patrolcars_seq").StartsAt(10023);

            modelBuilder.HasSequence("patrolcheckinout_seq").StartsAt(10020);

            modelBuilder.HasSequence("patrolpersonstatelog_seq").StartsAt(10209);

            modelBuilder.HasSequence("persons_seq").StartsAt(28);

            modelBuilder.HasSequence("reservedcallers_seq").StartsAt(4);

            modelBuilder.HasSequence("shifts_seq").StartsAt(4);

            modelBuilder.HasSequence("sysdiagrams_seq").StartsAt(2);

            modelBuilder.HasSequence("users_seq").StartsAt(9);
        }
    }
}
