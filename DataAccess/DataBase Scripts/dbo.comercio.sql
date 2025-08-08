/****** Object:  Table [dbo].[comercio]    Script Date: 03/07/2025 20:35:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[comercio](
	[idComercio] [int] NOT NULL,
	[idCuenta] [int] NOT NULL,
	[nombre] [nvarchar](50) NOT NULL,
	[estadoSolicitud] [nvarchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[idComercio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[comercio]  WITH CHECK ADD FOREIGN KEY([idCuenta])
REFERENCES [dbo].[cuentaComercio] ([idCuenta])
GO