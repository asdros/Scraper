SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ScrapItem] (
    [Id]             TINYINT      IDENTITY (1, 1) NOT NULL,
    [Date]           VARCHAR (20) NOT NULL,
    [Sunrise]        VARCHAR (10) NOT NULL,
    [Sunset]         VARCHAR (10) NOT NULL,
    [TempDay]        TINYINT      NOT NULL,
    [TempNight]      TINYINT      NOT NULL,
    [Pressure]       FLOAT		  NOT NULL,
    [RainFall]       SMALLINT	  NOT NULL,
    [MoonPhase]      VARCHAR (20) NOT NULL,
    [FishingQuality] VARCHAR (20) NOT NULL,
    [City]           VARCHAR (30) NULL,
 CONSTRAINT [PK_ScrapItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
