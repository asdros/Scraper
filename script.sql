USE [webScraper]
GO
/****** Object:  Table [dbo].[ScrapItem]    Script Date: 26.06.2020 00:54:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ScrapItem](
	[Id] [tinyint] IDENTITY(1,1) NOT NULL,
	[Date] [varchar](20) NOT NULL,
	[Sunrise] [varchar](10) NOT NULL,
	[Sunset] [varchar](10) NOT NULL,
	[TempDay] [varchar](10) NOT NULL,
	[TempNight] [varchar](10) NOT NULL,
	[Pressure] [varchar](10) NOT NULL,
	[RainFall] [varchar](10) NOT NULL,
	[MoonPhase] [varchar](20) NOT NULL,
	[FishingQuality] [varchar](20) NOT NULL,
	[City] [varchar](30) NULL,
 CONSTRAINT [PK_ScrapItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
