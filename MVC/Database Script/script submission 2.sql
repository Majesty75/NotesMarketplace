USE [master]
GO
/****** Object:  Database [NotesMarketPlace]    Script Date: 4/7/2021 11:56:24 AM ******/
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
/****** Object:  Table [dbo].[Countries]    Script Date: 4/7/2021 11:56:24 AM ******/
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
/****** Object:  Table [dbo].[Downloads]    Script Date: 4/7/2021 11:56:24 AM ******/
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
	[DownloadDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[IsPaid] [bit] NOT NULL,
	[NoteTitle] [nvarchar](100) NULL,
	[NoteCategory] [int] NOT NULL,
	[PurchasedPrice] [decimal](18, 0) NULL,
 CONSTRAINT [PK__Download__73D5A710513804D5] PRIMARY KEY CLUSTERED 
(
	[DownloadID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NoteCategories]    Script Date: 4/7/2021 11:56:24 AM ******/
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
/****** Object:  Table [dbo].[Notes]    Script Date: 4/7/2021 11:56:25 AM ******/
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
/****** Object:  Table [dbo].[NotesAttachments]    Script Date: 4/7/2021 11:56:25 AM ******/
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
/****** Object:  Table [dbo].[NotesReports]    Script Date: 4/7/2021 11:56:25 AM ******/
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
/****** Object:  Table [dbo].[NotesReviews]    Script Date: 4/7/2021 11:56:25 AM ******/
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
/****** Object:  Table [dbo].[NoteTypes]    Script Date: 4/7/2021 11:56:25 AM ******/
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
/****** Object:  Table [dbo].[ReferenceData]    Script Date: 4/7/2021 11:56:25 AM ******/
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
/****** Object:  Table [dbo].[SystemConfigurations]    Script Date: 4/7/2021 11:56:25 AM ******/
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
/****** Object:  Table [dbo].[UserProfile]    Script Date: 4/7/2021 11:56:25 AM ******/
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
	[AddressLine2] [nvarchar](100) NULL,
	[City] [nvarchar](50) NOT NULL,
	[State] [nvarchar](50) NOT NULL,
	[ZipCode] [nvarchar](50) NOT NULL,
	[Country] [int] NOT NULL,
	[University] [nvarchar](100) NULL,
	[College] [nvarchar](100) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK__UserProf__290C888440E711E6] PRIMARY KEY CLUSTERED 
(
	[ProfileID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRoles]    Script Date: 4/7/2021 11:56:25 AM ******/
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
/****** Object:  Table [dbo].[Users]    Script Date: 4/7/2021 11:56:25 AM ******/
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
	[GUID] [nvarchar](max) NULL,
 CONSTRAINT [PK__Users__1788CCACD65A9E1A] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Countries] ON 

INSERT [dbo].[Countries] ([CountryID], [CountryName], [CountryCode], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (1, N'India', N'+91', 1, 1, CAST(N'2021-02-16T23:29:20.547' AS DateTime), 1, CAST(N'2021-02-16T23:29:20.547' AS DateTime))
INSERT [dbo].[Countries] ([CountryID], [CountryName], [CountryCode], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (3, N'USA', N'+1', 1, 1, CAST(N'2021-03-05T01:57:12.113' AS DateTime), 1, CAST(N'2021-03-05T01:57:12.113' AS DateTime))
INSERT [dbo].[Countries] ([CountryID], [CountryName], [CountryCode], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (4, N'Italy', N'+39', 1, 1, NULL, 1, CAST(N'2021-03-05T01:58:32.373' AS DateTime))
SET IDENTITY_INSERT [dbo].[Countries] OFF
GO
SET IDENTITY_INSERT [dbo].[Downloads] ON 

INSERT [dbo].[Downloads] ([DownloadID], [NoteID], [SellerID], [BuyerID], [IsAllowed], [AttachmentsID], [IsDownloaded], [DownloadDate], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsPaid], [NoteTitle], [NoteCategory], [PurchasedPrice]) VALUES (1, 1, 4, 1, 1, 1, 0, NULL, 1, CAST(N'2021-02-17T00:18:43.260' AS DateTime), 4, CAST(N'2021-03-26T18:49:03.797' AS DateTime), 1, N'First Book', 1, CAST(1234 AS Decimal(18, 0)))
INSERT [dbo].[Downloads] ([DownloadID], [NoteID], [SellerID], [BuyerID], [IsAllowed], [AttachmentsID], [IsDownloaded], [DownloadDate], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsPaid], [NoteTitle], [NoteCategory], [PurchasedPrice]) VALUES (2, 4, 20, 4, 1, 4, 1, CAST(N'2021-04-03T13:32:04.987' AS DateTime), 1, CAST(N'2021-03-08T05:29:20.353' AS DateTime), 20, CAST(N'2021-04-03T13:32:04.987' AS DateTime), 1, N'The Principal of Computer Hardware', 1, CAST(25 AS Decimal(18, 0)))
INSERT [dbo].[Downloads] ([DownloadID], [NoteID], [SellerID], [BuyerID], [IsAllowed], [AttachmentsID], [IsDownloaded], [DownloadDate], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsPaid], [NoteTitle], [NoteCategory], [PurchasedPrice]) VALUES (3, 1, 20, 4, 1, 1, 1, CAST(N'2021-04-04T15:50:56.237' AS DateTime), 20, CAST(N'2021-03-26T17:51:44.073' AS DateTime), 4, CAST(N'2021-04-04T15:50:56.237' AS DateTime), 1, N'First Book', 1, CAST(1234 AS Decimal(18, 0)))
INSERT [dbo].[Downloads] ([DownloadID], [NoteID], [SellerID], [BuyerID], [IsAllowed], [AttachmentsID], [IsDownloaded], [DownloadDate], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsPaid], [NoteTitle], [NoteCategory], [PurchasedPrice]) VALUES (4, 6, 20, 4, 0, 12, 0, NULL, 20, CAST(N'2021-03-26T18:20:15.207' AS DateTime), 20, CAST(N'2021-03-26T18:20:15.207' AS DateTime), 1, N'12th Accounts Textbook 10th edition', 4, CAST(250 AS Decimal(18, 0)))
INSERT [dbo].[Downloads] ([DownloadID], [NoteID], [SellerID], [BuyerID], [IsAllowed], [AttachmentsID], [IsDownloaded], [DownloadDate], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsPaid], [NoteTitle], [NoteCategory], [PurchasedPrice]) VALUES (5, 10, 4, 20, 1, 5, 1, CAST(N'2021-04-03T23:36:26.620' AS DateTime), 20, CAST(N'2021-03-26T18:28:51.513' AS DateTime), 20, CAST(N'2021-04-03T23:36:26.620' AS DateTime), 0, N'Testing Add Notes', 4, CAST(0 AS Decimal(18, 0)))
INSERT [dbo].[Downloads] ([DownloadID], [NoteID], [SellerID], [BuyerID], [IsAllowed], [AttachmentsID], [IsDownloaded], [DownloadDate], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsPaid], [NoteTitle], [NoteCategory], [PurchasedPrice]) VALUES (7, 13, 20, 4, 1, 8, 1, CAST(N'2021-04-03T14:03:11.737' AS DateTime), 4, CAST(N'2021-03-26T20:32:47.993' AS DateTime), 4, CAST(N'2021-04-03T14:03:11.737' AS DateTime), 1, N'Cosmic Catastrophes: Exploding Stars And Black Holes', 1, CAST(10 AS Decimal(18, 0)))
INSERT [dbo].[Downloads] ([DownloadID], [NoteID], [SellerID], [BuyerID], [IsAllowed], [AttachmentsID], [IsDownloaded], [DownloadDate], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsPaid], [NoteTitle], [NoteCategory], [PurchasedPrice]) VALUES (12, 1, 20, 4, 0, 1, 0, NULL, 4, CAST(N'2021-04-03T13:51:07.560' AS DateTime), 4, CAST(N'2021-04-03T13:51:07.560' AS DateTime), 1, N'First Book', 1, CAST(1234 AS Decimal(18, 0)))
SET IDENTITY_INSERT [dbo].[Downloads] OFF
GO
SET IDENTITY_INSERT [dbo].[NoteCategories] ON 

INSERT [dbo].[NoteCategories] ([CategoryID], [CategoryName], [CategoryDescription], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (1, N'Science', N'notes related to science field', 1, 1, CAST(N'2021-02-17T00:08:04.407' AS DateTime), 1, CAST(N'2021-02-17T00:08:04.407' AS DateTime))
INSERT [dbo].[NoteCategories] ([CategoryID], [CategoryName], [CategoryDescription], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (3, N'IT', N'Notes with instructions for IT people.', 1, 1, CAST(N'2021-03-05T01:56:03.157' AS DateTime), 1, CAST(N'2021-03-05T01:56:03.157' AS DateTime))
INSERT [dbo].[NoteCategories] ([CategoryID], [CategoryName], [CategoryDescription], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (4, N'Accounts', N'Accounts notes', 1, 1, CAST(N'2021-03-05T01:56:30.443' AS DateTime), 1, CAST(N'2021-03-05T01:56:30.443' AS DateTime))
INSERT [dbo].[NoteCategories] ([CategoryID], [CategoryName], [CategoryDescription], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (5, N'Mythology', N'Mythological content', 1, 1, CAST(N'2021-03-10T11:57:00.000' AS DateTime), 1, CAST(N'2021-03-10T11:57:00.000' AS DateTime))
SET IDENTITY_INSERT [dbo].[NoteCategories] OFF
GO
SET IDENTITY_INSERT [dbo].[Notes] ON 

INSERT [dbo].[Notes] ([NoteID], [Title], [Category], [DisplayPicture], [NoteDescription], [NoteType], [NumberOfPages], [PublishedDate], [University], [Country], [Course], [CourseCode], [Professor], [IsPaid], [Price], [Preview], [IsActive], [SellerID], [NoteStatus], [ActionBy], [AdminRemarks], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (1, N'First Book', 1, NULL, N'this is trial book', 1, NULL, CAST(N'2000-11-02T00:00:00.000' AS DateTime), N'abcd', 1, N'Trial Course', N'CO123', N'dr.strange', 1, CAST(1234 AS Decimal(18, 0)), NULL, 1, 20, 3, 1, N'Ok', 1, CAST(N'2021-02-17T00:13:40.307' AS DateTime), 1, CAST(N'2021-02-17T00:13:40.307' AS DateTime))
INSERT [dbo].[Notes] ([NoteID], [Title], [Category], [DisplayPicture], [NoteDescription], [NoteType], [NumberOfPages], [PublishedDate], [University], [Country], [Course], [CourseCode], [Professor], [IsPaid], [Price], [Preview], [IsActive], [SellerID], [NoteStatus], [ActionBy], [AdminRemarks], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (3, N'Computer Science', 1, N'/Content/NotesData/3/CoverPage-3.png', N'Lorem ipsum dolor sit amet, consectetur elit. Quae, sit mollitia voluptatum ad aperiam maiores natus debitis consectetur, molestiae recusandae voluptate, nobis? Quod nisi, ad quaerat dignissimos dicta, earum! Vero!', 1, 227, CAST(N'2020-11-25T00:00:00.000' AS DateTime), N'University of California', 3, N'Computer Engineering', N'248705', N'Mr.Richard Brown', 1, CAST(15 AS Decimal(18, 0)), N'/NotesData/3/Preview-3.pdf', 1, 4, 3, 1, N'Ok', 1, CAST(N'2021-03-05T02:09:36.457' AS DateTime), 1, CAST(N'2021-03-05T02:09:36.457' AS DateTime))
INSERT [dbo].[Notes] ([NoteID], [Title], [Category], [DisplayPicture], [NoteDescription], [NoteType], [NumberOfPages], [PublishedDate], [University], [Country], [Course], [CourseCode], [Professor], [IsPaid], [Price], [Preview], [IsActive], [SellerID], [NoteStatus], [ActionBy], [AdminRemarks], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (4, N'The Principal of Computer Hardware', 1, N'/Content/NotesData/4/CoverPage-4.png', N'Book about Processor and stuff.', 2, 356, CAST(N'2021-02-20T13:56:24.000' AS DateTime), N'GTU', 1, N'Electronics Engineering', N'EL472', N'Ms. Hannah Baker', 0, CAST(0 AS Decimal(18, 0)), N'/NotesData/4/Preview-4.pdf', 1, 20, 4, 1, N'OK', 1, CAST(N'2021-03-05T02:14:57.853' AS DateTime), 1, CAST(N'2021-03-05T02:14:57.853' AS DateTime))
INSERT [dbo].[Notes] ([NoteID], [Title], [Category], [DisplayPicture], [NoteDescription], [NoteType], [NumberOfPages], [PublishedDate], [University], [Country], [Course], [CourseCode], [Professor], [IsPaid], [Price], [Preview], [IsActive], [SellerID], [NoteStatus], [ActionBy], [AdminRemarks], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (6, N'12th Accounts Textbook 10th edition', 4, N'/Content/NotesData/6/CoverPage-6.png', N'12th Accounts Textbook by GHSEB ', 3, 650, CAST(N'2016-08-26T00:00:00.000' AS DateTime), N'GHSEB', 1, N'12th Commerce ', N'ACC012', N'Jitunbhai', 1, CAST(250 AS Decimal(18, 0)), NULL, 1, 20, 4, 1, NULL, 1, CAST(N'2021-03-05T02:18:36.980' AS DateTime), 1, CAST(N'2021-03-05T02:18:36.980' AS DateTime))
INSERT [dbo].[Notes] ([NoteID], [Title], [Category], [DisplayPicture], [NoteDescription], [NoteType], [NumberOfPages], [PublishedDate], [University], [Country], [Course], [CourseCode], [Professor], [IsPaid], [Price], [Preview], [IsActive], [SellerID], [NoteStatus], [ActionBy], [AdminRemarks], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (10, N'Note Title Dummy', 4, N'/Content/NotesData/10/CoverPage-10.png', N'Final Test.', 1, 1, CAST(N'2000-02-02T03:15:00.000' AS DateTime), N'NYU', 3, N'Test And Debug', N'404', N'Debugger', 0, CAST(0 AS Decimal(18, 0)), N'/NotesData/10/Preview-10.pdf', 1, 4, 3, 1, N'OK', 4, CAST(N'2021-06-02T00:00:00.000' AS DateTime), 4, CAST(N'2021-02-02T00:00:00.000' AS DateTime))
INSERT [dbo].[Notes] ([NoteID], [Title], [Category], [DisplayPicture], [NoteDescription], [NoteType], [NumberOfPages], [PublishedDate], [University], [Country], [Course], [CourseCode], [Professor], [IsPaid], [Price], [Preview], [IsActive], [SellerID], [NoteStatus], [ActionBy], [AdminRemarks], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (11, N'Testing Add Notes 2', 3, N'/Content/NotesData/11/CoverPage-11.PNG', N'Just Testing', 2, 102, NULL, N'NFA', 3, N'Test And Debug', N'404', N'Debugger', 1, CAST(102 AS Decimal(18, 0)), N'/NotesData/11/Preview-11.pdf', 1, 4, 1, NULL, NULL, 4, CAST(N'2021-03-08T06:46:53.857' AS DateTime), 4, CAST(N'2021-03-08T06:46:53.857' AS DateTime))
INSERT [dbo].[Notes] ([NoteID], [Title], [Category], [DisplayPicture], [NoteDescription], [NoteType], [NumberOfPages], [PublishedDate], [University], [Country], [Course], [CourseCode], [Professor], [IsPaid], [Price], [Preview], [IsActive], [SellerID], [NoteStatus], [ActionBy], [AdminRemarks], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (12, N'The Basic Computer Harware', 3, N'/Content/NotesData/12/CoverPage-12.PNG', N'This Note is about nvidia GPU architectures.', 2, 25, NULL, N'IIT Delhi', 1, N'Computer Architecture', N'CP303', N'N.M.Patel', 1, CAST(100 AS Decimal(18, 0)), N'/NotesData/12/Preview-12.pdf', 1, 4, 3, NULL, NULL, 4, CAST(N'2021-03-09T15:59:28.020' AS DateTime), 4, CAST(N'2021-03-09T15:59:28.020' AS DateTime))
INSERT [dbo].[Notes] ([NoteID], [Title], [Category], [DisplayPicture], [NoteDescription], [NoteType], [NumberOfPages], [PublishedDate], [University], [Country], [Course], [CourseCode], [Professor], [IsPaid], [Price], [Preview], [IsActive], [SellerID], [NoteStatus], [ActionBy], [AdminRemarks], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (13, N'Cosmic Catastrophes: Exploding Stars And Black Holes', 1, N'/Content/NotesData/13/CoverPage-13.jpg', N'It seems that book name and content does not match up well.', 1, 6, CAST(N'2020-02-01T00:00:00.000' AS DateTime), N'BVM', 1, N'Python Programming', N'CP460', N'Prof. UKJ', 1, CAST(10 AS Decimal(18, 0)), N'/NotesData/13/Preview-13.pdf', 1, 20, 3, 3, N'Ok', 20, CAST(N'2021-03-18T14:43:24.950' AS DateTime), 20, CAST(N'2021-03-18T14:43:24.950' AS DateTime))
INSERT [dbo].[Notes] ([NoteID], [Title], [Category], [DisplayPicture], [NoteDescription], [NoteType], [NumberOfPages], [PublishedDate], [University], [Country], [Course], [CourseCode], [Professor], [IsPaid], [Price], [Preview], [IsActive], [SellerID], [NoteStatus], [ActionBy], [AdminRemarks], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (14, N'The Indian Economics', 4, N'/Content/NotesData/14/CoverPage-14.png', N'Nothing Much here just beta testing', 2, 560, CAST(N'2020-01-02T00:00:00.000' AS DateTime), N'NONAME', 3, N'NOCOURSE', N'FF0000', N'Dan Brown', 1, CAST(50 AS Decimal(18, 0)), N'/NotesData/14/Preview-14.pdf', 1, 20, 1, 4, N'Ok', 20, CAST(N'2021-03-23T22:34:20.493' AS DateTime), 20, CAST(N'2021-04-06T00:53:28.633' AS DateTime))
INSERT [dbo].[Notes] ([NoteID], [Title], [Category], [DisplayPicture], [NoteDescription], [NoteType], [NumberOfPages], [PublishedDate], [University], [Country], [Course], [CourseCode], [Professor], [IsPaid], [Price], [Preview], [IsActive], [SellerID], [NoteStatus], [ActionBy], [AdminRemarks], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (16, N'Testing Add Notes After Adding NoteActions', 5, N'/Content/NotesData/16/CoverPage-16.png', N'Just Testing Stuff. And also thinking how much of time does unit testing  this page will take.', 1, 6, NULL, N'NONAME', 4, N'Python Programming', N'CP460', N'Dan Brown', 0, CAST(0 AS Decimal(18, 0)), N'/Content/NotesData/16/Preview-16.pdf', 1, 20, 0, NULL, NULL, 20, CAST(N'2021-04-06T00:53:19.173' AS DateTime), 20, CAST(N'2021-04-06T00:53:19.173' AS DateTime))
SET IDENTITY_INSERT [dbo].[Notes] OFF
GO
SET IDENTITY_INSERT [dbo].[NotesAttachments] ON 

INSERT [dbo].[NotesAttachments] ([AttachementID], [NoteID], [FilesName], [FilePath], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (1, 1, N'Attachment-1-1.pdf', N'20/Notes/1', 1, 1, CAST(N'2021-02-17T00:26:47.250' AS DateTime), 1, CAST(N'2021-02-17T00:26:47.250' AS DateTime))
INSERT [dbo].[NotesAttachments] ([AttachementID], [NoteID], [FilesName], [FilePath], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (5, 10, N'Attachment-5-1.pdf;', N'4/Notes/10/', 1, 4, CAST(N'2021-03-02T00:00:00.000' AS DateTime), 4, CAST(N'2021-03-02T00:00:00.000' AS DateTime))
INSERT [dbo].[NotesAttachments] ([AttachementID], [NoteID], [FilesName], [FilePath], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (6, 11, N'Attachment-6-1.pdf;', N'4/Notes/11/', 1, 4, CAST(N'2021-03-08T06:46:54.087' AS DateTime), 4, CAST(N'2021-03-08T06:46:54.087' AS DateTime))
INSERT [dbo].[NotesAttachments] ([AttachementID], [NoteID], [FilesName], [FilePath], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (7, 12, N'Attachment-7-1.pdf;', N'4/Notes/12/', 1, 4, CAST(N'2021-03-09T15:59:28.523' AS DateTime), 4, CAST(N'2021-03-09T15:59:28.523' AS DateTime))
INSERT [dbo].[NotesAttachments] ([AttachementID], [NoteID], [FilesName], [FilePath], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (8, 13, N'Attachment-13-1.pdf;', N'20/Notes/13/', 1, 20, CAST(N'2021-03-18T14:43:25.400' AS DateTime), 20, CAST(N'2021-03-18T14:43:25.400' AS DateTime))
INSERT [dbo].[NotesAttachments] ([AttachementID], [NoteID], [FilesName], [FilePath], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (9, 14, N'Attachment-14-1.pdf;Attachment-14-2.pdf;Attachment-14-3.pdf;', N'20/Notes/14/', 1, 20, CAST(N'2021-03-23T22:34:21.107' AS DateTime), 20, CAST(N'2021-04-06T00:01:38.940' AS DateTime))
INSERT [dbo].[NotesAttachments] ([AttachementID], [NoteID], [FilesName], [FilePath], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (10, 3, N'Attachment-3-1.pdf;', N'4/Notes/3/', 1, 1, CAST(N'2021-03-26T18:17:04.870' AS DateTime), 1, CAST(N'2021-03-26T18:17:04.870' AS DateTime))
INSERT [dbo].[NotesAttachments] ([AttachementID], [NoteID], [FilesName], [FilePath], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (11, 4, N'Attachment-4-1.pdf;', N'3/Notes/4/', 1, 1, CAST(N'2021-03-26T18:17:47.243' AS DateTime), 1, CAST(N'2021-03-26T18:17:47.243' AS DateTime))
INSERT [dbo].[NotesAttachments] ([AttachementID], [NoteID], [FilesName], [FilePath], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (12, 6, N'Attachment-6-1.pdf;', N'1/Notes/6/', 1, 1, CAST(N'2021-03-26T18:18:14.287' AS DateTime), 1, CAST(N'2021-03-26T18:18:14.287' AS DateTime))
INSERT [dbo].[NotesAttachments] ([AttachementID], [NoteID], [FilesName], [FilePath], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (14, 16, N'Attachment-16-1.pdf;', N'20/Notes/16/', 1, 20, CAST(N'2021-04-06T00:53:19.227' AS DateTime), 20, CAST(N'2021-04-06T00:53:19.227' AS DateTime))
SET IDENTITY_INSERT [dbo].[NotesAttachments] OFF
GO
SET IDENTITY_INSERT [dbo].[NotesReports] ON 

INSERT [dbo].[NotesReports] ([ReportID], [NoteID], [BuyerID], [DownloadID], [Remarks], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (1, 1, 1, 1, N'remarks', 1, CAST(N'2021-02-17T00:23:40.703' AS DateTime), 1, CAST(N'2021-02-17T00:23:40.703' AS DateTime))
INSERT [dbo].[NotesReports] ([ReportID], [NoteID], [BuyerID], [DownloadID], [Remarks], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (7, 10, 20, 5, N'Nothing Personal.', 20, CAST(N'2021-04-04T12:46:20.310' AS DateTime), 20, CAST(N'2021-04-04T12:46:20.310' AS DateTime))
INSERT [dbo].[NotesReports] ([ReportID], [NoteID], [BuyerID], [DownloadID], [Remarks], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (8, 13, 4, 7, N'No offense but ye note se ye ummed nahi thi.', 4, CAST(N'2021-04-04T16:26:19.047' AS DateTime), 4, CAST(N'2021-04-04T16:26:19.050' AS DateTime))
SET IDENTITY_INSERT [dbo].[NotesReports] OFF
GO
SET IDENTITY_INSERT [dbo].[NotesReviews] ON 

INSERT [dbo].[NotesReviews] ([ReviewID], [NoteID], [BuyerID], [DownloadID], [Rating], [Comment], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (1, 1, 1, 1, CAST(4 AS Decimal(18, 0)), N'amazing note', 1, CAST(N'2021-02-17T00:20:42.553' AS DateTime), 1, CAST(N'2021-02-17T00:20:42.553' AS DateTime))
INSERT [dbo].[NotesReviews] ([ReviewID], [NoteID], [BuyerID], [DownloadID], [Rating], [Comment], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (2, 1, 1, 1, CAST(2 AS Decimal(18, 0)), N'Lorem ipsum dolor sit amet, consectetur elit. Quae, sit mollitia voluptatum ad aperiam maiores natus debitis consectetur, molestiae recusandae voluptate, nobis? Quod nisi, ad quaerat dignissimos dicta, earum! Vero!', 1, CAST(N'2021-03-07T11:37:34.630' AS DateTime), 1, CAST(N'2021-03-07T11:37:34.630' AS DateTime))
INSERT [dbo].[NotesReviews] ([ReviewID], [NoteID], [BuyerID], [DownloadID], [Rating], [Comment], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (4, 1, 1, 1, CAST(3 AS Decimal(18, 0)), N'Lorem ipsum dolor sit amet, consectetur elit. Quae, sit mollitia voluptatum ad aperiam maiores natus debitis consectetur, molestiae recusandae voluptate, nobis? Quod nisi, ad quaerat dignissimos dicta, earum! Vero!', 1, CAST(N'2021-03-07T11:37:57.823' AS DateTime), 1, CAST(N'2021-03-07T11:37:57.823' AS DateTime))
INSERT [dbo].[NotesReviews] ([ReviewID], [NoteID], [BuyerID], [DownloadID], [Rating], [Comment], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (5, 1, 4, 1, CAST(5 AS Decimal(18, 0)), N'Lorem ipsum dolor sit amet, consectetur elit. Quae, sit mollitia voluptatum ad aperiam maiores natus debitis consectetur, molestiae recusandae voluptate, nobis? Quod nisi, ad quaerat dignissimos dicta, earum! Vero!', 1, CAST(N'2021-03-07T12:00:31.640' AS DateTime), 1, CAST(N'2021-03-07T12:00:31.640' AS DateTime))
INSERT [dbo].[NotesReviews] ([ReviewID], [NoteID], [BuyerID], [DownloadID], [Rating], [Comment], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (10, 10, 20, 5, CAST(5 AS Decimal(18, 0)), N'Testing Add Review Feature.', 20, CAST(N'2021-04-03T20:18:25.313' AS DateTime), 20, CAST(N'2021-04-04T12:06:15.927' AS DateTime))
INSERT [dbo].[NotesReviews] ([ReviewID], [NoteID], [BuyerID], [DownloadID], [Rating], [Comment], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (11, 13, 4, 7, CAST(4 AS Decimal(18, 0)), N'abcd, efghi, jkl, mnooo', 4, CAST(N'2021-04-04T16:28:30.560' AS DateTime), 4, CAST(N'2021-04-04T16:28:30.560' AS DateTime))
SET IDENTITY_INSERT [dbo].[NotesReviews] OFF
GO
SET IDENTITY_INSERT [dbo].[NoteTypes] ON 

INSERT [dbo].[NoteTypes] ([TypeID], [TypeName], [TypeDescription], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (1, N'Class Notes', N'Hand written notes from classes', 1, 1, CAST(N'2021-02-17T00:11:54.753' AS DateTime), 1, CAST(N'2021-02-17T00:11:54.753' AS DateTime))
INSERT [dbo].[NoteTypes] ([TypeID], [TypeName], [TypeDescription], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (2, N'Digital Notes', N'Notes created on computers', 1, 1, CAST(N'2021-03-05T01:54:09.073' AS DateTime), 1, CAST(N'2021-03-05T01:54:09.073' AS DateTime))
INSERT [dbo].[NoteTypes] ([TypeID], [TypeName], [TypeDescription], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (3, N'Old TextBook', N'Old Textbook for sale', 1, 1, CAST(N'2021-03-05T01:55:20.840' AS DateTime), 1, CAST(N'2021-03-05T01:55:20.840' AS DateTime))
SET IDENTITY_INSERT [dbo].[NoteTypes] OFF
GO
SET IDENTITY_INSERT [dbo].[ReferenceData] ON 

INSERT [dbo].[ReferenceData] ([DataID], [DataKey], [DataValue], [RefCategory], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (1, N'M', N'Male', N'gender', 1, 1, CAST(N'2021-02-16T23:54:22.970' AS DateTime), 1, CAST(N'2021-02-16T23:54:22.970' AS DateTime))
INSERT [dbo].[ReferenceData] ([DataID], [DataKey], [DataValue], [RefCategory], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (2, N'F', N'Female', N'gender', 1, 1, CAST(N'2021-02-16T23:55:15.527' AS DateTime), 1, CAST(N'2021-02-16T23:55:15.527' AS DateTime))
SET IDENTITY_INSERT [dbo].[ReferenceData] OFF
GO
SET IDENTITY_INSERT [dbo].[SystemConfigurations] ON 

INSERT [dbo].[SystemConfigurations] ([ConfigID], [ConfigKey], [ConfigValue], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (1, N'DefaultUserProfile', N'/SystemConfig/DefaultUserProfile.png', 1, 1, CAST(N'2021-02-17T00:30:17.580' AS DateTime), 1, CAST(N'2021-02-17T00:30:17.580' AS DateTime))
INSERT [dbo].[SystemConfigurations] ([ConfigID], [ConfigKey], [ConfigValue], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (2, N'DefaultNoteCoverPage', N'/SystemConfig/DefaultNoteCover.png', 1, 1, CAST(N'2021-03-05T01:45:31.490' AS DateTime), 1, CAST(N'2021-03-05T01:45:31.490' AS DateTime))
INSERT [dbo].[SystemConfigurations] ([ConfigID], [ConfigKey], [ConfigValue], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (3, N'TwitterLink', N'https://twitter.com/', 1, 1, CAST(N'2021-03-05T01:46:33.953' AS DateTime), 1, CAST(N'2021-03-05T01:46:33.953' AS DateTime))
INSERT [dbo].[SystemConfigurations] ([ConfigID], [ConfigKey], [ConfigValue], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (4, N'FacebookLink', N'https://www.facebook.com/facebook', 1, 1, CAST(N'2021-03-05T01:47:40.607' AS DateTime), 1, CAST(N'2021-03-05T01:47:40.607' AS DateTime))
INSERT [dbo].[SystemConfigurations] ([ConfigID], [ConfigKey], [ConfigValue], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (5, N'LinkedInLink', N'https://in.linkedin.com/', 1, 1, CAST(N'2021-03-05T01:48:26.433' AS DateTime), 1, CAST(N'2021-03-05T01:48:26.433' AS DateTime))
INSERT [dbo].[SystemConfigurations] ([ConfigID], [ConfigKey], [ConfigValue], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (6, N'MailAddress', N'goyaniyash75@gmail.com', 1, 1, CAST(N'2021-03-06T17:28:11.337' AS DateTime), 1, CAST(N'2021-03-06T17:28:11.337' AS DateTime))
INSERT [dbo].[SystemConfigurations] ([ConfigID], [ConfigKey], [ConfigValue], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (7, N'MailPassword', N'qhtujezfyhuatgal', 1, 1, CAST(N'2021-03-06T17:40:32.663' AS DateTime), 1, CAST(N'2021-03-06T17:40:32.663' AS DateTime))
INSERT [dbo].[SystemConfigurations] ([ConfigID], [ConfigKey], [ConfigValue], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (8, N'AdminEmails', N'yashgoyani101@gmail.com;goyaniyash75@gmail.com', 1, 1, CAST(N'2021-03-07T00:38:04.427' AS DateTime), 1, CAST(N'2021-03-07T00:38:04.427' AS DateTime))
INSERT [dbo].[SystemConfigurations] ([ConfigID], [ConfigKey], [ConfigValue], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (10, N'SupportContact', N'+919876543210', 1, 1, CAST(N'2021-03-23T20:09:44.347' AS DateTime), 1, CAST(N'2021-03-23T20:09:44.347' AS DateTime))
SET IDENTITY_INSERT [dbo].[SystemConfigurations] OFF
GO
SET IDENTITY_INSERT [dbo].[UserProfile] ON 

INSERT [dbo].[UserProfile] ([ProfileID], [UserID], [DOB], [Gender], [SecondaryEmailAddress], [CountryCode], [PhoneNo], [ProfilePicture], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [University], [College], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (1, 1, CAST(N'2000-01-01' AS Date), 1, N'superadmin2nd@notesmarketplace.com', N'+91', N'9876543210', N'/Assets/1/UserProfile-1$jfif', N'192.168.0.1', N'127.0.0.1', N'ssd', N'motherboard', N'000000', 1, N'lenovo menufacturing unit,china', N'somewhere in china', 1, CAST(N'2021-02-16T23:49:09.137' AS DateTime), 1, CAST(N'2021-02-16T23:49:09.137' AS DateTime))
INSERT [dbo].[UserProfile] ([ProfileID], [UserID], [DOB], [Gender], [SecondaryEmailAddress], [CountryCode], [PhoneNo], [ProfilePicture], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [University], [College], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (6, 4, NULL, 1, NULL, N'+91', N'7896540321', N'/Assets/4/UserProfile-4$jfif', N'1', N'2', N'2', N'2', N'265478', 3, N'University', N'College', 1, CAST(N'2021-03-05T01:42:51.213' AS DateTime), 1, CAST(N'2021-03-05T01:42:51.213' AS DateTime))
INSERT [dbo].[UserProfile] ([ProfileID], [UserID], [DOB], [Gender], [SecondaryEmailAddress], [CountryCode], [PhoneNo], [ProfilePicture], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [University], [College], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (8, 20, CAST(N'2000-06-16' AS Date), 1, NULL, N'+91', N'9874563102', N'/Assets/20/UserProfile-20$jpg', N'Gokuldham Society', N'Power Gully,Goregaon west', N'Power Gully,Goregaon west', N'Maharashtra', N'400065', 1, N'ABC', N'ABC', 1, CAST(N'2021-03-12T00:00:00.000' AS DateTime), 20, CAST(N'2021-03-27T17:42:46.817' AS DateTime))
INSERT [dbo].[UserProfile] ([ProfileID], [UserID], [DOB], [Gender], [SecondaryEmailAddress], [CountryCode], [PhoneNo], [ProfilePicture], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [University], [College], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (16, 21, CAST(N'2021-03-27' AS Date), 1, NULL, N'+91', N'7567485945', N'/Assets/21/UserProfile-21$jpeg', N'khodiyar sheri', N'rupaimata mandir pachhal', N'rupaimata mandir pachhal', N'ring road paravadi', N'364275', 1, NULL, NULL, 21, CAST(N'2021-03-27T19:02:27.677' AS DateTime), 21, CAST(N'2021-03-27T19:02:27.677' AS DateTime))
SET IDENTITY_INSERT [dbo].[UserProfile] OFF
GO
SET IDENTITY_INSERT [dbo].[UserRoles] ON 

INSERT [dbo].[UserRoles] ([RoleID], [RoleName], [RoleDescription], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (1, N'SuperAdmin', N'this user have all the permissions', 1, 1, CAST(N'2021-02-16T23:22:37.227' AS DateTime), 1, CAST(N'2021-02-16T23:22:37.227' AS DateTime))
INSERT [dbo].[UserRoles] ([RoleID], [RoleName], [RoleDescription], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (2, N'SubAdmin', N'This user has limited but more permission than normal user.', 1, 1, CAST(N'2021-02-16T23:28:15.380' AS DateTime), 1, CAST(N'2021-02-16T23:28:15.380' AS DateTime))
INSERT [dbo].[UserRoles] ([RoleID], [RoleName], [RoleDescription], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (3, N'NormalUser', N'This User has limited permissions', 1, 1, CAST(N'2021-03-05T01:26:38.567' AS DateTime), 1, CAST(N'2021-03-05T01:26:38.567' AS DateTime))
INSERT [dbo].[UserRoles] ([RoleID], [RoleName], [RoleDescription], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (4, N'UserProfileNotCreated', N'This User can only access userprofile and anonymous access pages ', 1, 1, CAST(N'2021-03-06T15:03:35.837' AS DateTime), 1, CAST(N'2021-03-06T15:03:35.837' AS DateTime))
SET IDENTITY_INSERT [dbo].[UserRoles] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([UserID], [Email], [FirstName], [LastName], [Passwd], [IsActive], [RoleID], [IsEmailVerified], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [GUID]) VALUES (1, N'superadmin@gmail.com', N'super', N'admin', N'@Toornimda1', 1, 1, 1, 1, CAST(N'2021-02-16T23:46:44.993' AS DateTime), 1, CAST(N'2021-02-16T23:46:44.993' AS DateTime), NULL)
INSERT [dbo].[Users] ([UserID], [Email], [FirstName], [LastName], [Passwd], [IsActive], [RoleID], [IsEmailVerified], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [GUID]) VALUES (3, N'dummymail@gmail.com', N'Dummy', N'Name', N'Password1', 1, 4, 1, 1, CAST(N'2021-03-05T01:28:25.597' AS DateTime), 1, CAST(N'2021-03-05T01:28:25.597' AS DateTime), NULL)
INSERT [dbo].[Users] ([UserID], [Email], [FirstName], [LastName], [Passwd], [IsActive], [RoleID], [IsEmailVerified], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [GUID]) VALUES (4, N'subadmin@gmail.com', N'Sub', N'Admin', N'PassWord2', 1, 3, 1, 1, CAST(N'2021-03-05T01:29:40.530' AS DateTime), 1, CAST(N'2021-03-05T01:29:40.530' AS DateTime), NULL)
INSERT [dbo].[Users] ([UserID], [Email], [FirstName], [LastName], [Passwd], [IsActive], [RoleID], [IsEmailVerified], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [GUID]) VALUES (20, N'yashgoyani101@gmail.com', N'Dummy', N'Name', N'P@sswd1', 1, 3, 1, 1, CAST(N'2021-03-05T00:00:00.000' AS DateTime), 20, CAST(N'2021-04-05T23:59:11.120' AS DateTime), NULL)
INSERT [dbo].[Users] ([UserID], [Email], [FirstName], [LastName], [Passwd], [IsActive], [RoleID], [IsEmailVerified], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [GUID]) VALUES (21, N'svayamgoyani90163@gmail.com', N'Swayam', N'Goyani', N'@P@ssword1', 1, 3, 1, 1, CAST(N'2021-03-27T18:36:40.313' AS DateTime), 21, CAST(N'2021-03-27T18:38:23.313' AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Countrie__5D9B0D2CFB7B8C68]    Script Date: 4/7/2021 11:56:25 AM ******/
ALTER TABLE [dbo].[Countries] ADD UNIQUE NONCLUSTERED 
(
	[CountryCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Users__A9D10534EC2BA4C4]    Script Date: 4/7/2021 11:56:25 AM ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [UQ__Users__A9D10534EC2BA4C4] UNIQUE NONCLUSTERED 
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
ALTER TABLE [dbo].[Notes]  WITH CHECK ADD  CONSTRAINT [Notes_Country_FK] FOREIGN KEY([Country])
REFERENCES [dbo].[Countries] ([CountryID])
GO
ALTER TABLE [dbo].[Notes] CHECK CONSTRAINT [Notes_Country_FK]
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
ALTER TABLE [dbo].[UserProfile]  WITH CHECK ADD  CONSTRAINT [UserProfile_Country_FK] FOREIGN KEY([Country])
REFERENCES [dbo].[Countries] ([CountryID])
GO
ALTER TABLE [dbo].[UserProfile] CHECK CONSTRAINT [UserProfile_Country_FK]
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
