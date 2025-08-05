/****** Object:  Table [dbo].[institucionBancaria]    Script Date: 03/07/2025 20:38:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[institucionBancaria](
	[idInstBancaria] [int] NOT NULL,
	[codigoIdentidad] [int] NOT NULL,
	[codigoIBAN] [int] NOT NULL,
	[cedulaJuridica] [nvarchar](30) NOT NULL,
	[direccionSedePrincipal] [nvarchar](100) NOT NULL,
	[telefono] [int] NOT NULL,
	[estadoSolicitud] [nvarchar](20) NOT NULL,
	[correoElectronico] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[idInstBancaria] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO