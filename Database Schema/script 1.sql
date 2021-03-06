USE [master]
GO
/****** Object:  Database [NotesMarketPlace]    Script Date: 2/17/2021 12:33:46 AM ******/
CREATE DATABASE [NotesMarketPlace]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'NotesMarketPlace', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\NotesMarketPlace.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'NotesMarketPlace_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\NotesMarketPlace_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [NotesMarketPlace] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [NotesMarketPlace].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [NotesMarketPlace] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET ARITHABORT OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [NotesMarketPlace] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [NotesMarketPlace] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET  ENABLE_BROKER 
GO
ALTER DATABASE [NotesMarketPlace] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [NotesMarketPlace] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET RECOVERY FULL 
GO
ALTER DATABASE [NotesMarketPlace] SET  MULTI_USER 
GO
ALTER DATABASE [NotesMarketPlace] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [NotesMarketPlace] SET DB_CHAINING OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [NotesMarketPlace] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [NotesMarketPlace] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'NotesMarketPlace', N'ON'
GO
ALTER DATABASE [NotesMarketPlace] SET QUERY_STORE = OFF
GO
USE [NotesMarketPlace]
GO
/****** Object:  Table [dbo].[Countries]    Script Date: 2/17/2021 12:33:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Countries](
	[CountryID] [int] IDENTITY(1,1) NOT NULL,
	[CountryName] [nvarchar](100) NOT NULL,
	[CountryCode] [nvarchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[CountryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Downloads]    Script Date: 2/17/2021 12:33:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Downloads](
	[DownloadID] [int] IDENTITY(1,1) NOT NULL,
	[NoteID] [int] NOT NULL,
	[SellerID] [int] NOT NULL,
	[BuyerID] [int] NOT NULL,
	[IsAllowed] [bit] NOT NULL,
	[AttachmentsID] [int] NOT NULL,
	[IsDownloaded] [bit] NOT NULL,
	[DownloadDate] [datetime] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[IsPaid] [bit] NOT NULL,
	[NoteTitle] [nvarchar](100) NULL,
	[NoteCategory] [int] NOT NULL,
	[PurchasedPrice] [decimal](18, 0) NULL,
PRIMARY KEY CLUSTERED 
(
	[DownloadID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NoteCategories]    Script Date: 2/17/2021 12:33:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NoteCategories](
	[CategoryID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](100) NOT NULL,
	[CategoryDescription] [nvarchar](max) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notes]    Script Date: 2/17/2021 12:33:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notes](
	[NoteID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[Category] [int] NOT NULL,
	[DisplayPicture] [nvarchar](500) NULL,
	[NoteDescription] [nvarchar](max) NOT NULL,
	[NoteType] [int] NULL,
	[NumberOfPages] [int] NULL,
	[PublishedDate] [datetime] NULL,
	[University] [nvarchar](200) NULL,
	[Country] [int] NULL,
	[Course] [nvarchar](100) NULL,
	[CourseCode] [nvarchar](100) NULL,
	[Professor] [nvarchar](100) NULL,
	[IsPaid] [bit] NOT NULL,
	[Price] [decimal](18, 0) NULL,
	[Preview] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[SellerID] [int] NOT NULL,
	[NoteStatus] [int] NOT NULL,
	[ActionBy] [int] NULL,
	[AdminRemarks] [nvarchar](max) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[NoteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NotesAttachments]    Script Date: 2/17/2021 12:33:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NotesAttachments](
	[AttachementID] [int] IDENTITY(1,1) NOT NULL,
	[NoteID] [int] NOT NULL,
	[FilesName] [nvarchar](100) NOT NULL,
	[FilePath] [nvarchar](max) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[AttachementID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NotesReports]    Script Date: 2/17/2021 12:33:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NotesReports](
	[ReportID] [int] IDENTITY(1,1) NOT NULL,
	[NoteID] [int] NOT NULL,
	[BuyerID] [int] NOT NULL,
	[DownloadID] [int] NOT NULL,
	[Remarks] [nvarchar](max) NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ReportID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NotesReviews]    Script Date: 2/17/2021 12:33:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NotesReviews](
	[ReviewID] [int] IDENTITY(1,1) NOT NULL,
	[NoteID] [int] NOT NULL,
	[BuyerID] [int] NOT NULL,
	[DownloadID] [int] NOT NULL,
	[Rating] [decimal](18, 0) NOT NULL,
	[Comment] [nvarchar](max) NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ReviewID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NoteTypes]    Script Date: 2/17/2021 12:33:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NoteTypes](
	[TypeID] [int] IDENTITY(1,1) NOT NULL,
	[TypeName] [nvarchar](100) NOT NULL,
	[TypeDescription] [nvarchar](max) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[TypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReferenceData]    Script Date: 2/17/2021 12:33:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReferenceData](
	[DataID] [int] IDENTITY(1,1) NOT NULL,
	[DataKey] [nvarchar](100) NOT NULL,
	[DataValue] [nvarchar](100) NOT NULL,
	[RefCategory] [nvarchar](100) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[DataID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SystemConfigurations]    Script Date: 2/17/2021 12:33:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemConfigurations](
	[ConfigID] [int] IDENTITY(1,1) NOT NULL,
	[ConfigKey] [nvarchar](100) NOT NULL,
	[ConfigValue] [nvarchar](500) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ConfigID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserProfile]    Script Date: 2/17/2021 12:33:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserProfile](
	[ProfileID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[DOB] [date] NULL,
	[Gender] [int] NULL,
	[SecondaryEmailAddress] [nvarchar](100) NULL,
	[CountryCode] [nvarchar](50) NULL,
	[PhoneNo] [nvarchar](15) NULL,
	[ProfilePicture] [nvarchar](500) NULL,
	[AddressLine1] [nvarchar](100) NOT NULL,
	[AddressLine2] [nvarchar](100) NOT NULL,
	[City] [nvarchar](50) NOT NULL,
	[State] [nvarchar](50) NOT NULL,
	[ZipCode] [nvarchar](50) NOT NULL,
	[Country] [nvarchar](50) NOT NULL,
	[University] [nvarchar](100) NOT NULL,
	[College] [nvarchar](100) NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ProfileID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRoles]    Script Date: 2/17/2021 12:33:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoles](
	[RoleID] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
	[RoleDescription] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 2/17/2021 12:33:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[Passwd] [nvarchar](24) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[RoleID] [int] NOT NULL,
	[IsEmailVerified] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Countries] ON 

INSERT [dbo].[Countries] ([CountryID], [CountryName], [CountryCode], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (1, N'India', N'+91', 1, 1, CAST(N'2021-02-16T23:29:20.547' AS DateTime), 1, CAST(N'2021-02-16T23:29:20.547' AS DateTime))
SET IDENTITY_INSERT [dbo].[Countries] OFF
GO
SET IDENTITY_INSERT [dbo].[Downloads] ON 

INSERT [dbo].[Downloads] ([DownloadID], [NoteID], [SellerID], [BuyerID], [IsAllowed], [AttachmentsID], [IsDownloaded], [DownloadDate], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsPaid], [NoteTitle], [NoteCategory], [PurchasedPrice]) VALUES (1, 1, 1, 1, 1, 1, 1, CAST(N'2021-02-17T00:18:43.260' AS DateTime), 1, CAST(N'2021-02-17T00:18:43.260' AS DateTime), 1, CAST(N'2021-02-17T00:18:43.260' AS DateTime), 1, N'First Book', 1, CAST(1234 AS Decimal(18, 0)))
SET IDENTITY_INSERT [dbo].[Downloads] OFF
GO
SET IDENTITY_INSERT [dbo].[NoteCategories] ON 

INSERT [dbo].[NoteCategories] ([CategoryID], [CategoryName], [CategoryDescription], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (1, N'Science', N'notes related to science field', 1, 1, CAST(N'2021-02-17T00:08:04.407' AS DateTime), 1, CAST(N'2021-02-17T00:08:04.407' AS DateTime))
SET IDENTITY_INSERT [dbo].[NoteCategories] OFF
GO
SET IDENTITY_INSERT [dbo].[Notes] ON 

INSERT [dbo].[Notes] ([NoteID], [Title], [Category], [DisplayPicture], [NoteDescription], [NoteType], [NumberOfPages], [PublishedDate], [University], [Country], [Course], [CourseCode], [Professor], [IsPaid], [Price], [Preview], [IsActive], [SellerID], [NoteStatus], [ActionBy], [AdminRemarks], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (1, N'First Book', 1, N'system-config/default-bookcover', N'this is trial book', 1, 5, CAST(N'2000-11-02T00:00:00.000' AS DateTime), N'abcd', 1, N'Trial Course', N'CO123', N'dr.strange', 1, CAST(1234 AS Decimal(18, 0)), N'notes/previews/1.pdf', 1, 1, 4, 1, N'abcd', 1, CAST(N'2021-02-17T00:13:40.307' AS DateTime), 1, CAST(N'2021-02-17T00:13:40.307' AS DateTime))
SET IDENTITY_INSERT [dbo].[Notes] OFF
GO
SET IDENTITY_INSERT [dbo].[NotesAttachments] ON 

INSERT [dbo].[NotesAttachments] ([AttachementID], [NoteID], [FilesName], [FilePath], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (1, 1, N'first-book.pdf', N'members/1/notes/first-book.pdf', 1, 1, CAST(N'2021-02-17T00:26:47.250' AS DateTime), 1, CAST(N'2021-02-17T00:26:47.250' AS DateTime))
SET IDENTITY_INSERT [dbo].[NotesAttachments] OFF
GO
SET IDENTITY_INSERT [dbo].[NotesReports] ON 

INSERT [dbo].[NotesReports] ([ReportID], [NoteID], [BuyerID], [DownloadID], [Remarks], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (1, 1, 1, 1, N'remarks', 1, CAST(N'2021-02-17T00:23:40.703' AS DateTime), 1, CAST(N'2021-02-17T00:23:40.703' AS DateTime))
SET IDENTITY_INSERT [dbo].[NotesReports] OFF
GO
SET IDENTITY_INSERT [dbo].[NotesReviews] ON 

INSERT [dbo].[NotesReviews] ([ReviewID], [NoteID], [BuyerID], [DownloadID], [Rating], [Comment], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (1, 1, 1, 1, CAST(4 AS Decimal(18, 0)), N'amazing note', 1, CAST(N'2021-02-17T00:20:42.553' AS DateTime), 1, CAST(N'2021-02-17T00:20:42.553' AS DateTime))
SET IDENTITY_INSERT [dbo].[NotesReviews] OFF
GO
SET IDENTITY_INSERT [dbo].[NoteTypes] ON 

INSERT [dbo].[NoteTypes] ([TypeID], [TypeName], [TypeDescription], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (1, N'Class Notes', N'Hand written notes from classes', 1, 1, CAST(N'2021-02-17T00:11:54.753' AS DateTime), 1, CAST(N'2021-02-17T00:11:54.753' AS DateTime))
SET IDENTITY_INSERT [dbo].[NoteTypes] OFF
GO
SET IDENTITY_INSERT [dbo].[ReferenceData] ON 

INSERT [dbo].[ReferenceData] ([DataID], [DataKey], [DataValue], [RefCategory], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (1, N'Male', N'M', N'gender', 1, 1, CAST(N'2021-02-16T23:54:22.970' AS DateTime), 1, CAST(N'2021-02-16T23:54:22.970' AS DateTime))
INSERT [dbo].[ReferenceData] ([DataID], [DataKey], [DataValue], [RefCategory], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (2, N'Female', N'F', N'gender', 1, 1, CAST(N'2021-02-16T23:55:15.527' AS DateTime), 1, CAST(N'2021-02-16T23:55:15.527' AS DateTime))
SET IDENTITY_INSERT [dbo].[ReferenceData] OFF
GO
SET IDENTITY_INSERT [dbo].[SystemConfigurations] ON 

INSERT [dbo].[SystemConfigurations] ([ConfigID], [ConfigKey], [ConfigValue], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (1, N'default-user-profile', N'system-config/default-user-profile.jpg', 1, 1, CAST(N'2021-02-17T00:30:17.580' AS DateTime), 1, CAST(N'2021-02-17T00:30:17.580' AS DateTime))
SET IDENTITY_INSERT [dbo].[SystemConfigurations] OFF
GO
SET IDENTITY_INSERT [dbo].[UserProfile] ON 

INSERT [dbo].[UserProfile] ([ProfileID], [UserID], [DOB], [Gender], [SecondaryEmailAddress], [CountryCode], [PhoneNo], [ProfilePicture], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [University], [College], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (1, 1, CAST(N'2000-01-01' AS Date), 1, N'superadmin2nd@notesmarketplace.com', N'+91', N'9876543210', N'system-config/default-user-profile.jpg', N'192.168.0.1', N'127.0.0.1', N'ssd', N'motherboard', N'000000', N'India', N'lenovo menufacturing unit,china', N'somewhere in china', 1, CAST(N'2021-02-16T23:49:09.137' AS DateTime), 1, CAST(N'2021-02-16T23:49:09.137' AS DateTime))
SET IDENTITY_INSERT [dbo].[UserProfile] OFF
GO
SET IDENTITY_INSERT [dbo].[UserRoles] ON 

INSERT [dbo].[UserRoles] ([RoleID], [RoleName], [RoleDescription], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (1, N'super admin', N'this user have all the permissions', 1, 1, CAST(N'2021-02-16T23:22:37.227' AS DateTime), 1, CAST(N'2021-02-16T23:22:37.227' AS DateTime))
INSERT [dbo].[UserRoles] ([RoleID], [RoleName], [RoleDescription], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (2, N'super admin', N'this user have all the permissions', 1, 1, CAST(N'2021-02-16T23:28:15.380' AS DateTime), 1, CAST(N'2021-02-16T23:28:15.380' AS DateTime))
SET IDENTITY_INSERT [dbo].[UserRoles] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([UserID], [Email], [FirstName], [LastName], [Passwd], [IsActive], [RoleID], [IsEmailVerified], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (1, N'superadmin@notesmarketplace.com', N'super', N'admin', N'toornimda', 1, 1, 1, 1, CAST(N'2021-02-16T23:46:44.993' AS DateTime), 1, CAST(N'2021-02-16T23:46:44.993' AS DateTime))
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Countrie__5D9B0D2CFB7B8C68]    Script Date: 2/17/2021 12:33:46 AM ******/
ALTER TABLE [dbo].[Countries] ADD UNIQUE NONCLUSTERED 
(
	[CountryCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Users__A9D10534EC2BA4C4]    Script Date: 2/17/2021 12:33:46 AM ******/
ALTER TABLE [dbo].[Users] ADD UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Countries] ADD  CONSTRAINT [CreatedDate_Countries]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Countries] ADD  CONSTRAINT [ModifiedDate_Countries]  DEFAULT (getdate()) FOR [ModifiedDate]
GO
ALTER TABLE [dbo].[Downloads] ADD  CONSTRAINT [CreatedDate_Downloads]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Downloads] ADD  CONSTRAINT [ModifiedDate_Downloads]  DEFAULT (getdate()) FOR [ModifiedDate]
GO
ALTER TABLE [dbo].[NoteCategories] ADD  CONSTRAINT [CreatedDate_NoteCategories]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[NoteCategories] ADD  CONSTRAINT [ModifiedDate_NoteCategories]  DEFAULT (getdate()) FOR [ModifiedDate]
GO
ALTER TABLE [dbo].[Notes] ADD  CONSTRAINT [CreatedDate_Notes]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Notes] ADD  CONSTRAINT [ModifiedDate_Notes]  DEFAULT (getdate()) FOR [ModifiedDate]
GO
ALTER TABLE [dbo].[NotesAttachments] ADD  CONSTRAINT [CreatedDate_NotesAttchments]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[NotesAttachments] ADD  CONSTRAINT [ModifiedDate_NotesAttchments]  DEFAULT (getdate()) FOR [ModifiedDate]
GO
ALTER TABLE [dbo].[NotesReports] ADD  CONSTRAINT [CreatedDate_NotesReports]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[NotesReports] ADD  CONSTRAINT [ModifiedDate_NotesReports]  DEFAULT (getdate()) FOR [ModifiedDate]
GO
ALTER TABLE [dbo].[NotesReviews] ADD  CONSTRAINT [CreatedDate_NotesReviews]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[NotesReviews] ADD  CONSTRAINT [ModifiedDate_NotesReviews]  DEFAULT (getdate()) FOR [ModifiedDate]
GO
ALTER TABLE [dbo].[NoteTypes] ADD  CONSTRAINT [CreatedDate_NoteTypes]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[NoteTypes] ADD  CONSTRAINT [ModifiedDate_NoteTypes]  DEFAULT (getdate()) FOR [ModifiedDate]
GO
ALTER TABLE [dbo].[ReferenceData] ADD  CONSTRAINT [CreatedDate_ReferenceData]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[ReferenceData] ADD  CONSTRAINT [ModifiedDate_ReferenceData]  DEFAULT (getdate()) FOR [ModifiedDate]
GO
ALTER TABLE [dbo].[SystemConfigurations] ADD  CONSTRAINT [CreatedDate_SystemConfigurations]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[SystemConfigurations] ADD  CONSTRAINT [ModifiedDate_SystemConfigurations]  DEFAULT (getdate()) FOR [ModifiedDate]
GO
ALTER TABLE [dbo].[UserProfile] ADD  CONSTRAINT [CreatedDate_UserProfile]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[UserProfile] ADD  CONSTRAINT [ModifiedDate_UserProfile]  DEFAULT (getdate()) FOR [ModifiedDate]
GO
ALTER TABLE [dbo].[UserRoles] ADD  CONSTRAINT [CreatedDate_UserRoles]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[UserRoles] ADD  CONSTRAINT [ModifiedDate_UserRoles]  DEFAULT (getdate()) FOR [ModifiedDate]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [CreatedDate_Users]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [ModifiedDate_Users]  DEFAULT (getdate()) FOR [ModifiedDate]
GO
ALTER TABLE [dbo].[Downloads]  WITH CHECK ADD  CONSTRAINT [Downloads_BuyerID_FK] FOREIGN KEY([BuyerID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Downloads] CHECK CONSTRAINT [Downloads_BuyerID_FK]
GO
ALTER TABLE [dbo].[Downloads]  WITH CHECK ADD  CONSTRAINT [Downloads_NoteCategory_FK] FOREIGN KEY([NoteCategory])
REFERENCES [dbo].[NoteCategories] ([CategoryID])
GO
ALTER TABLE [dbo].[Downloads] CHECK CONSTRAINT [Downloads_NoteCategory_FK]
GO
ALTER TABLE [dbo].[Downloads]  WITH CHECK ADD  CONSTRAINT [Downloads_NoteID_FK] FOREIGN KEY([NoteID])
REFERENCES [dbo].[Notes] ([NoteID])
GO
ALTER TABLE [dbo].[Downloads] CHECK CONSTRAINT [Downloads_NoteID_FK]
GO
ALTER TABLE [dbo].[Downloads]  WITH CHECK ADD  CONSTRAINT [Downloads_SellerID_FK] FOREIGN KEY([SellerID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Downloads] CHECK CONSTRAINT [Downloads_SellerID_FK]
GO
ALTER TABLE [dbo].[Notes]  WITH CHECK ADD  CONSTRAINT [Notes_Category_FK] FOREIGN KEY([Category])
REFERENCES [dbo].[NoteCategories] ([CategoryID])
GO
ALTER TABLE [dbo].[Notes] CHECK CONSTRAINT [Notes_Category_FK]
GO
ALTER TABLE [dbo].[Notes]  WITH CHECK ADD  CONSTRAINT [Notes_Type_FK] FOREIGN KEY([NoteType])
REFERENCES [dbo].[NoteTypes] ([TypeID])
GO
ALTER TABLE [dbo].[Notes] CHECK CONSTRAINT [Notes_Type_FK]
GO
ALTER TABLE [dbo].[Notes]  WITH CHECK ADD  CONSTRAINT [Notes_UserID_FK] FOREIGN KEY([SellerID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Notes] CHECK CONSTRAINT [Notes_UserID_FK]
GO
ALTER TABLE [dbo].[NotesAttachments]  WITH CHECK ADD  CONSTRAINT [NotesAttachments_NoteID_FK] FOREIGN KEY([NoteID])
REFERENCES [dbo].[Notes] ([NoteID])
GO
ALTER TABLE [dbo].[NotesAttachments] CHECK CONSTRAINT [NotesAttachments_NoteID_FK]
GO
ALTER TABLE [dbo].[NotesReports]  WITH CHECK ADD  CONSTRAINT [NoteReports_BuyerID_FK] FOREIGN KEY([BuyerID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[NotesReports] CHECK CONSTRAINT [NoteReports_BuyerID_FK]
GO
ALTER TABLE [dbo].[NotesReports]  WITH CHECK ADD  CONSTRAINT [NoteReports_DownloadID_FK] FOREIGN KEY([DownloadID])
REFERENCES [dbo].[Downloads] ([DownloadID])
GO
ALTER TABLE [dbo].[NotesReports] CHECK CONSTRAINT [NoteReports_DownloadID_FK]
GO
ALTER TABLE [dbo].[NotesReports]  WITH CHECK ADD  CONSTRAINT [NoteReports_NoteID_FK] FOREIGN KEY([NoteID])
REFERENCES [dbo].[Notes] ([NoteID])
GO
ALTER TABLE [dbo].[NotesReports] CHECK CONSTRAINT [NoteReports_NoteID_FK]
GO
ALTER TABLE [dbo].[NotesReviews]  WITH CHECK ADD  CONSTRAINT [NoteReviews_BuyerID_FK] FOREIGN KEY([BuyerID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[NotesReviews] CHECK CONSTRAINT [NoteReviews_BuyerID_FK]
GO
ALTER TABLE [dbo].[NotesReviews]  WITH CHECK ADD  CONSTRAINT [NoteReviews_DownloadID_FK] FOREIGN KEY([DownloadID])
REFERENCES [dbo].[Downloads] ([DownloadID])
GO
ALTER TABLE [dbo].[NotesReviews] CHECK CONSTRAINT [NoteReviews_DownloadID_FK]
GO
ALTER TABLE [dbo].[NotesReviews]  WITH CHECK ADD  CONSTRAINT [NoteReviews_NoteID_FK] FOREIGN KEY([NoteID])
REFERENCES [dbo].[Notes] ([NoteID])
GO
ALTER TABLE [dbo].[NotesReviews] CHECK CONSTRAINT [NoteReviews_NoteID_FK]
GO
ALTER TABLE [dbo].[UserProfile]  WITH CHECK ADD  CONSTRAINT [UserProfile_CountryCode_FK] FOREIGN KEY([CountryCode])
REFERENCES [dbo].[Countries] ([CountryCode])
GO
ALTER TABLE [dbo].[UserProfile] CHECK CONSTRAINT [UserProfile_CountryCode_FK]
GO
ALTER TABLE [dbo].[UserProfile]  WITH CHECK ADD  CONSTRAINT [UserProfile_UserID_FK] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[UserProfile] CHECK CONSTRAINT [UserProfile_UserID_FK]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [Users_RoleID_FK] FOREIGN KEY([RoleID])
REFERENCES [dbo].[UserRoles] ([RoleID])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [Users_RoleID_FK]
GO
USE [master]
GO
ALTER DATABASE [NotesMarketPlace] SET  READ_WRITE 
GO
